using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
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
        private byte[] byte1;
        private byte[] byte2;
        public Comparator()
        {
            this.InitializeComponent();
        }

        private async void upload_file1(object sender, RoutedEventArgs e)
        {
            if (byte1 != null)
            {
                Array.Clear(byte1, 0, byte1.Length);
            }

            var picker = new Windows.Storage.Pickers.FileOpenPicker();
            picker.SuggestedStartLocation = Windows.Storage.Pickers.PickerLocationId.PicturesLibrary;
            picker.FileTypeFilter.Add("*");
            using (Stream fileStr = await (await picker.PickSingleFileAsync()).OpenStreamForReadAsync())
            {
                byte[] bytes = new byte[fileStr.Length];
                const int BUFFER_SIZE = 1024;
                byte[] buffer = new byte[BUFFER_SIZE];
                int position = 0;
                int bytesread = 0;
                while ((bytesread = await fileStr.ReadAsync(buffer, 0, BUFFER_SIZE)) > 0)
                    for (int i = 0; i < bytesread; i++, position++)
                        bytes[position] = buffer[i];

                byte1 = new byte[bytes.Length];
                Array.Copy(bytes, byte1, bytes.Length);
                TextBoxCompare.Text = "";
                foreach (var el in byte1)
                {
                    TextBoxCompare.Text += el.ToString();
                }
            }
        }
        private async void upload_file2(object sender, RoutedEventArgs e)
        {
            if (byte2 != null)
            {
                Array.Clear(byte2, 0, byte2.Length);
            }

            var picker = new Windows.Storage.Pickers.FileOpenPicker();
            picker.SuggestedStartLocation = Windows.Storage.Pickers.PickerLocationId.PicturesLibrary;
            picker.FileTypeFilter.Add("*");
            using (Stream fileStr = await (await picker.PickSingleFileAsync()).OpenStreamForReadAsync())
            {
                byte[] bytes = new byte[fileStr.Length];
                const int BUFFER_SIZE = 1024;
                byte[] buffer = new byte[BUFFER_SIZE];
                int position = 0;
                int bytesread = 0;
                while ((bytesread = await fileStr.ReadAsync(buffer, 0, BUFFER_SIZE)) > 0)
                    for (int i = 0; i < bytesread; i++, position++)
                        bytes[position] = buffer[i];

                byte2 = new byte[bytes.Length];
                Array.Copy(bytes, byte2, bytes.Length);
                TextBoxCompare.Text = "";
                foreach (var el in byte2)
                {
                    TextBoxCompare.Text += el.ToString();
                }
            }
        }

        private void compare_files(object sender, RoutedEventArgs e)
        {
            TextBoxCompare.Text = "";
            /*            Thread thread1 = new Thread(() => getByte(byte1, file1));
                        Thread thread2 = new Thread(() => getByte(byte2, file2));
                        thread1.Start();
                        thread2.Start();
                        thread1.Join();
                        thread2.Join();*/
            TextBoxCompare.Text = compare(byte1, byte2).ToString();
            /*
                        if (compare(byte1, byte2) == true)
                        {
                            TextBoxCompare.Text = "Files are the same";
                        }
                        else if (compare(byte1, byte2) == false) 
                        {
                            TextBoxCompare.Text = "Files are not the same";
                        }*/
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

        /*        private async Task getByte(byte[] array, StorageFile file)
                {
                    Array.Clear(byte1, 0, byte1.Length);
                    Array.Clear(byte2, 0, byte2.Length);
                    using (Stream fileStr = await (file.OpenStreamForReadAsync()))
                    {
                        byte[] bit = new byte[fileStr.Length];
                        const int BUFFER_SIZE = 1024;
                        byte[] buffer = new byte[BUFFER_SIZE];
                        int position = 0;
                        int bytesread = 0;
                        while ((bytesread = await fileStr.ReadAsync(buffer, 0, BUFFER_SIZE)) > 0)
                            for (int i = 0; i < bytesread; i++, position++)
                                bit[position] = buffer[i];
                        array = bit;
                    }
                }*/

        private async void pickAFile()
        {
            TextBoxCompare.Text = "";
            var picker = new Windows.Storage.Pickers.FileOpenPicker();
            picker.SuggestedStartLocation = Windows.Storage.Pickers.PickerLocationId.PicturesLibrary;
            picker.FileTypeFilter.Add("*");
            using (Stream fileStr = await (await picker.PickSingleFileAsync()).OpenStreamForReadAsync())
            {
                byte[] bytes = new byte[fileStr.Length];
                const int BUFFER_SIZE = 1024;
                byte[] buffer = new byte[BUFFER_SIZE];
                int position = 0;
                int bytesread = 0;
                while ((bytesread = await fileStr.ReadAsync(buffer, 0, BUFFER_SIZE)) > 0)
                    for (int i = 0; i < bytesread; i++, position++)
                        bytes[position] = buffer[i];
                foreach (var el in bytes)
                {
                    TextBoxCompare.Text += el.ToString();
                }
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

    }
}
