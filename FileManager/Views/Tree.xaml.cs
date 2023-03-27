using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.Storage.Pickers;
using Windows.Storage;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;

// The Blank Page item template is documented at https://go.microsoft.com/fwlink/?LinkId=234238

namespace FileManager.Views
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class Tree : Page
    {
        public Tree()
        {
            this.InitializeComponent();
        }

        private void TextBoxTree_TextChanged(object sender, TextChangedEventArgs e)
        {
            UpdateGenerateButtonState();
        }

        private async void TreeSelectFolder_Click(object sender, RoutedEventArgs e)
        {
            FolderPicker folderPicker = new FolderPicker();
            folderPicker.SuggestedStartLocation = PickerLocationId.Desktop;

            // Restrict the picker to only allow folders
            folderPicker.FileTypeFilter.Add(".folder");
            folderPicker.FileTypeFilter.Add(".directory");
            folderPicker.FileTypeFilter.Add(".library-ms");

            // Show the folder picker dialog
            StorageFolder folder = await folderPicker.PickSingleFolderAsync();

            if (folder != null)
            {
                // The user selected a folder, update the UI accordingly
                TreeTextBox.Text = folder.Path;
            }
        }

        private void TreeButton_Click(object sender, RoutedEventArgs e)
        {
            ChangeTextLoading();
            TreeButton.IsEnabled = false;
        }

        private void UpdateGenerateButtonState()
        {
            if (!String.IsNullOrEmpty(TreeTextBox.Text))
            {
                TreeButton.IsEnabled = true;
            }
            else
            {
                TreeButton.IsEnabled = false;
            }
        }

        private void ChangeTextLoading()
        {
            var textBlock = FindName("TreeGenerateTextblock") as TextBlock;
            textBlock.Text = "Please wait, scanning in progress...";
            textBlock.Text += Environment.NewLine;
            textBlock.Text += "Note that changing to a different tab will cancel the search, but you can minimise the application";
        }

        private void ChangeText(List<StorageFile> message)
        {
            var textBlock = FindName("TreeGenerateTextblock") as TextBlock;
            textBlock.Text = "finished";

        }
    }
}
