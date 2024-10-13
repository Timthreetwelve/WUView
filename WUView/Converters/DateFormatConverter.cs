﻿// Copyright (c) Tim Kennedy. All Rights Reserved. Licensed under the MIT License.

namespace WUView.Converters;

/// <summary>
/// Sets the date format.
/// </summary>
/// <seealso cref="System.Windows.Data.IValueConverter" />
internal sealed class DateFormatConverter : IValueConverter
{
    public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
    {
        if (value is DateTime dateTime)
        {
            DateTime item = dateTime;

            CultureInfo cult = CultureInfo.CurrentCulture;
            switch (UserSettings.Setting!.DateFormat)
            {
                case 1:
                    return item.ToString("yyyy/MM/dd  HH:mm");
                case 2:
                    return item.ToString("MM/dd/yyyy  hh:mm tt");
                case 3:
                    return item.ToString("dd-MMM-yyyy  HH:mm");
                case 4:
                    return item.ToUniversalTime().ToString("yyyy-MM-dd  HH:mm  UTC");
                case 5:
                    return item.ToString("dd/MM/yyyy");
                case 6:
                    return item.ToString("dd/MM/yyyy  HH:mm");
                case 7:
                    return item.ToString("yyyy/MM/dd");
                case 8:
                    string cultDateOnly = cult.DateTimeFormat.ShortDatePattern;
                    return item.ToString(cultDateOnly);
                case 9:
                    string cultDate = cult.DateTimeFormat.ShortDatePattern;
                    string cultTime = cult.DateTimeFormat.ShortTimePattern;
                    return item.ToString($"{cultDate}  {cultTime}");

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
