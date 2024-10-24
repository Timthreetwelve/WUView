// Copyright (c) Tim Kennedy. All Rights Reserved. Licensed under the MIT License.

namespace WUView;

public partial class MainWindow : Window
{
    public MainWindow()
    {
        SingleInstance.Create(AppInfo.AppName);

        InitializeComponent();

        MainWindowHelpers.WUVStartup();
    }
}
