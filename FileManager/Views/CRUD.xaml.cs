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
using System.Runtime.CompilerServices;


namespace FileManager.Views
{
    public sealed partial class CRUD : Page
    {
        private StorageFolder folderPick;
        private HashSet<StorageFile> filesPick;

        public CRUD()
        {
            this.InitializeComponent();
            filesPick = new HashSet<StorageFile>();
        }

        private async void ChooseFile(object sender, RoutedEventArgs e)
        {
            var picker = new Windows.Storage.Pickers.FileOpenPicker();
            picker.ViewMode = Windows.Storage.Pickers.PickerViewMode.Thumbnail;
            picker.SuggestedStartLocation = Windows.Storage.Pickers.PickerLocationId.PicturesLibrary;
            picker.FileTypeFilter.Add("*");
            StorageFile fileUpload = await picker.PickSingleFileAsync();
            this.filesPick.Add(fileUpload);
            MessageBox.Text = "File successfully chosen";
        }
        private async void ChooseFolder(object sender, RoutedEventArgs e)
        {
            var picker = new Windows.Storage.Pickers.FolderPicker();
            picker.ViewMode = Windows.Storage.Pickers.PickerViewMode.Thumbnail;
            picker.SuggestedStartLocation = Windows.Storage.Pickers.PickerLocationId.PicturesLibrary;
            picker.FileTypeFilter.Add("*");
            StorageFolder folder = await picker.PickSingleFolderAsync();
            this.folderPick = folder;
            MessageBox.Text = "Folder successfully chosen";
        }

        private async void CopyButton_Click(object sender, RoutedEventArgs e)
        {
            if (this.filesPick.Count > 0)
            {
                if (this.folderPick != null)
                {
                    List<Task> tasks = new List<Task>();
                    foreach (StorageFile file in filesPick)
                    {
                        tasks.Add(CopyFileAsync(file, folderPick));
                    }
                    await Task.WhenAll(tasks);
                    this.filesPick.Clear();
                    MessageBox.Text = "File(s) successfully moved";
                }
                else
                {
                    MessageBox.Text = "You have to select a folder to copy the files to.";
                }
            }
            else
            {
                MessageBox.Text = "You have to select at least one file to copy.";
            }
        }

        private async void DeleteButton_Click(object sender, RoutedEventArgs e)
        {
            if (this.filesPick.Count > 0)
            {
                List<Task> tasks = new List<Task>();
                foreach (StorageFile file in filesPick)
                {
                    tasks.Add(DeleteFilesAsync(file));
                }
                await Task.WhenAll(tasks);
                this.filesPick.Clear();
                MessageBox.Text = "File(s) successfully deleted";
            }
            else
            {
                MessageBox.Text = "You have to select at least one file to delete.";
            }
        }

        private async void RenameButton_Click(object sender, RoutedEventArgs e)
        {
            if (this.filesPick.Count > 0)
            {
                List<Task> tasks = new List<Task>();
                foreach (var file in filesPick)
                {
                    tasks.Add(RenameFilesAsync(file));
                }
                await Task.WhenAll(tasks);
                this.filesPick.Clear();
                MessageBox.Text = "File(s) successfully renamed";
            }
            else
            {
                MessageBox.Text = "You have to select at least one file to rename.";
            }
        }

        private async void MoveButton_Click(object sender, RoutedEventArgs e)
        {
            if (this.filesPick.Count > 0)
            {
                if (this.folderPick != null)
                {
                    List<Task> tasks = new List<Task>();
                    foreach (StorageFile file in filesPick)
                    {
                        tasks.Add(MoveFileAsync(file, folderPick));
                    }
                    await Task.WhenAll(tasks);
                    this.filesPick.Clear();
                    MessageBox.Text = "File(s) successfully moved";
                }
                else
                {
                    MessageBox.Text = "You have to select a folder to move the files to.";
                }
            }
            else
            {
                MessageBox.Text = "You have to select at least one file to move.";
            }
        }

        public async Task CopyFileAsync(StorageFile sourceFile, StorageFolder destinationFolder)
        {
            string newFileName = sourceFile.Name;
            StorageFile newFile = await destinationFolder.CreateFileAsync(newFileName, CreationCollisionOption.GenerateUniqueName);
            await sourceFile.CopyAndReplaceAsync(newFile);
        }

        public async Task MoveFileAsync(StorageFile sourceFile, StorageFolder destinationFolder)
        {
            await sourceFile.MoveAsync(destinationFolder);
        }

        private async Task DeleteFilesAsync(StorageFile fileToDelete)
        {
            await fileToDelete.DeleteAsync();
        }

        private async Task RenameFilesAsync(StorageFile fileToRename)
        {
            string fileExtension = Path.GetExtension(fileToRename.Name);
            string newName = string.Concat(RenameTextbox.Text, fileExtension);
            await fileToRename.RenameAsync(newName);
        }

        private void RenameTextbox_TextChanged(object sender, TextChangedEventArgs e)
        {
            if (!String.IsNullOrEmpty(RenameTextbox.Text) && !String.IsNullOrEmpty(RenameTextbox.Text))
            {
                RenameButton.IsEnabled = true;
            }
            else
            {
                RenameButton.IsEnabled = false;
            }
        }
    }
}
