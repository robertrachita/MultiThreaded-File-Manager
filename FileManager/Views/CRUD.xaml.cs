using System;
using System.Collections.Generic;
using System.Diagnostics;
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
    public sealed partial class CRUD : Page
    {
        public CRUD()
        {
            this.InitializeComponent();
        }

        private void Textbox_TextChanged(object sender, TextChangedEventArgs e)
        {
            if (TextBoxCrud.Text == "")
            {
                CopyButton.IsEnabled = false;
            }
            else
            {
                CopyButton.IsEnabled = true;
            }
        }

        private void CopyButton_Click(object sender, RoutedEventArgs e)
        {
            File.Copy(@"c:\Users\levis\OneDrive\Asztali gép\dnd.png", @"c:\Users\levis\OneDrive\asd.png", true);
            Trace.WriteLine("button clicked");
        }
    }
}
