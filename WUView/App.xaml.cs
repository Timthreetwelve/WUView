// Copyright (c) Tim Kennedy. All Rights Reserved. Licensed under the MIT License.

namespace WUView;

/// <summary>
/// Interaction logic for App.xaml
/// </summary>
public partial class App : Application
{
    #region Single instance
    /// <summary>
    /// Override the Startup Event.
    /// </summary>
    /// <remarks>
    /// Only allows a single instance of the application to run.
    /// </remarks>
    protected override void OnStartup(StartupEventArgs e)
    {
        SingleInstance.Create(AppInfo.AppName);

        base.OnStartup(e);
    }
    #endregion Single instance
}
