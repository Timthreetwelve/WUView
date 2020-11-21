// Copyright(c) Tim Kennedy. All Rights Reserved. Licensed under the MIT License.

using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;

namespace WUView
{
    public partial class FontSelector : Window
    {
        public FontSelector()
        {
            InitializeComponent();
            LoadListbox();
        }

        private void LoadListbox()
        {
            System.Collections.Generic.ICollection<FontFamily> fontlist = Fonts.SystemFontFamilies;
            lb1.ItemsSource = fontlist.OrderBy(x => x.Source);
            lb1.SelectedValuePath = "Source";
            lb1.SelectedValue = Properties.Settings.Default.FontFamily;
            lb1.ScrollIntoView(lb1.SelectedItem);
            _ = lb1.Focus();
        }

        private void ListBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (IsLoaded)
            {
                Properties.Settings.Default.FontFamily = lb1.SelectedValue.ToString();
            }
        }
    }
}
