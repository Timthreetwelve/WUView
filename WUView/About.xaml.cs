// Copyright (c) Tim Kennedy. All Rights Reserved. Licensed under the MIT License.

#region using directives
using System.Windows;
using System.Windows.Input;
using System.Diagnostics;
using System.Reflection;
using System.Windows.Navigation;
#endregion

namespace WUView
{
    public partial class About : Window
    {
        public About()
        {
            InitializeComponent();
        }

        #region Mouse events
        private void Window_MouseDown(object sender, MouseButtonEventArgs e)
        {
            Close();
        }
        #endregion

        #region Window events
        private void Window_Activated(object sender, System.EventArgs e)
        {
            FileVersionInfo versionInfo = FileVersionInfo.GetVersionInfo(Assembly.GetEntryAssembly().Location);

            string version = versionInfo.FileVersion;
            string copyright = versionInfo.LegalCopyright;
            string product = versionInfo.ProductName;

            tbVersion.Text = version.Remove(version.LastIndexOf("."));
            tbCopyright.Text = copyright.Replace("Copyright ", "");
            Title = $"About {product}";
            Topmost = true;
        }
        #endregion

        #region Link events
        private void ReadMeLink_Click(object sender, RoutedEventArgs e)
        {
            Topmost = false;
            Close();
            _ = Process.Start(".\\ReadMe.txt");
        }

        private void OnNavigate(object sender, RequestNavigateEventArgs e)
        {
            Process.Start(e.Uri.AbsoluteUri);
            e.Handled = true;
        }
        #endregion
    }
}
