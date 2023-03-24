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
                TextBoxDefaultView.Text = "If you see this message, the application does not have sufficient permissions to run properly. In order to solve this, the Settings App will be launched in ~20 seconds. Please search for the 'FileManager' app, select 'Advanced Options' from the 3 dots menu  on the right, and tick the 'File system' slider under the 'App permissions' tab. The app will close itself soon, please restart to use it."; 
            }
            else
            {
                TextBoxDefaultView.Text = "hello";
            }
        }
    }
}
