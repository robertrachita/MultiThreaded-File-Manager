using System;
using System.Collections.Generic;
using System.Threading;
using Windows.Storage;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using System.Threading.Tasks;
using Windows.Storage.Pickers;

namespace FileManager.Views
{
    public sealed partial class FileSystem : Page
    {
        public FileSystem()
        {
            this.InitializeComponent();
        }

        private void SearchNameTextbox_TextChanged(object sender, TextChangedEventArgs e)
        {
            UpdateSearchButtonState();
        }
        
        private void UpdateSearchButtonState()
        {
            if(!String.IsNullOrEmpty(SearchNameTextbox.Text) && !String.IsNullOrEmpty(SearchPathTextbox.Text))
            {
                SearchButton.IsEnabled = true;
            }
            else
            {
                SearchButton.IsEnabled = false;
            }
        }

        public async void StartSearch(String searchTerm, string searchPath)
        {
            var searchTask = Task.Run(() => Search(searchTerm, searchPath));
            var result = await searchTask;

            ChangeText(result);
        }

        private List<StorageFile> Search(String searchTerm, String searchPath)
        {
            var searchFolder = StorageFolder.GetFolderFromPathAsync(searchPath).GetAwaiter().GetResult();
            var result = new List<StorageFile>();

            var threadList = new List<Thread>();
            foreach (var folder in searchFolder.GetFoldersAsync().GetAwaiter().GetResult())
            {
                var thread = new Thread(() =>
                {
                    result.AddRange(SearchFilesInFolder(folder, searchTerm));
                });
                threadList.Add(thread);
                thread.Start();
            }


            // Wait for all threads to complete
            foreach (var thread in threadList)
            {
                thread.Join();
            }

            //ChangeText(result);
            return result;
        }

        private void SearchButton_Click(object sender, RoutedEventArgs e)
        {
            ChangeTextLoading();
            SearchButton.IsEnabled = false;
            StartSearch(SearchNameTextbox.Text, SearchPathTextbox.Text);
        }

        private static List<StorageFile> SearchFilesInFolder(StorageFolder folder, string searchTerm)
        {
            var result = new List<StorageFile>();
            foreach (var file in folder.GetFilesAsync().GetAwaiter().GetResult())
            {
                if (file.Name.Contains(searchTerm))
                {
                    result.Add(file);
                }
            }

            foreach (var subFolder in folder.GetFoldersAsync().GetAwaiter().GetResult())
            {
                result.AddRange(SearchFilesInFolder(subFolder, searchTerm));
            }

            return result;
        }

        private void ChangeText(List<StorageFile> message)
        {
            var textBlock = FindName("SeachFoundTextblock") as TextBlock;

            ScrollViewer scrollViewer = FindName("SearchScroll") as ScrollViewer;
            scrollViewer.VerticalScrollBarVisibility = ScrollBarVisibility.Auto;
            
            textBlock.Text = "Found " + message.Count.ToString() + " file(s)";
            textBlock.Text += Environment.NewLine;
            foreach (var file in message)
            {
                textBlock.Text += file.Path;
                textBlock.Text += Environment.NewLine;
            }
        }

        private void ChangeTextLoading()
        {
            var textBlock = FindName("SeachFoundTextblock") as TextBlock;
            textBlock.Text = "Please wait, search in progress...";
            textBlock.Text += Environment.NewLine;
            textBlock.Text += "Note that changing to a different tab will cancel the search, but you can minimise the application";
        }

        private async void SearchSelectFolder_Click(object sender, RoutedEventArgs e)
        {
            FolderPicker folderPicker = new FolderPicker();
            folderPicker.SuggestedStartLocation = PickerLocationId.Desktop; // Set the starting location for the picker
            //folderPicker.FileTypeFilter.Add("*"); // Set the file type filter to allow all file types

            // Restrict the picker to only allow folders
            folderPicker.FileTypeFilter.Add(".folder");
            folderPicker.FileTypeFilter.Add(".directory");
            folderPicker.FileTypeFilter.Add(".library-ms");

            // Show the folder picker dialog
            StorageFolder folder = await folderPicker.PickSingleFolderAsync();

            if (folder != null)
            {
                SearchPathTextbox.Text = folder.Path;
            }
        }

        private void SearchPathTextbox_TextChanged(object sender, TextChangedEventArgs e)
        {
            UpdateSearchButtonState();
        }
    }
}
