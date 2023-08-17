// Copyright (c) Tim Kennedy. All Rights Reserved. Licensed under the MIT License.

namespace WUView.Views;
/// <summary>
/// Interaction logic for SettingsPage.xaml
/// </summary>
public partial class SettingsPage : UserControl
{
    public SettingsPage()
    {
        InitializeComponent();
    }

    /// <summary>
    /// Handles the Loaded event of the language ComboBox.
    /// </summary>
    private void CbxLanguage_Loaded(object sender, RoutedEventArgs e)
    {
        cbxLanguage.SelectedIndex = LocalizationHelpers.GetLanguageIndex();
    }
}
