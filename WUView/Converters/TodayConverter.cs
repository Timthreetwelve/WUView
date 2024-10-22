// Copyright (c) Tim Kennedy. All Rights Reserved. Licensed under the MIT License.

namespace WUView.Converters;

/// <summary>
/// Converter that determines if the update occurred today.
/// </summary>
/// <remarks>Returns <see langword="true"/> if date is today.</remarks>
internal sealed class TodayConverter : IValueConverter
{
    public object Convert(object? value, Type targetType, object? parameter, CultureInfo culture)
    {
        return UserSettings.Setting!.BoldToday ? value is DateTime date && date.Date == DateTime.Today : (object)false;
    }

    public object ConvertBack(object? value, Type targetType, object? parameter, CultureInfo culture)
    {
        return Binding.DoNothing;
    }
}
