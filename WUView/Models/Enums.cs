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
[TypeConverter(typeof(EnumDescriptionTypeConverter))]
public enum ThemeType
{
    [LocalizedDescription("SettingsEnum_Theme_Light")]
    Light = 0,
    [LocalizedDescription("SettingsEnum_Theme_Dark")]
    Dark = 1,
    [LocalizedDescription("SettingsEnum_Theme_Darker")]
    Darker = 2,
    [LocalizedDescription("SettingsEnum_Theme_System")]
    System = 3
}

/// <summary>
/// Size of the UI, Smallest, Smaller, Small, Default, Large, Larger, or Largest
/// </summary>
[TypeConverter(typeof(EnumDescriptionTypeConverter))]
public enum MySize
{
    [LocalizedDescription("SettingsEnum_Size_Smallest")]
    Smallest = 0,
    [LocalizedDescription("SettingsEnum_Size_Smaller")]
    Smaller = 1,
    [LocalizedDescription("SettingsEnum_Size_Small")]
    Small = 2,
    [LocalizedDescription("SettingsEnum_Size_Default")]
    Default = 3,
    [LocalizedDescription("SettingsEnum_Size_Large")]
    Large = 4,
    [LocalizedDescription("SettingsEnum_Size_Larger")]
    Larger = 5,
    [LocalizedDescription("SettingsEnum_Size_Largest")]
    Largest = 6
}

/// <summary>
/// One of the 19 predefined Material Design in XAML colors
/// </summary>
[TypeConverter(typeof(EnumDescriptionTypeConverter))]
public enum AccentColor
{
    [LocalizedDescription("SettingsEnum_AccentColor_Red")]
    Red = 0,
    [LocalizedDescription("SettingsEnum_AccentColor_Pink")]
    Pink = 1,
    [LocalizedDescription("SettingsEnum_AccentColor_Purple")]
    Purple = 2,
    [LocalizedDescription("SettingsEnum_AccentColor_DeepPurple")]
    DeepPurple = 3,
    [LocalizedDescription("SettingsEnum_AccentColor_Indigo")]
    Indigo = 4,
    [LocalizedDescription("SettingsEnum_AccentColor_Blue")]
    Blue = 5,
    [LocalizedDescription("SettingsEnum_AccentColor_LightBlue")]
    LightBlue = 6,
    [LocalizedDescription("SettingsEnum_AccentColor_Cyan")]
    Cyan = 7,
    [LocalizedDescription("SettingsEnum_AccentColor_Teal")]
    Teal = 8,
    [LocalizedDescription("SettingsEnum_AccentColor_Green")]
    Green = 9,
    [LocalizedDescription("SettingsEnum_AccentColor_LightGreen")]
    LightGreen = 10,
    [LocalizedDescription("SettingsEnum_AccentColor_Lime")]
    Lime = 11,
    [LocalizedDescription("SettingsEnum_AccentColor_Yellow")]
    Yellow = 12,
    [LocalizedDescription("SettingsEnum_AccentColor_Amber")]
    Amber = 13,
    [LocalizedDescription("SettingsEnum_AccentColor_Orange")]
    Orange = 14,
    [LocalizedDescription("SettingsEnum_AccentColor_DeepOrange")]
    DeepOrange = 15,
    [LocalizedDescription("SettingsEnum_AccentColor_Brown")]
    Brown = 16,
    [LocalizedDescription("SettingsEnum_AccentColor_Gray")]
    Gray = 17,
    [LocalizedDescription("SettingsEnum_AccentColor_BlueGray")]
    BlueGray = 18,
    [LocalizedDescription("SettingsEnum_AccentColor_Black")]
    Black = 19,
    [LocalizedDescription("SettingsEnum_AccentColor_White")]
    White = 20,
}

/// <summary>
/// Space between rows in the DataGrid
/// </summary>
[TypeConverter(typeof(EnumDescriptionTypeConverter))]
public enum Spacing
{
    [LocalizedDescription("SettingsEnum_Spacing_Compact")]
    Compact = 0,
    [LocalizedDescription("SettingsEnum_Spacing_Comfortable")]
    Comfortable = 1,
    [LocalizedDescription("SettingsEnum_Spacing_Wide")]
    Wide = 2
}

/// <summary>
/// Maximum number of updates to get
/// </summary>
[TypeConverter(typeof(EnumDescriptionTypeConverter))]
public enum MaxUpdates
{
    [LocalizedDescription("SettingsEnum_MaxUpdates_All")]
    All = 0,
    [LocalizedDescription("SettingsEnum_MaxUpdates_50")]
    Max50 = 1,
    [LocalizedDescription("SettingsEnum_MaxUpdates_100")]
    Max100 = 2,
    [LocalizedDescription("SettingsEnum_MaxUpdates_250")]
    Max250 = 3,
    [LocalizedDescription("SettingsEnum_MaxUpdates_500")]
    Max500 = 4
}

/// <summary>
/// Date format choices
/// </summary>
/// <see cref="DateFormatConverter"/>
[TypeConverter(typeof(EnumDescriptionTypeConverter))]
public enum DateFormat
{
    [Description("MM/dd/yyyy")]
    MMddyyyy = 0,
    [Description("yyyy/MM/dd HH:mm")]
    yyyyMMddHHmm = 1,
    [Description("MM/dd/yyyy hh:mm tt")]
    MMddyyyhhmmtt = 2,
    [Description("d-MMM-yyyy H:mm")]
    dMMMyyyyHmm = 3,
    [Description("yyyy-MM-dd HH:mm UTC")]
    yyyyMMddHHmmUTC = 4,
    [Description("dd/MM/yyyy")]
    ddMMyyyy = 5,
    [Description("dd/MM/yyyy HH:mm")]
    ddMMyyyyHHmm = 6,
    [Description("yyyy/MM/dd")]
    ddMMyy = 7,
    [LocalizedDescription("SettingsEnum_DateFormat_RegionalDate")]
    RegionalDateOnly = 8,
    [LocalizedDescription("SettingsEnum_DateFormat_RegionalDateTime")]
    RegionalDateTime = 9
}
