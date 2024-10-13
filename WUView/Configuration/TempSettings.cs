// Copyright (c) Tim Kennedy. All Rights Reserved. Licensed under the MIT License.

namespace WUView.Configuration;

/// <summary>
/// Class for non-persistent settings.
/// </summary>
[INotifyPropertyChanged]
internal sealed partial class TempSettings : ConfigManager<TempSettings>
{
    [ObservableProperty]
    private static bool _appExpanderOpen;

    [ObservableProperty]
    private static bool _uIExpanderOpen;

    [ObservableProperty]
    private static bool _langExpanderOpen;
}
