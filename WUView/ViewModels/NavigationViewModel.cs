// Copyright (c) Tim Kennedy. All Rights Reserved. Licensed under the MIT License.

namespace WUView.ViewModels;

public partial class NavigationViewModel : ObservableObject
{
    #region MainWindow Instance
    private static readonly MainWindow? _mainWindow = Application.Current.MainWindow as MainWindow;
    #endregion MainWindow Instance

    #region Constructor
    public NavigationViewModel()
    {
        if (CurrentViewModel == null)
        {
            NavigateToPage(NavPage.Viewer);
        }
    }
    #endregion Constructor

    #region Properties
    [ObservableProperty]
    private object? _currentViewModel;

    [ObservableProperty]
    private string? _pageTitle;
    #endregion Properties

    #region List of navigation items
    public static List<NavigationItem> NavigationViewModelTypes { get; set; } =
    [
        new ()
        {
            Name = GetStringResource("NavItem_Updates"),
            NavPage = NavPage.Viewer,
            ViewModelType = typeof(MainViewModel),
            IconKind = PackIconKind.ViewList,
            PageTitle = GetStringResource("NavTitle_Updates")
        },
            new ()
        {
            Name = GetStringResource("NavItem_Settings"),
            NavPage = NavPage.Settings,
            ViewModelType = typeof(SettingsViewModel),
            IconKind = PackIconKind.SettingsOutline,
            PageTitle = GetStringResource("NavTitle_Settings")
        },
        new ()
        {
            Name = GetStringResource("NavItem_About"),
            NavPage = NavPage.About,
            ViewModelType = typeof(AboutViewModel),
            IconKind = PackIconKind.AboutCircleOutline,
            PageTitle = GetStringResource("NavTitle_About")
        },
        new ()
        {
            Name = GetStringResource("NavItem_Exit"),
            IconKind = PackIconKind.ExitToApp,
            IsExit = true
        }
    ];

    #endregion List of navigation items

    #region Navigation Methods
    private void NavigateToPage(NavPage page)
    {
        Navigate(FindNavPage(page));
    }

    private static NavigationItem FindNavPage(NavPage page)
    {
        return NavigationViewModelTypes.Find(x => x.NavPage == page)!;
    }
    #endregion Navigation Methods

    #region Relay Commands
    // Keep in mind that the community toolkit will add "Command" to the end of the method name.

    #region Navigate Command
    [RelayCommand]
    private void Navigate(object param)
    {
        if (param is NavigationItem item)
        {
            if (item.IsExit)
            {
                Application.Current.Shutdown();
            }
            if (item.ViewModelType is not null)
            {
                PageTitle = item.PageTitle;
                CurrentViewModel = null;
                CurrentViewModel = Activator.CreateInstance((Type)item.ViewModelType);
            }
        }
    }
    #endregion Navigate Command

    #region Edit the exclude file
    [RelayCommand]
    private static void EditExclude()
    {
        if (Keyboard.Modifiers == ModifierKeys.Shift)
        {
            TextFileViewer.ViewTextFile(FileHelpers.GetExcludesFile());
        }
        else
        {
            _ = MainViewModel.EditExcludes();
        }
    }
    #endregion Exit the exclude file

    #region Open the About page
    [RelayCommand]
    private void OpenAbout()
    {
        NavigateToPage(NavPage.About);
    }
    #endregion Open the About page

    #region Open the Settings page
    [RelayCommand]
    private void OpenSettings()
    {
        NavigateToPage(NavPage.Settings);
    }
    #endregion Open the Settings page

    #region View log file
    [RelayCommand]
    private static void ViewLogFile()
    {
        SnackbarMsg.ClearAndQueueMessage(GetStringResource("MsgText_OpeningLogFile"), 2000);
        TextFileViewer.ViewTextFile(GetLogfileName()!);
    }
    #endregion View log file

    #region View readme file
    [RelayCommand]
    private static void ViewReadMeFile()
    {
        SnackbarMsg.ClearAndQueueMessage(GetStringResource("MsgText_OpeningReadMeFile"), 2000);
        TextFileViewer.ViewTextFile(Path.Combine(AppInfo.AppDirectory, "readme.txt"));
    }
    #endregion View readme file

