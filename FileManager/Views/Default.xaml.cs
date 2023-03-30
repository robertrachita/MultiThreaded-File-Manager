using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.Foundation;
using Windows.Foundation.Collections;
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
    public sealed partial class Default : Page
    {
        public Default()
        {
            this.InitializeComponent();
            if (App.NotHavePermission)
            {
                TextBoxDefaultView.Text = "If you see this message, the application does not have sufficient permissions to run properly. In order to solve this, the Settings App has been launched. Please search for the 'FileManager' app, select 'Advanced Options' from the 3 dots menu  on the right, and tick the 'File system' slider under the 'App permissions' tab. The app will close itself soon, please restart it after granting the rights."; 
            }
            else
            {
                TextBoxDefaultView.Text = "Greetings,";
                TextBoxDefaultView.Text += Environment.NewLine;
                TextBoxDefaultView.Text += "The following features are available:";
                TextBoxDefaultView.Text += Environment.NewLine;
                TextBoxDefaultView.Text += "\u2022 File Searcher";
                TextBoxDefaultView.Text += Environment.NewLine;
                TextBoxDefaultView.Text += "\u2022File Comparison";
                TextBoxDefaultView.Text += Environment.NewLine;
                TextBoxDefaultView.Text += "\u2022 File Operations";
                TextBoxDefaultView.Text += Environment.NewLine;
                TextBoxDefaultView.Text += "\u2022 Folder Tree View";
                TextBoxDefaultView.Text += Environment.NewLine;
                TextBoxDefaultView.Text += "\u2022 Server Operations,";
                TextBoxDefaultView.Text += Environment.NewLine;
                TextBoxDefaultView.Text += "Please select the feature you'd like to use by navigating the Menu bar on the left side";
                TextBoxDefaultView.Text += Environment.NewLine;
                TextBoxDefaultView.Text += "Note that, due to the nature of UWP, this application cannot access drive root directories, may have troubles accessing other drives beside C:\\ and/or process any operations that requires admin access. Thank you for your understanding, and we hope that you will find our application useful!";
            }
        }
    }
}
