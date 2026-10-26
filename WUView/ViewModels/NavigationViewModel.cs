// Copyright (c) Tim Kennedy. All Rights Reserved. Licensed under the MIT License.

namespace WUView.ViewModels;

internal sealed partial class NavigationViewModel : ObservableObject
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
#pragma warning disable MVVMTK0042 
    [ObservableProperty]
    private object? _currentViewModel;

    [ObservableProperty]
    private string? _pageTitle;
#pragma warning restore MVVMTK0042
    #endregion Properties

    #region List of navigation items
    public static List<NavigationItem> NavigationViewModelTypes { get; set; } =
    [
        new()
        {
            Name = GetStringResource("NavItem_Updates"),
            NavPage = NavPage.Viewer,
            ViewModelType = typeof(MainViewModel),
            IconKind = PackIconKind.ViewList,
            PageTitle = GetStringResource("NavTitle_Updates")
        },
        new()
        {
            Name = GetStringResource("NavItem_Settings"),
            NavPage = NavPage.Settings,
            ViewModelType = typeof(SettingsViewModel),
            IconKind = PackIconKind.SettingsOutline,
            PageTitle = GetStringResource("NavTitle_Settings")
        },
        new()
        {
            Name = GetStringResource("NavItem_About"),
            NavPage = NavPage.About,
            ViewModelType = typeof(AboutViewModel),
            IconKind = PackIconKind.AboutCircleOutline,
            PageTitle = GetStringResource("NavTitle_About")
        },
        new()
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
    public static void ViewLogFile()
    {
        SnackbarMsg.ClearAndQueueMessage(GetStringResource("MsgText_OpeningLogFile"), 2000);
        TextFileViewer.ViewTextFile(GetLogfileName());
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
        string filePath = string.Empty;
        try
        {
            string fileName = PathHelpers.FindOnPath("Explorer.exe");
            if (fileName?.Length == 0)
            {
                _log.Error("Error trying to open application folder: Explorer.exe not found");
                string msg = $"{GetStringResource("MsgText_Error_FileExplorer")}" +
                             $"\n\n{GetStringResource("MsgText_ErrorSeeLog")}";
                _ = new MDCustMsgBox(msg,
                         GetStringResource("MsgText_ErrorCaption"),
                         ButtonType.Ok,
                         false,
                         true,
                         _mainWindow,
                         true).ShowDialog();
                return;
            }
            using Process process = new();
            process.StartInfo.UseShellExecute = false;
            process.StartInfo.FileName = fileName;
            process.StartInfo.Arguments = AppInfo.AppDirectory;
            _ = process.Start();
        }
        catch (Exception ex)
        {
            _log.Error(ex, $"Error trying to open {PathHelpers.AnonymizePath(filePath)}: {ex.Message}");
            _ = new MDCustMsgBox(GetStringResource("MsgText_Error_FileExplorer"),
                     GetStringResource("MsgText_ErrorCaption"),
                     ButtonType.Ok,
                     false,
                     true,
                     null,
                     true).ShowDialog();
        }
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
        // The case statements are in order by modifier keys (none, alt, control, control+shift), then by key.
        // The underscore (_) is a discard for a value if it is not needed.
        switch ((e.KeyboardDevice.Modifiers, e.Key, e.SystemKey))
        {
            // No modifier keys
            case (ModifierKeys.None, Key.F1, _):
                _mainWindow!.NavigationListBox.SelectedValue = FindNavPage(NavPage.About);
                break;

            case (ModifierKeys.None, Key.F5, _):
                MainPage.RefreshAll();
                break;

            case (ModifierKeys.None, Key.Escape, _):
                if (CurrentViewModel is MainViewModel)
                {
                    MainPage.Instance!.TbxSearch.Clear();
                }
                e.Handled = true;
                break;

            // Alt key
            // No alt key commands defined yet.

            // Control key
            case (ModifierKeys.Control, Key.D, _):
                UserSettings.Setting!.ShowDetails = !UserSettings.Setting.ShowDetails;
                MainPage.Instance!.SetDetailsHeight();
                break;

            case (ModifierKeys.Control, Key.E, _):
                ToggleExcluded();
                break;

            case (ModifierKeys.Control, Key.F, _):
                MainPage.Instance!.TbxSearch.Focus();
                break;

            case (ModifierKeys.Control, Key.L, _):
                _ = MainViewModel.EditExcludes();
                break;

            case (ModifierKeys.Control, Key.R, _):
                MainPage.Instance!.ClearColumnSort();
                break;

            case (ModifierKeys.Control, Key.T, _):
                ChangeDateFormat();
                break;

            case (ModifierKeys.Control, Key.U, _):
                _mainWindow!.NavigationListBox.SelectedValue = FindNavPage(NavPage.Viewer);
                break;

            case (ModifierKeys.Control, Key.Add, _):
            case (ModifierKeys.Control, Key.OemPlus, _):
                MainWindowHelpers.EverythingLarger();
                ShowUIChangeMessage("size");
                break;

            case (ModifierKeys.Control, Key.Subtract, _):
            case (ModifierKeys.Control, Key.OemMinus, _):
                MainWindowHelpers.EverythingSmaller();
                ShowUIChangeMessage("size");
                break;

            case (ModifierKeys.Control, Key.OemComma, _):
                _mainWindow!.NavigationListBox.SelectedValue = FindNavPage(NavPage.Settings);
                break;

            // Control + Shift keys
            case (ModifierKeys.Control | ModifierKeys.Shift, Key.C, _):
                CycleColor();
                break;

            case (ModifierKeys.Control | ModifierKeys.Shift, Key.F, _):
                OpenAppFolder();
                break;

            case (ModifierKeys.Control | ModifierKeys.Shift, Key.K, _):
                CompareLanguageDictionaries();
                ViewLogFile();
                e.Handled = true;
                break;

            case (ModifierKeys.Control | ModifierKeys.Shift, Key.R, _):
                CycleRowSpacing(e);
                break;

            case (ModifierKeys.Control | ModifierKeys.Shift, Key.S, _):
                TextFileViewer.ViewTextFile(ConfigHelpers.SettingsFileName);
                break;

            case (ModifierKeys.Control | ModifierKeys.Shift, Key.T, _):
                CycleTheme();
                break;

            case (ModifierKeys.Control | ModifierKeys.Shift, Key.Add, _):
            case (ModifierKeys.Control | ModifierKeys.Shift, Key.OemPlus, _):
                IncreaseFontSize();
                break;

            case (ModifierKeys.Control | ModifierKeys.Shift, Key.Subtract, _):
            case (ModifierKeys.Control | ModifierKeys.Shift, Key.OemMinus, _):
                DecreaseFontSize();
                break;
        }
    #endregion Key down events
    }

    #region Helper methods for key down events
    private static void ChangeDateFormat()
    {
        switch (UserSettings.Setting!.DateFormat)
        {
            case >= 9:
                UserSettings.Setting.DateFormat = 0;
                break;
            default:
                UserSettings.Setting.DateFormat++;
                break;
        }
        MainPage.Instance!.UpdateGrid();

        ShowUIChangeMessage("date");
    }

    private static void CycleTheme()
    {
        UserSettings.Setting!.UITheme = UserSettings.Setting.UITheme switch
        {
            ThemeType.Light => ThemeType.LightGray,
            ThemeType.LightGray => ThemeType.Dark,
            ThemeType.Dark => ThemeType.Darker,
            ThemeType.Darker => ThemeType.DarkBlue,
            ThemeType.DarkBlue => ThemeType.System,
            _ => ThemeType.Light,
        };
        ShowUIChangeMessage("theme");
    }

    private static void CycleColor()
    {
        if (UserSettings.Setting!.PrimaryColor >= AccentColor.White)
        {
            UserSettings.Setting.PrimaryColor = AccentColor.Red;
        }
        else
        {
            UserSettings.Setting.PrimaryColor++;
        }
        ShowUIChangeMessage("color");
    }

    private static void CycleRowSpacing(KeyEventArgs e)
    {
        if (UserSettings.Setting!.RowSpacing >= Spacing.Wide)
        {
            UserSettings.Setting.RowSpacing = Spacing.Compact;
        }
        else
        {
            UserSettings.Setting.RowSpacing++;
        }
        e.Handled = true;
    }

    private static void DecreaseFontSize()
    {
        if (UserSettings.Setting!.SelectedFontSize > 8)
        {
            UserSettings.Setting.SelectedFontSize--;
        }
    }

    private static void IncreaseFontSize()
    {
        if (UserSettings.Setting!.SelectedFontSize < 24)
        {
            UserSettings.Setting.SelectedFontSize++;
        }
    }
    #endregion Helper methods for key down events

    #region Show snack bar message for UI changes
    private static void ShowUIChangeMessage(string messageType)
    {
        CompositeFormat? composite = null;
        string messageVar = string.Empty;
        string messageTxt = string.Empty;

        switch (messageType)
        {
            case "size":
                composite = MsgTextUISizeSet;
                messageVar = EnumDescConverter.GetEnumDescription(UserSettings.Setting!.UISize);
                break;
            case "theme":
                composite = MsgTextUIThemeSet;
                messageVar = EnumDescConverter.GetEnumDescription(UserSettings.Setting!.UITheme);
                break;
            case "color":
                composite = MsgTextUIColorSet;
                messageVar = EnumDescConverter.GetEnumDescription(UserSettings.Setting!.PrimaryColor);
                break;
            case "date":
                composite = null;
                messageTxt  = GetStringResource("MsgText_DateFormatChange");
                break;
        }

        if (composite is not null)
        {
            string message = string.Format(CultureInfo.InvariantCulture, composite, messageVar);
            SnackbarMsg.ClearAndQueueMessage(message, 2000);
        }
        else if(!string.IsNullOrEmpty(messageTxt))
        {
            SnackbarMsg.ClearAndQueueMessage(messageTxt, 2000);
        }
    }
    #endregion Show snack bar message for UI changes
}
