using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using System.Security.Cryptography;
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
        private StorageFile myVariable;
        private StorageFile file2;

        public Comparator()
        {
            this.InitializeComponent();
        }


        private void Button_Click(object sender, RoutedEventArgs e)
        {
            string path1;
            string path2;
            pickAFile();
        }

        private async void pickAFile()
        {
            var picker = new Windows.Storage.Pickers.FileOpenPicker();
            picker.SuggestedStartLocation = Windows.Storage.Pickers.PickerLocationId.PicturesLibrary;
            picker.FileTypeFilter.Add("*");
            this.myVariable = await picker.PickSingleFileAsync();
            
            FileStream template = File.OpenRead(this.myVariable.Path);
            
            //byte[] bit = File.ReadAllBytes(myVariable.Path);
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

        private bool FileEquals_by_bytes_singleThreaded(string path1, string path2)
        {
            byte[] file1 = File.ReadAllBytes(path1);
            byte[] file2 = File.ReadAllBytes(path2);
            if (file1.Length == file2.Length) 
            {
                for (int i = 0; i < file1.Length; i++) 
                {
                    if (file1[i] != file2[i])
                    {
                        return false;
                    }
                }
                return true;
            }
            return false;
        }

        private async void Button_Click_1(object sender, RoutedEventArgs e)
        {
            var picker = new Windows.Storage.Pickers.FileOpenPicker();
            picker.SuggestedStartLocation = Windows.Storage.Pickers.PickerLocationId.PicturesLibrary;
            picker.FileTypeFilter.Add("*");
            this.file2 = await picker.PickSingleFileAsync();
        }

        private void Button_Click_2(object sender, RoutedEventArgs e)
        {
            bool yes = this.FileEquals_by_bytes_singleThreaded(myVariable.Path, file2.Path);
            TextBoxCompare.Text = yes.ToString();
        }
    }
}
