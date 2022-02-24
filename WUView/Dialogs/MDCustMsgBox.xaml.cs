// Copyright (c) Tim Kennedy. All Rights Reserved. Licensed under the MIT License.

// Inspired by https://stackoverflow.com/a/60302166

namespace WUView.Dialogs;

public partial class MDCustMsgBox : Window
{
    /// <summary>
    /// Custom message box for MDIX
    /// </summary>
    /// <param name="Message">Text of the message</param>
    /// <param name="Title">Text that goes in the title bar</param>
    /// <param name="Buttons">OK, OKCancel or YesNo</param>
    public MDCustMsgBox(string Message, string Title, ButtonType Buttons)
    {
        InitializeComponent();

        txtMessage.Text = Message;

        #region Message box title
        if (string.IsNullOrEmpty(Title))
        {
            txtTitle.Text = Application.Current.MainWindow.Title;
        }
        else
        {
            txtTitle.Text = Title;
        }
        #endregion Message box title

        #region Button visibility
        switch (Buttons)
        {
            case ButtonType.Ok:
                btnOk.Visibility = Visibility.Visible;
                btnCancel.Visibility = Visibility.Collapsed;
                btnYes.Visibility = Visibility.Collapsed;
                btnNo.Visibility = Visibility.Collapsed;
                break;

            case ButtonType.OkCancel:
                btnYes.Visibility = Visibility.Collapsed;
                btnNo.Visibility = Visibility.Collapsed;
                break;

            case ButtonType.YesNo:
                btnOk.Visibility = Visibility.Collapsed;
                btnCancel.Visibility = Visibility.Collapsed;
                break;
        }
        #endregion Button visibility

        #region Window position
        if (Application.Current.MainWindow.IsVisible)
        {
            Owner = GetWindow(Application.Current.MainWindow);
            WindowStartupLocation = WindowStartupLocation.CenterOwner;
        }
        else
        {
            WindowStartupLocation = WindowStartupLocation.CenterScreen;
        }
        #endregion Window position
    }

    #region Button and mouse events
    private void Btn_Click_True(object sender, RoutedEventArgs e)
    {
        DialogResult = true;
        Close();
    }

    private void Btn_Click_False(object sender, RoutedEventArgs e)
    {
        DialogResult = false;
        Close();
    }

    private void TitleBar_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
    {
        DragMove();
    }
    #endregion Button and mouse events
}

#region Button type enumeration
public enum ButtonType
{
    OkCancel,
    YesNo,
    Ok,
}
#endregion