    #region Toggle details
    [RelayCommand]
    private static void ToggleDetails()
    {
        UserSettings.Setting!.ShowDetails = !UserSettings.Setting.ShowDetails;
        MainPage.Instance!.SetDetailsHeight();
    }
    #endregion Toggle details

    #region Toggle excluded
    [RelayCommand]
    private static void ToggleExcluded()
    {
        UserSettings.Setting!.HideExcluded = !UserSettings.Setting.HideExcluded;
        MainPage.Instance!.FilterTheGrid(true);
    }
    #endregion Toggle excluded

    #region Remove column sort
    [RelayCommand]
    private static void RemoveSort()
    {
        MainPage.Instance!.ClearColumnSort();
    }
    #endregion Remove column sort

    #region UI Smaller and Larger
    [RelayCommand]
    private static void UILarger()
    {
        MainWindowHelpers.EverythingLarger();
    }

    [RelayCommand]
    private static void UISmaller()
    {
        MainWindowHelpers.EverythingSmaller();
    }
    #endregion UI Smaller and Larger

    #region Application Shutdown
    [RelayCommand]
    private static void Exit()
    {
        Application.Current.Shutdown();
    }
    #endregion Application Shutdown

    #region Launch Windows Update
    [RelayCommand]
    private static void OpenWindowsUpdate()
    {
        SnackbarMsg.ClearAndQueueMessage(GetStringResource("MsgText_OpeningWindowsUpdate"));
        using Process procWU = new();
        procWU.StartInfo.FileName = "ms-settings:windowsupdate";
        procWU.StartInfo.UseShellExecute = true;
        _ = procWU.Start();
    }
    #endregion Launch Windows Update

    #region Save as JSON file
    [RelayCommand]
    private static async Task SaveJson()
    {
        await FileHelpers.SaveAsJson();
    }
    #endregion Save as JSON file

    #region Save to CSV file
    [RelayCommand]
    private static async Task SaveCSV()
    {
        await FileHelpers.SaveToCSV();
    }
    #endregion Save to CSV file

    #region Save details to text file
    [RelayCommand]
    private static async Task SaveText()
    {
        await FileHelpers.SaveToFile();
    }
    #endregion Save details to text file

    #region Copy to clipboard
    [RelayCommand]
    private static void CopyClipboard()
    {
        MainPage.Instance!.Copy2Clipboard(true);
    }
    #endregion Copy to clipboard

    #region Check for new release
    [RelayCommand]
    private static async Task CheckReleaseAsync()
    {
        await GitHubHelpers.CheckRelease();
    }
    #endregion Check for new release

    #region Open the app folder
    [RelayCommand]
    private static void OpenAppFolder()
    {
        using Process process = new();
        process.StartInfo.UseShellExecute = false;
        process.StartInfo.FileName = "Explorer.exe";
        process.StartInfo.Arguments = AppInfo.AppDirectory;
        _ = process.Start();
    }
    #endregion Open the app folder

    #endregion Relay Commands

