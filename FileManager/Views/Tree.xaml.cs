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
using System.Text;
using System.Threading.Tasks;
using System.Threading;

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

        private void ChangeText(String message)
        {
            var textBlock = FindName("TreeGenerateTextblock") as TextBlock;
            textBlock.Text = message;

        }

        private async void TreeButton_Click(object sender, RoutedEventArgs e)
        {
            ChangeTextLoading();
            TreeButton.IsEnabled = false;
            string treeView = await GenerateTreeView(TreeTextBox.Text);
            //ChangeText(treeView);
            await WriteEffect(TreeGenerateTextblock, treeView);

        }

        private async Task<string> GenerateTreeView(string folderPath)
        {
            StringBuilder sb = new StringBuilder();
            StorageFolder rootFolder = await StorageFolder.GetFolderFromPathAsync(folderPath);

            // Start a new thread to generate the tree view
            Thread thread = new Thread(() =>
            {
                sb.Append($"{rootFolder.Name}\n");
                GenerateTreeViewHelper(rootFolder, sb);
            });
            thread.Start();

            thread.Join();

            return sb.ToString();
        }

        private void GenerateTreeViewHelper(StorageFolder folder, StringBuilder sb, string prefix = "|-- ")
        {
            // get the files and subfolders in the folder
            IReadOnlyList<StorageFolder> subFolders = null;
            IReadOnlyList<StorageFile> files = null;
            try
            {
                subFolders = folder.GetFoldersAsync().AsTask().Result;
                files = folder.GetFilesAsync().AsTask().Result;
            }
            catch (Exception ex)
            {
                sb.Append($"{prefix}{folder.Name} [Access Denied]\n");
                return;
            }

            // add the files to the tree view
            foreach (var file in files)
            {
                sb.Append($"{prefix}{file.Name}\n");
            }

            // call this method recursively for each subfolder
            foreach (var subFolder in subFolders)
            {
                sb.Append($"{prefix}{subFolder.Name}\n");
                GenerateTreeViewHelper(subFolder, sb, prefix + "|---- ");
            }
        }

        private async Task WriteEffect(TextBlock textBlock, string text)
        {
            textBlock.Text = "";
            var characters = text.ToCharArray();
            var stringBuilder = new StringBuilder();
            var delay = TimeSpan.FromSeconds(0.001);

            foreach (var character in characters)
            {
                stringBuilder.Append(character);
                textBlock.Text = stringBuilder.ToString();
                await Task.Delay(delay);
                TreeScroll.ChangeView(null, TreeScroll.ExtentHeight, null);
            }
        }
    }
}
