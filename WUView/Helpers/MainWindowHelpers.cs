// Copyright (c) Tim Kennedy. All Rights Reserved. Licensed under the MIT License.

namespace WUView.Helpers;

/// <summary>
/// Class for methods used by the MainWindow or other classes.
/// </summary>
internal static class MainWindowHelpers
{
    #region MainWindow Instance
    private static readonly MainWindow? _mainWindow = Application.Current.MainWindow as MainWindow;
    #endregion MainWindow Instance

    #region StopWatch
    public static readonly Stopwatch _stopwatch = Stopwatch.StartNew();
    #endregion StopWatch

    #region Set and Save MainWindow position and size
    /// <summary>
    /// Sets the MainWindow position and size.
    /// </summary>
    public static void SetWindowPosition()
    {
        _mainWindow!.Height = UserSettings.Setting!.WindowHeight;
        _mainWindow!.Left = UserSettings.Setting!.WindowLeft;
        _mainWindow!.Top = UserSettings.Setting!.WindowTop;
        _mainWindow!.Width = UserSettings.Setting!.WindowWidth;

        if (UserSettings.Setting!.StartCentered)
        {
            _mainWindow.WindowStartupLocation = WindowStartupLocation.CenterScreen;
        }
    }

    /// <summary>
    /// Saves the MainWindow position and size.
    /// </summary>
    public static void SaveWindowPosition()
    {
        Window mainWindow = Application.Current.MainWindow;
        UserSettings.Setting!.WindowHeight = Math.Floor(mainWindow.Height);
        UserSettings.Setting!.WindowLeft = Math.Floor(mainWindow.Left);
        UserSettings.Setting!.WindowTop = Math.Floor(mainWindow.Top);
        UserSettings.Setting!.WindowWidth = Math.Floor(mainWindow.Width);
    }
    #endregion Set and Save MainWindow position and size

    #region Get property value
    /// <summary>
    /// Gets the value of the property
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    /// <returns>An object containing the value of the property</returns>
    public static object? GetPropertyValue(object sender, PropertyChangedEventArgs e)
    {
        PropertyInfo? prop = sender.GetType().GetProperty(e.PropertyName!);
        return prop?.GetValue(sender, null);
    }
    #endregion Get property value

    #region Window Title
    /// <summary>
    /// Puts the version number in the title bar as well as Administrator if running elevated
    /// </summary>
    public static string WindowTitleVersionAdmin()
    {
        // Set the windows title
        return AppInfo.IsAdmin
            ? $"{AppInfo.AppProduct}  {AppInfo.AppProductVersion} - ({GetStringResource("MsgText_WindowTitleAdministrator")})"
            : $"{AppInfo.AppProduct}  {AppInfo.AppProductVersion}";
    }
    #endregion Window Title

    #region Running as Administrator?
    /// <summary>
    /// Determines if running as administrator (elevated)
    /// </summary>
    /// <returns>True if running elevated</returns>
    public static bool IsAdministrator()
    {
        return new WindowsPrincipal(WindowsIdentity.GetCurrent()).IsInRole(WindowsBuiltInRole.Administrator);
    }
    #endregion Running as Administrator?

    #region Event handlers
    /// <summary>
    /// Event handlers.
    /// </summary>
    internal static void EventHandlers()
    {
        // Unhandled exception handler
        AppDomain.CurrentDomain.UnhandledException += CurrentDomain_UnhandledException;

        // Settings change event
        UserSettings.Setting!.PropertyChanged += SettingChange.UserSettingChanged!;
        TempSettings.Setting!.PropertyChanged += SettingChange.TempSettingChanged!;

        // Window Loaded
        _mainWindow!.Loaded += MainWindow_Loaded;

        // Content rendered
        _mainWindow.ContentRendered += MainWindow_ContentRendered!;

        // Window closing event
        _mainWindow.Closing += MainWindow_Closing!;
    }
    #endregion Event handlers

    #region Window Events
    private static void MainWindow_Loaded(object sender, RoutedEventArgs e)
    {
        WUApiHelpers.LogWUAInfo();
        WUApiHelpers.LogWUEnabled();
    }

    public static void MainWindow_ContentRendered(object sender, EventArgs e)
    {
        MainViewModel.GatherInfo();

        if (UserSettings.Setting!.AutoSelectFirstRow && MainPage.Instance!.dataGrid.Items.Count > 0)
        {
            MainPage.Instance.dataGrid.SelectedIndex = 0;
        }
    }

    public static void MainWindow_Closing(object sender, CancelEventArgs e)
    {
        // Stop the stopwatch and record elapsed time
        _stopwatch.Stop();
        _log.Info($"{AppInfo.AppName} {GetStringResource("MsgText_ApplicationShutdown")}.  " +
            $"{GetStringResource("MsgText_ElapsedTime")}: {_stopwatch.Elapsed:h\\:mm\\:ss\\.ff}");

        // Shut down NLog
        LogManager.Shutdown();

        // Save settings
        SaveWindowPosition();
        ConfigHelpers.SaveSettings();
    }
    #endregion Window Events

    #region Unhandled Exception Handler
    /// <summary>
    /// Handles any exceptions that weren't caught by a try-catch statement.
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="args"></param>
    /// <remarks>
    /// This uses default message box.
    /// </remarks>
    internal static void CurrentDomain_UnhandledException(object sender, UnhandledExceptionEventArgs args)
    {
        _log.Error("Unhandled Exception");
        Exception e = (Exception)args.ExceptionObject;
        _log.Error(e.Message);
        if (e.InnerException != null)
        {
            _log.Error(e.InnerException.ToString());
        }
        _log.Error(e.StackTrace);

        _ = MessageBox.Show($"An error has occurred.\n{e.Message}\n\nSee the log file. ",
            "ERROR",
            MessageBoxButton.OK,
            MessageBoxImage.Error);
    }

    #endregion Unhandled Exception Handler

    #region Log Startup
    /// <summary>
    /// Initializes NLog and writes startup messages to the log.
    /// </summary>
    internal static void LogStartup()
    {
        // Set NLog configuration
        NLogHelpers.NLogConfig(false);

        // Log the version, build date and commit id
        _log.Info($"{AppInfo.AppName} ({AppInfo.AppProduct}) {AppInfo.AppVersion} {GetStringResource("MsgText_ApplicationStarting")}");
        _log.Info($"{AppInfo.AppName} {GetStringResource("About_Copyright")} {AppInfo.AppCopyright}");
        _log.Debug($"{AppInfo.AppName} was started from {AppInfo.AppPath}");
        _log.Debug($"{AppInfo.AppName} Build date: {BuildInfo.BuildDateUtc:f} (UTC)");
        _log.Debug($"{AppInfo.AppName} Commit ID: {BuildInfo.CommitIDString} ");

        // Log the .NET version and OS platform
        _log.Debug($"Operating System version: {AppInfo.OsPlatform}");
        _log.Debug($".Net version: {AppInfo.RuntimeVersion.Replace(".NET", "")}");
    }
    #endregion Log Startup

    #region Show MainWindow
    /// <summary>
    /// Show the main window and set it's state to normal
    /// </summary>
    public static void ShowMainWindow()
    {
        Application.Current.MainWindow.Show();
        Application.Current.MainWindow.Visibility = Visibility.Visible;
        Application.Current.MainWindow.WindowState = WindowState.Normal;
        Application.Current.MainWindow.ShowInTaskbar = true;
        _ = Application.Current.MainWindow.Activate();
    }
    #endregion Show MainWindow
}
