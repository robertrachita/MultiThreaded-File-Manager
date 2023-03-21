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

        public IEnumerable<string> Search(String searchTerm, String searchPath, String searchExtension)
        {
            //var options = new QueryOptions(CommonFileQuery.DefaultQuery, new[] { searchTerm, ".exe"});
            List<String> searchExtensionFilter = new List<String>
            {
                searchExtension
            };
            var options = new QueryOptions(CommonFileQuery.DefaultQuery, searchExtensionFilter);
            var folder = StorageFolder.GetFolderFromPathAsync(searchPath).GetAwaiter().GetResult();
            var query = folder.CreateFileQueryWithOptions(options);

            var files = new List<string>();

            // Execute the query synchronously
            var queryResults = query.GetFilesAsync().GetAwaiter().GetResult();

            var threads = new List<Thread>();

            foreach (var file in queryResults)
            {
                var thread = new Thread(() =>
                {
                    try
                    {
                        // Read the file contents
                        var text = FileIO.ReadTextAsync(file).GetAwaiter().GetResult();

                        // Add the file path to the list if the search term is found
                        if (text.Contains(searchTerm))
                        {
                            lock (files)
                            {
                                files.Add(file.Path);
                            }
                        }
                    }
                    catch (Exception ex)
                    {
                        // Handle any exceptions that occur during file reading
                        Console.WriteLine($"Error reading file {file.Path}: {ex}");
                    }
                });

                threads.Add(thread);
                thread.Start();
            }

            // Wait for all threads to complete
            foreach (var thread in threads)
            {
                thread.Join();
            }

            var textBlock = FindName("SearchFoundTextBlock") as TextBlock;
            textBlock.Text = "Found " + files.Count + " files";
            return files;
        }

        private void SearchButton_Click(object sender, RoutedEventArgs e)
        {
            if (SearchPathTextbox.Text == null || SearchPathTextbox.Text == "")
            {
                SearchPathTextbox.Text = "C:\\";
            }
            //SearchNameTextbox.Text = "'" + SearchNameTextbox.Text + "'";
            //Search(SearchNameTextbox.Text, SearchPathTextbox.Text);
            Search("yes", "C:\\Users\\rober\\Desktop\\Work\\MultiThreaded-File-Manager\\FileManager", ".exe");
        }
    }
}
