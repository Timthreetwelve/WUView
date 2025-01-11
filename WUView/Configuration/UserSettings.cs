// Copyright (c) Tim Kennedy. All Rights Reserved. Licensed under the MIT License.

namespace WUView.Configuration;

[INotifyPropertyChanged]
public partial class UserSettings : ConfigManager<UserSettings>
{
    #region Properties
    /// <summary>
    /// Select the first row of the data grid.
    /// </summary>
    [ObservableProperty]
    private bool _autoSelectFirstRow;

    /// <summary>
    /// Show updates with the current date in bold.
    /// </summary>
    [ObservableProperty]
    private static bool _boldToday = true;

    /// <summary>
    /// Used to persist the column order if the user changes it.
    /// </summary>
    [ObservableProperty]
    private int _columnDate = 1;

    /// <summary>
    /// Used to persist the column order if the user changes it.
    /// </summary>
    [ObservableProperty]
    private static int _columnKB;

    /// <summary>
    /// Used to persist the column order if the user changes it.
    /// </summary>
    [ObservableProperty]
    private int _columnResult = 3;

    /// <summary>
    /// Used to persist the column order if the user changes it.
    /// </summary>
    [ObservableProperty]
    private int _columnTitle = 2;

    /// <summary>
    /// Date format.
    /// </summary>
    [ObservableProperty]
    private int _dateFormat = 9;

    /// <summary>
    /// Height of the details pane.
    /// </summary>
    [ObservableProperty]
    private double _detailsHeight = 300;

    /// <summary>
    ///  Used to determine used to determine scaling of dialogs.
    /// </summary>
    [ObservableProperty]
    private static double _dialogScale = 1;

    /// <summary>
    /// Toggle inclusion of KB number and result when excluding updates.
    /// </summary>
    [ObservableProperty]
    private bool _excludeKBandResult;

    /// <summary>
    /// Toggle hiding excluded updates.
    /// </summary>
    [ObservableProperty]
    private bool _hideExcluded = true;

    /// <summary>
    /// Include debug level messages in the log file.
    /// </summary>
    [ObservableProperty]
    private bool _includeDebug = true;

    /// <summary>
    /// Keep window topmost.
    /// </summary>
    [ObservableProperty]
    private bool _keepOnTop;

    /// <summary>
    /// Enable language testing.
    /// </summary>
    [ObservableProperty]
    private bool _languageTesting;

    /// <summary>
    /// Maximum number of updates to get.
    /// </summary>
    [ObservableProperty]
    private MaxUpdates _maxUpdates = MaxUpdates.All;

    /// <summary>
    /// Accent color.
    /// </summary>
    [ObservableProperty]
    private AccentColor _primaryColor = AccentColor.Blue;

    /// <summary>
    /// Vertical spacing in the data grids.
    /// </summary>
    [ObservableProperty]
    private Spacing _rowSpacing = Spacing.Comfortable;

    /// <summary>
    /// Font used in datagrids.
    /// </summary>
    [ObservableProperty]
    private string? _selectedFont = "Segoe UI";

    /// <summary>
    /// Show the details pane at the bottom.
    /// </summary>
    [ObservableProperty]
    private bool _showDetails = true;

    /// <summary>
    /// Show Exit in the navigation menu.
    /// </summary>
    [ObservableProperty]
    private bool _showExitInNav = true;

    /// <summary>
    /// Show messages in log for updates with non-zero result code.
    /// </summary>
    [ObservableProperty]
    private bool _showLogWarnings = true;

    /// <summary>
    /// Option start with window centered on screen.
    /// </summary>
    [ObservableProperty]
    private bool _startCentered = true;

    /// <summary>
    /// Defined language to use in the UI.
    /// </summary>
    [ObservableProperty]
    private string _uILanguage = "en-US";

    /// <summary>
    /// Amount of UI zoom.
    /// </summary>
    [ObservableProperty]
    private MySize _uISize = MySize.Default;

    /// <summary>
    /// Theme type.
    /// </summary>
    [ObservableProperty]
    private ThemeType _uITheme = ThemeType.System;

    /// <summary>
    /// Use accent color for snack bar message background.
    /// </summary>
    [ObservableProperty]
    private bool _useAccentColorOnSnackbar;

    /// <summary>
    /// Use the operating system language (if one has been provided).
    /// </summary>
    [ObservableProperty]
    private bool _useOSLanguage;

    /// <summary>
    /// Height of the window.
    /// </summary>
    [ObservableProperty]
    private double _windowHeight = 650;

    /// <summary>
    /// Position of left side of the window.
    /// </summary>
    [ObservableProperty]
    private double _windowLeft = 100;

    /// <summary>
    /// Position of the top side of the window.
    /// </summary>
    [ObservableProperty]
    private double _windowTop = 100;

    /// <summary>
    /// Width of the window.
    /// </summary>
    [ObservableProperty]
    private double _windowWidth = 1200;
    #endregion Properties
}
