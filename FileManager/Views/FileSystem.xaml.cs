using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using System.Threading;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.Storage.Search;
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
    public sealed partial class FileSystem : Page
    {
        public FileSystem()
        {
            this.InitializeComponent();
        }

        private void SearchNameTextbox_TextChanged(object sender, TextChangedEventArgs e)
        {
            if (SearchNameTextbox.Text == "")
            {
                SearchButton.IsEnabled = false;
            }
            else
            {
                SearchButton.IsEnabled = true;
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
            if (SearchPathTextbox.Text == null || SearchPathTextbox.Text == "")
            {
                SearchPathTextbox.Text = "C:\\";
            }
            //SearchNameTextbox.Text = "'" + SearchNameTextbox.Text + "'";
            //Search(SearchNameTextbox.Text, SearchPathTextbox.Text);
            StartSearch("Docker", "C:\\Users\\rober\\Desktop\\Work");
            //StartSearch(SearchNameTextbox.Text, SearchPathTextbox.Text);
        }

        private static List<StorageFile> SearchFilesInFolder(StorageFolder folder, string searchTerm)
        {
            var result = new List<StorageFile>();
            //ask to stop
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
            textBlock.Text = "Found " + message.Count.ToString() + " file(s)";
            textBlock.Text += Environment.NewLine;
            foreach (var file in message)
            {
                textBlock.Text += file.Path;
                textBlock.Text += Environment.NewLine;
            }
        }

        private void SeachFoundTextblock_SelectionChanged(object sender, RoutedEventArgs e)
        {

        }
    }
}
