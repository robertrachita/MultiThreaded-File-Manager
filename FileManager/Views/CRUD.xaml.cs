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
            OperationDropDown.Items.Add("Rename");
            OperationDropDown.Items.Add("Delete");
            OperationDropDown.Items.Add("Copy");
            OperationDropDown.Items.Add("Move");

        }

        private void ExecuteButton_Click(object sender, RoutedEventArgs e)
        {
            if (OperationDropDown.SelectedItem == null)
            {
                Trace.WriteLine("null zsamo");
            }
            else if (OperationDropDown.SelectedItem.ToString() == "Rename")
            {
                if (RenameTextbox.Text != "")
                {
                    RenameExecute(sender, e);
                }
                else
                {
                    MessageBox.Text = "You have to give a name to rename the file";
                }
            }
            else if (OperationDropDown.SelectedItem.ToString() == "Delete")
            {
                DeleteExecute(sender, e);
            }
            else if (OperationDropDown.SelectedItem.ToString() == "Copy")
            {
                CopyExecute(sender, e);
            }
            else if (OperationDropDown.SelectedItem.ToString() == "Move")
            {
                MoveExecute(sender, e);
            }
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

        private async void CopyExecute(object sender, RoutedEventArgs e)
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

        private async void DeleteExecute(object sender, RoutedEventArgs e)
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

        private async void RenameExecute(object sender, RoutedEventArgs e)
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

        private async void MoveExecute(object sender, RoutedEventArgs e)
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

        private void ComboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (OperationDropDown.SelectedItem == null)
            {
                filePicker.IsEnabled = false;
                RenameTextbox.IsEnabled = false;
                ExecuteButton.IsEnabled = false;
                folderPicker.IsEnabled = false;
            }
            else if (OperationDropDown.SelectedItem.ToString() == "Rename")
            {
                filePicker.IsEnabled = true;
                RenameTextbox.IsEnabled = true;
                ExecuteButton.IsEnabled = true;
                folderPicker.IsEnabled = false;
            }
            else if (OperationDropDown.SelectedItem.ToString() == "Delete")
            {
                filePicker.IsEnabled = true;
                RenameTextbox.IsEnabled = false;
                ExecuteButton.IsEnabled = true;
                folderPicker.IsEnabled = false;
            }
            else if (OperationDropDown.SelectedItem.ToString() == "Copy")
            {
                filePicker.IsEnabled = true;
                RenameTextbox.IsEnabled = false;
                ExecuteButton.IsEnabled = true;
                folderPicker.IsEnabled = true;
            }
            else if (OperationDropDown.SelectedItem.ToString() == "Move")
            {
                filePicker.IsEnabled = true;
                RenameTextbox.IsEnabled = false;
                ExecuteButton.IsEnabled = true;
                folderPicker.IsEnabled = true;
            }
        }

        private void RenameTextbox_TextChanged(object sender, TextChangedEventArgs e)
        {

        }
    }
}
