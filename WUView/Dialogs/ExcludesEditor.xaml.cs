// Copyright (c) Tim Kennedy. All Rights Reserved. Licensed under the MIT License.

namespace WUView.Dialogs;

/// <summary>
/// Dialog to edit the exclude list
/// </summary>
public partial class ExcludesEditor : UserControl
{
    public ExcludesEditor()
    {
        InitializeComponent();
    }

    private void UserControl_Loaded(object sender, RoutedEventArgs e)
    {
        TextBox1.CaretIndex = TextBox1.Text.Length;
    }
}
