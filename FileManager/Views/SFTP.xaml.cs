using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using System.Threading.Tasks;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.Storage;
using Windows.Storage.Pickers;
using Windows.UI.Popups;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Media.Imaging;
using Windows.UI.Xaml.Navigation;

// The Blank Page item template is documented at https://go.microsoft.com/fwlink/?LinkId=234238

namespace FileManager.Views
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class SFTP : Page
    {
        private Server server;
        public static SFTP Current;

        public SFTP()
        {
            this.InitializeComponent();
            Current = this;
            server = new Server("192.168.178.207", 2222, "tester", "password");
            
        }

        private async void Uploadbtn_Click(object sender, RoutedEventArgs e)
        {
            FileOpenPicker openPicker = new FileOpenPicker();
            openPicker.ViewMode = PickerViewMode.Thumbnail;
            openPicker.SuggestedStartLocation = PickerLocationId.PicturesLibrary;
            openPicker.FileTypeFilter.Add("*");
            IReadOnlyList<StorageFile> filelist = await openPicker.PickMultipleFilesAsync();
            if( filelist.Count > 0) {
                await Task.Run(() => server.connectClient());
                server.UploadTask("/public/", filelist);
            }    
        }

        private async void Downloadbtn_Click(object sender, RoutedEventArgs e)
        {

            FileOpenPicker openPicker = new FileOpenPicker();
            openPicker.ViewMode = PickerViewMode.Thumbnail;
            openPicker.SuggestedStartLocation = PickerLocationId.PicturesLibrary;
            openPicker.FileTypeFilter.Add("*");
            IReadOnlyList<StorageFile> filelist = await openPicker.PickMultipleFilesAsync();
            await Task.Run(() => server.connectClient());
            server.downloadAsync(filelist);
        }

        private async void Uploabtn_without_task_Click(object sender, RoutedEventArgs e)
        {
            FileOpenPicker openPicker = new FileOpenPicker();
            openPicker.ViewMode = PickerViewMode.Thumbnail;
            openPicker.SuggestedStartLocation = PickerLocationId.PicturesLibrary;
            openPicker.FileTypeFilter.Add("*");
            IReadOnlyList<StorageFile> filelist = await openPicker.PickMultipleFilesAsync();
            if (filelist.Count > 0)
            {
                await Task.Run(() => server.connectClient());
                server.UploadWithoutTask("/public/", filelist);
            }
        }
    }
}