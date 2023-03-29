using System;
using System.IO;
using System.Security.Cryptography;
using System.Text;
using System.Collections.Generic;
using System.Runtime.InteropServices.WindowsRuntime;
using System.Text.RegularExpressions;
using System.Threading;
using System.Threading.Tasks;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.Security.Cryptography;
using Windows.Security.Cryptography.Core;
using Windows.Storage;
using Windows.Storage.Streams;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;
using Windows.UI.Popups;
using System.Diagnostics;
using System.Timers;

namespace FileManager.Views
{
    public sealed partial class Comparator : Page
    {
        private StorageFile file1;
        private byte[] byte1;
        private StorageFile file2;
        private byte[] byte2;
        public Comparator()
        {
            this.InitializeComponent();
        }

        private async void filePick(int i)
        {
            try
            {
                var picker = new Windows.Storage.Pickers.FileOpenPicker();
                picker.ViewMode = Windows.Storage.Pickers.PickerViewMode.Thumbnail;
                picker.SuggestedStartLocation = Windows.Storage.Pickers.PickerLocationId.PicturesLibrary;
                picker.FileTypeFilter.Add("*");
                StorageFile file = await picker.PickSingleFileAsync();
                if (i == 1)
                {
                    this.file1 = file;
                }
                else
                {
                    this.file2 = file;
                }
                dialogCommmand();
            } 
            catch(Exception ex)
            {
                MessageDialog dialog = new MessageDialog("An error occured" + ex.Message, "Exception");
                await dialog.ShowAsync();
            }
        }

        private async void dialogCommmand()
        {
            await new MessageDialog("Your file is uploaded", "Information").ShowAsync();
        }

        private async void generateByte(StorageFile file)
        {
            if (file != null)
            {
                using (Stream fileStr = await file.OpenStreamForReadAsync())
                {
                    byte[] bytes = new byte[fileStr.Length];

                    const int BUFFER_SIZE = 1024;
                    byte[] buffer = new byte[BUFFER_SIZE];
                    int position = 0;
                    int bytesread = 0;
                    while ((bytesread = await fileStr.ReadAsync(buffer, 0, BUFFER_SIZE)) > 0)
                        for (int i = 0; i < bytesread; i++, position++)
                            bytes[position] = buffer[i];
                    if (file.Equals(file1))
                    {
                        byte1 = new byte[bytes.Length];
                        Array.Copy(bytes, byte1, bytes.Length);
                    }
                    else
                    {
                        byte2 = new byte[bytes.Length];
                        Array.Copy(bytes, byte2, bytes.Length);
                    }
                }
            }
        }

        private async void generate_byte2(StorageFile file)
        {
            using (MemoryStream memStream = new MemoryStream())
            {
                using (Stream fileStr = await file.OpenStreamForReadAsync())
                {
                    await fileStr.CopyToAsync(memStream);
                    byte1 = memStream.ToArray();
                }
            }
        }

        private void upload_file1(object sender, RoutedEventArgs e)
        {
            if (this.file1 != null)
            {
                this.file1 = null;
            }
            this.filePick(1);
        }

        private void upload_file2(object sender, RoutedEventArgs e)
        {
            if (this.file2 != null)
            {
                this.file2 = null;
            }
            this.filePick(2);
        }

        private async void compare_files(object sender, RoutedEventArgs e)
        {
            try
            {
                if (this.file1 == null || this.file2 == null)
                {
                    return;
                }
                Stopwatch timer = new Stopwatch();
                timer.Start();
                TextBoxCompare.Text = "";

                Thread t1 = new Thread(() => this.generateByte(this.file1));
                Thread t2 = new Thread(() => this.generateByte(this.file2));

                t1.Start();
                t2.Start();

                t1.Join();
                t2.Join();

                Thread.Sleep(800);
                if (compare(byte1, byte2))
                {
                    TextBoxCompare.Text = "Files are the same";
                }
                else
                {
                    TextBoxCompare.Text = "Files are not the same";
                }
                timer.Stop();
                TimeSpan timeSpan = timer.Elapsed;
                string timeTaken = "Time taken: " + timeSpan.ToString(@"m\:ss\.fff");
                MessageDialog dialog = new MessageDialog(timeTaken, "Information");
                await dialog.ShowAsync();
            }
            catch(Exception ex)
            {
                MessageDialog dialog = new MessageDialog("An error occured" + ex.Message, "Exception");
                await dialog.ShowAsync();
            }
        }

        private bool compare(byte[] array1, byte[] array2)
        {
            if (array1.Length != array2.Length)
                return false;

            for (int i = 0; i < array1.Length; i++)
            {
                if (array1[i] != array2[i])
                {
                    return false;
                }
            }
            return true;
        }
        private async void Single_Thread_Compare(object sender, RoutedEventArgs e)
        {
            try
            {
                System.IO.FileStream fs = new System.IO.FileStream(file1.Path, System.IO.FileMode.Open, System.IO.FileAccess.Read, FileShare.ReadWrite);
                if (file1 == null || file2 == null)
                {
                    return;
                }

                Stopwatch stopwatch = new Stopwatch();
                stopwatch.Start();;
                TextBoxCompare.Text = "";
                
                
                    /*
                                    if (compare(byte1, byte2))
                                    {
                                        TextBoxCompare.Text = "Files are the same";
                                    }
                                    else
                                    {
                                        TextBoxCompare.Text = "Files are not the same";
                                    }*/
                stopwatch.Stop();
                TimeSpan ts = stopwatch.Elapsed;
                string elapsedTime = String.Format("{0:00}:{1:00}:{2:00}.{3:00}", ts.Hours, ts.Minutes, ts.Seconds, ts.Milliseconds / 10);
/*                foreach (var el in yes)
                {
                    TextBoxCompare.Text += el.ToString();
                }*/

                // The method is async because of this
                MessageDialog dialog = new MessageDialog(elapsedTime, "Information");
                await dialog.ShowAsync();
            }
            catch(Exception ex)
            {
                MessageDialog dialog = new MessageDialog("An error occured: " + ex.Message, "Exception");
                await dialog.ShowAsync();
            }
        }
        private string generateHash(string path)
        {
            var computedHash = HashAlgorithmNames.Md5;
            IBuffer buffUtf8Msg = CryptographicBuffer.ConvertStringToBinary(path, BinaryStringEncoding.Utf8);

            HashAlgorithmProvider objAlgProv = HashAlgorithmProvider.OpenAlgorithm(computedHash);
            computedHash = objAlgProv.AlgorithmName;

            IBuffer buffHash = objAlgProv.HashData(buffUtf8Msg);

            if (buffHash.Length != objAlgProv.HashLength)
            {
                throw new Exception("There was an error");
            }

            string hex = CryptographicBuffer.EncodeToHexString(buffHash);
            return hex;
        }

        private string generate_hash_md5(string path)
        {
            using (var md5 = MD5.Create())
            {
                using (var stream = File.OpenRead(path))
                {
                    var hash = md5.ComputeHash(stream);
                    return BitConverter.ToString(hash).Replace("-", "");
                }
            }
        }

        private string generate_hash_sha256(string path)
        {
            using (var sha256 = SHA256.Create())
            {
                using (var stream = File.OpenRead(path))
                {

                    byte[] bytes = sha256.ComputeHash(stream);
                    var sb = new StringBuilder();
                    for (int i = 0; i < bytes.Length; i++)
                    {
                        sb.Append(bytes[i].ToString("x2"));
                    }
                    return sb.ToString();
                }

            }
        }

    }
}
