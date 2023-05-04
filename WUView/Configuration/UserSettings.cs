// Copyright (c) Tim Kennedy. All Rights Reserved. Licensed under the MIT License.

namespace WUView.Configuration;

[INotifyPropertyChanged]
public partial class UserSettings : ConfigManager<UserSettings>
{
    #region Observable Collections
    // place any observable collections here
    #endregion Observable Collections

    #region Properties
    [ObservableProperty]
    private static bool _appExpanderOpen;

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
    private AccentColor _primaryColor = AccentColor.Blue;

    [ObservableProperty]
    private Spacing _rowSpacing = Spacing.Comfortable;

    [ObservableProperty]
    private bool _showDetails = true;

    [ObservableProperty]
    private MySize _uISize = MySize.Default;

    [ObservableProperty]
    private static bool _uIExpanderOpen;

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
