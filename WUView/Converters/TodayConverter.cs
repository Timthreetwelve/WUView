// Copyright (c) Tim Kennedy. All Rights Reserved. Licensed under the MIT License.

namespace WUView.Converters;

/// <summary>
/// Converter that determines if the update occurred today.
/// </summary>
/// <remarks>Returns <see langword="true"/> if date is today.</remarks>
internal class TodayConverter : IValueConverter
{
    public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
    {
        if (UserSettings.Setting.BoldToday)
        {
            return value is not null and DateTime date && date.Date == DateTime.Today;
        }
        return false;
    }

    public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
    {
        return Binding.DoNothing;
    }
}