    #region Key down events
    /// <summary>
    /// Keyboard events
    /// </summary>
    [RelayCommand]
    private void KeyDown(KeyEventArgs e)
    {
        #region Keys without modifiers
        if (e.KeyboardDevice.Modifiers == ModifierKeys.None)
        {
            switch (e.Key)
            {
                case Key.F1:
                    _mainWindow!.NavigationListBox.SelectedValue = FindNavPage(NavPage.About);
                    break;
                case Key.F5:
                    MainPage.RefreshAll();
                    break;
                case Key.Escape:
                    {
                        if (CurrentViewModel is MainViewModel)
                        {
                            MainPage.Instance!.TbxSearch.Clear();
                        }
                        e.Handled = true;
                        break;
                    }
            }
        }
        #endregion Keys without modifiers

        #region Keys with Ctrl
        if (e.KeyboardDevice.Modifiers == ModifierKeys.Control)
        {
            switch (e.Key)
            {
                case Key.OemComma:
                    _mainWindow!.NavigationListBox.SelectedValue = FindNavPage(NavPage.Settings);
                    break;
                case Key.U:
                    _mainWindow!.NavigationListBox.SelectedValue = FindNavPage(NavPage.Viewer);
                    break;
                case Key.L:
                    _ = MainViewModel.EditExcludes();
                    break;
                case Key.D:
                    UserSettings.Setting!.ShowDetails = !UserSettings.Setting.ShowDetails;
                    MainPage.Instance!.SetDetailsHeight();
                    break;
                case Key.E:
                    ToggleExcluded();
                    break;
                case Key.F:
                    MainPage.Instance!.TbxSearch.Focus();
                    break;
                case Key.R:
                    MainPage.Instance!.ClearColumnSort();
                    break;
                case Key.T:
                    {
                        if (UserSettings.Setting!.DateFormat >= 9)
                        {
                            UserSettings.Setting.DateFormat = 0;
                        }
                        else
                        {
                            UserSettings.Setting.DateFormat++;
                        }
                        MainPage.Instance!.UpdateGrid();
                        SnackbarMsg.ClearAndQueueMessage(GetStringResource("MsgText_DateFormatChange"), 2000);
                        break;
                    }
                case Key.Add:
                case Key.OemPlus:
                    {
                        MainWindowHelpers.EverythingLarger();
                        string size = EnumDescConverter.GetEnumDescription(UserSettings.Setting!.UISize);
                        string message = string.Format(CultureInfo.InvariantCulture, MsgTextUISizeSet, size);
                        SnackbarMsg.ClearAndQueueMessage(message, 2000);
                        break;
                    }
                case Key.Subtract:
                case Key.OemMinus:
                    {
                        MainWindowHelpers.EverythingSmaller();
                        string size = EnumDescConverter.GetEnumDescription(UserSettings.Setting!.UISize);
                        string message = string.Format(CultureInfo.InvariantCulture, MsgTextUISizeSet, size);
                        SnackbarMsg.ClearAndQueueMessage(message, 2000);
                        break;
                    }
            }
        }
        #endregion Keys with Ctrl

        #region Keys with Ctrl and Shift
        if (e.KeyboardDevice.Modifiers == (ModifierKeys.Control | ModifierKeys.Shift))
        {
            switch (e.Key)
            {
                case Key.T:
                    {
                        switch (UserSettings.Setting!.UITheme)
                        {
                            case ThemeType.Light:
                                UserSettings.Setting.UITheme = ThemeType.Dark;
                                break;
                            case ThemeType.Dark:
                                UserSettings.Setting.UITheme = ThemeType.Darker;
                                break;
                            case ThemeType.Darker:
                                UserSettings.Setting.UITheme = ThemeType.System;
                                break;
                            case ThemeType.System:
                                UserSettings.Setting.UITheme = ThemeType.Light;
                                break;
                        }
                        string theme = EnumDescConverter.GetEnumDescription(UserSettings.Setting.UITheme);
                        string message = string.Format(CultureInfo.InvariantCulture, MsgTextUIThemeSet, theme);
                        SnackbarMsg.ClearAndQueueMessage(message, 2000);
                        break;
                    }
                case Key.C:
                    {
                        if (UserSettings.Setting!.PrimaryColor >= AccentColor.White)
                        {
                            UserSettings.Setting.PrimaryColor = AccentColor.Red;
                        }
                        else
                        {
                            UserSettings.Setting.PrimaryColor++;
                        }
                        string color = EnumDescConverter.GetEnumDescription(UserSettings.Setting.PrimaryColor);
                        string message = string.Format(CultureInfo.InvariantCulture, MsgTextUIColorSet, color);
                        SnackbarMsg.ClearAndQueueMessage(message, 2000);
                        break;
                    }
                case Key.F:
                    {
                        using Process p = new();
                        p.StartInfo.FileName = AppInfo.AppDirectory;
                        p.StartInfo.UseShellExecute = true;
                        p.StartInfo.ErrorDialog = false;
                        _ = p.Start();
                        break;
                    }
                case Key.K:
                    CompareLanguageDictionaries();
                    ViewLogFile();
                    break;
                case Key.R when UserSettings.Setting?.RowSpacing >= Spacing.Wide:
                    UserSettings.Setting.RowSpacing = Spacing.Compact;
                    break;
                case Key.R:
                    UserSettings.Setting!.RowSpacing++;
                    break;
                case Key.S:
                    TextFileViewer.ViewTextFile(ConfigHelpers.SettingsFileName);
                    break;
            }
        }
        #endregion Keys with Ctrl and Shift
    }
    #endregion Key down events
}
