// Copyright (c) Tim Kennedy. All Rights Reserved. Licensed under the MIT License.

namespace WUView.Configuration;

[INotifyPropertyChanged]
public partial class UserSettings : ConfigManager<UserSettings>
{
    #region Properties
    [ObservableProperty]
    private bool _autoSelectFirstRow;

    [ObservableProperty]
    private static bool _boldToday = true;

    [ObservableProperty]
    private int _columnDate = 1;

    [ObservableProperty]
    private static int _columnKB;

    [ObservableProperty]
    private int _columnResult = 3;

    [ObservableProperty]
    private int _columnTitle = 2;

    [ObservableProperty]
    private int _dateFormat = 9;

    [ObservableProperty]
    private double _detailsHeight = 300;

    [ObservableProperty]
    private static double _dialogScale = 1;

    [ObservableProperty]
    private bool _excludeKBandResult;

    [ObservableProperty]
    private bool _hideExcluded = true;

    [ObservableProperty]
    private bool _includeDebug = true;

    [ObservableProperty]
    private bool _keepOnTop;

    [ObservableProperty]
    private bool _languageTesting;

    [ObservableProperty]
    private MaxUpdates _maxUpdates = MaxUpdates.All;

    [ObservableProperty]
    private AccentColor _primaryColor = AccentColor.Blue;

    [ObservableProperty]
    private Spacing _rowSpacing = Spacing.Comfortable;

    /// <summary>
    /// Font used in datagrids.
    /// </summary>
    [ObservableProperty]
    private string? _selectedFont = "Segoe UI";

    [ObservableProperty]
    private bool _showDetails = true;

    [ObservableProperty]
    private bool _showExitInNav = true;

    [ObservableProperty]
    private bool _showLogWarnings = true;

    [ObservableProperty]
    private bool _startCentered = true;

    [ObservableProperty]
    private string _uILanguage = "en-US";

    [ObservableProperty]
    private MySize _uISize = MySize.Default;

    [ObservableProperty]
    private ThemeType _uITheme = ThemeType.System;

    [ObservableProperty]
    private bool _useOSLanguage;

    [ObservableProperty]
    private double _windowHeight = 650;

    [ObservableProperty]
    private double _windowLeft = 100;

    [ObservableProperty]
    private double _windowTop = 100;

    [ObservableProperty]
    private double _windowWidth = 1200;
    #endregion Properties
}
