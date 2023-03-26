using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.Storage;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;
using System.Threading.Tasks;


// The Blank Page item template is documented at https://go.microsoft.com/fwlink/?LinkId=234238

namespace FileManager.Views
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class CRUD : Page
    {
        public CRUD()
        {
            this.InitializeComponent();
        }

        private void Textbox_TextChanged(object sender, TextChangedEventArgs e)
        {
            if (TextBoxCrud.Text == "")
            {
                CopyButton.IsEnabled = false;
            }
            else
            {
                CopyButton.IsEnabled = true;
            }
        }

        private async void CopyButton_Click(object sender, RoutedEventArgs e)
        {
            StorageFile fileToCopy = await StorageFile.GetFileFromPathAsync("C:\\test.txt");
            StorageFolder destinationFolder = await StorageFolder.GetFolderFromPathAsync("C:\\test");
            await CopyFileAsync(fileToCopy, destinationFolder);
        }

        public async Task CopyFileAsync(StorageFile sourceFile, StorageFolder destinationFolder)
        {
            string newFileName = sourceFile.Name;
            StorageFile newFile = await destinationFolder.CreateFileAsync(newFileName, CreationCollisionOption.GenerateUniqueName);
            await sourceFile.CopyAndReplaceAsync(newFile);
        }
    }
}
