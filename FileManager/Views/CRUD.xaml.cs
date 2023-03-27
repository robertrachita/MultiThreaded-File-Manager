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


namespace FileManager.Views
{
    public sealed partial class CRUD : Page
    {
        private StorageFile file;
        private List<StorageFolder> folders;

        public CRUD()
        {
            this.InitializeComponent();
            folders = new List<StorageFolder>();
        }

        private async void CopyButton_Click(object sender, RoutedEventArgs e)
        {
            StorageFile fileToCopy = await StorageFile.GetFileFromPathAsync(file.Path);

            Parallel.ForEach(folders, async folder =>
            {
                await CopyFileAsync(fileToCopy, folder);
            });
        }

        public async Task CopyFileAsync(StorageFile sourceFile, StorageFolder destinationFolder)
        {
            string newFileName = sourceFile.Name;
            StorageFile newFile = await destinationFolder.CreateFileAsync(newFileName, CreationCollisionOption.GenerateUniqueName);
            await sourceFile.CopyAndReplaceAsync(newFile);
        }

        private async void filePick(object sender, RoutedEventArgs e)
        {
            var picker = new Windows.Storage.Pickers.FileOpenPicker();
            picker.ViewMode = Windows.Storage.Pickers.PickerViewMode.Thumbnail;
            picker.SuggestedStartLocation = Windows.Storage.Pickers.PickerLocationId.PicturesLibrary;
            picker.FileTypeFilter.Add("*");
            StorageFile fileUpload = await picker.PickSingleFileAsync();
            this.file = fileUpload;
        }

        private async void pickFolder(object sender, RoutedEventArgs e)
        {
            var picker = new Windows.Storage.Pickers.FolderPicker();
            picker.ViewMode = Windows.Storage.Pickers.PickerViewMode.Thumbnail;
            picker.SuggestedStartLocation = Windows.Storage.Pickers.PickerLocationId.PicturesLibrary;
            picker.FileTypeFilter.Add("*");
            StorageFolder folder = await picker.PickSingleFolderAsync();
            this.folders.Add(folder);
        }
    }
}
