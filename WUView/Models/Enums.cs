// Copyright (c) Tim Kennedy. All Rights Reserved. Licensed under the MIT License.

namespace WUView.Models;

/// <summary>
/// Navigation Page
/// </summary>
public enum NavPage
{
    Viewer = 0,
    Settings = 1,
    About = 2,
    Exit = 3
}

/// <summary>
/// Theme type, Light, Dark, or System
/// </summary>
public enum ThemeType
{
    Light = 0,
    [Description("Material Dark")]
    Dark = 1,
    Darker = 2,
    System = 3
}

/// <summary>
/// Size of the UI, Smallest, Smaller, Default, Larger, or Largest
/// </summary>
public enum MySize
{
    Smallest = 0,
    Smaller = 1,
    Small = 2,
    Default = 3,
    Large = 4,
    Larger = 5,
    Largest = 6
}

/// <summary>
/// One of the 19 predefined Material Design in XAML colors
/// </summary>
public enum AccentColor
{
    Red = 0,
    Pink = 1,
    Purple = 2,
    [Description("Deep Purple")]
    Deep_Purple = 3,
    Indigo = 4,
    Blue = 5,
    [Description("Light Blue")]
    Light_Blue = 6,
    Cyan = 7,
    Teal = 8,
    Green = 9,
    [Description("Light Green")]
    Light_Green = 10,
    Lime = 11,
    Yellow = 12,
    Amber = 13,
    Orange = 14,
    [Description("Deep Orange")]
    Deep_Orange = 15,
    Brown = 16,
    Gray = 17,
    [Description("Blue Gray")]
    Blue_Gray = 18,
    Black = 19,
    White = 20,
}

/// <summary>
/// Space between rows in the DataGrid
/// </summary>
public enum Spacing
{
    Compact = 0,
    Comfortable = 1,
    Wide = 2
}

/// <summary>
/// Maximum number of updates to get
/// </summary>
public enum MaxUpdates
{
    [Description("All Updates")]
    All = 0,
    [Description("50 Most Recent")]
    Max50 = 1,
    [Description("100 Most Recent")]
    Max100 = 2,
    [Description("250 Most Recent")]
    Max250 = 3,
    [Description("500 Most Recent")]
    Max500 = 4
}