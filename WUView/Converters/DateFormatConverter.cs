// Copyright (c) Tim Kennedy. All Rights Reserved. Licensed under the MIT License.

namespace WUView.Converters;

/// <summary>
/// Sets the date format.
/// </summary>
/// <seealso cref="System.Windows.Data.IValueConverter" />
internal class DateFormatConverter : IValueConverter
{
    public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
    {
        if (value is not null and DateTime)
        {
            DateTime item = (DateTime)value;

            switch (UserSettings.Setting.DateFormat)
            {
                case 1:
                    return item.ToString("yyyy/MM/dd  HH:mm");
                case 2:
                    return item.ToString("MM/dd/yyyy  hh:mm tt");
                case 3:
                    return item.ToString("dd-MMM-yyyy  HH:mm");
                case 4:
                    return item.ToUniversalTime().ToString("yyyy-MM-dd  HH:mm  UTC");
                default:
                    return item.ToString("MM/dd/yyyy");
            }
        }
        return string.Empty;
    }

    public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
    {
        return Binding.DoNothing;
    }
}
