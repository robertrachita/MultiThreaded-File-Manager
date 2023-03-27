using System;
using System.Collections.Generic;
using System.IO;
using System.Runtime.InteropServices.WindowsRuntime;
using System.Security.Cryptography;
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
        }

        private async void generateHash(StorageFile file)
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

        private async void generate_hash2(StorageFile file)
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

        private void compare_files(object sender, RoutedEventArgs e)
        {
            if (this.file1 == null || this.file2 == null)
            {
                return;
            }
            TextBoxCompare.Text = "";

            Thread t1 = new Thread(() => this.generateHash(this.file1));
            Thread t2 = new Thread(() => this.generateHash(this.file2));

            t1.Start();
            t2.Start();

            t1.Join();
            t2.Join();

            Thread.Sleep(1000);
            if (compare(byte1, byte2))
            {
                TextBoxCompare.Text = "Files are the same";
            }
            else
            {
                TextBoxCompare.Text = "Files are not the same";
            }
            //this.generateHash(this.file1);
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

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            File.OpenRead(this.file1.Name);
        }
    }
}
