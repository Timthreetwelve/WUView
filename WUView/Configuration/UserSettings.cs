// Copyright (c) Tim Kennedy. All Rights Reserved. Licensed under the MIT License.

namespace WUView.Configuration;

[INotifyPropertyChanged]
public partial class UserSettings : ConfigManager<UserSettings>
{
    #region Properties
    [ObservableProperty]
    private static bool _boldToday = true;

    [ObservableProperty]
    private int _dateFormat;

    [ObservableProperty]
    private double _detailsHeight = 300;

    [ObservableProperty]
    private bool _hideExcluded = true;

    [ObservableProperty]
    private bool _includeDebug = true;

    [ObservableProperty]
    private bool _keepOnTop;

    [ObservableProperty]
    private MaxUpdates _maxUpdates = MaxUpdates.All;

    [ObservableProperty]
    private AccentColor _primaryColor = AccentColor.Blue;

    [ObservableProperty]
    private Spacing _rowSpacing = Spacing.Comfortable;

    [ObservableProperty]
    private bool _showDetails = true;

    [ObservableProperty]
    private bool _startCentered = true;

    [ObservableProperty]
    private MySize _uISize = MySize.Default;

    [ObservableProperty]
    private ThemeType _uITheme = ThemeType.System;

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
