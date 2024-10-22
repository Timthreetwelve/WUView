// Copyright (c) Tim Kennedy. All Rights Reserved. Licensed under the MIT License.

namespace WUView.Converters;

/// <summary>
/// Sets the date format.
/// </summary>
/// <seealso cref="System.Windows.Data.IValueConverter" />
internal sealed class DateFormatConverter : IValueConverter
{
    public object Convert(object? value, Type targetType, object? parameter, CultureInfo culture)
    {
        if (value is DateTime dateTime)
        {
            DateTime item = dateTime;

            CultureInfo cult = CultureInfo.CurrentCulture;
            switch (UserSettings.Setting!.DateFormat)
            {
                case 1:
                    return item.ToString("yyyy/MM/dd  HH:mm", CultureInfo.InvariantCulture);
                case 2:
                    return item.ToString("MM/dd/yyyy  hh:mm tt", CultureInfo.InvariantCulture);
                case 3:
                    return item.ToString("dd-MMM-yyyy  HH:mm", CultureInfo.InvariantCulture);
                case 4:
                    return item.ToUniversalTime().ToString("yyyy-MM-dd  HH:mm  UTC", CultureInfo.InvariantCulture);
                case 5:
                    return item.ToString("dd/MM/yyyy", CultureInfo.InvariantCulture);
                case 6:
                    return item.ToString("dd/MM/yyyy  HH:mm", CultureInfo.InvariantCulture);
                case 7:
                    return item.ToString("yyyy/MM/dd", CultureInfo.InvariantCulture);
                case 8:
                    string cultDateOnly = cult.DateTimeFormat.ShortDatePattern;
                    return item.ToString(cultDateOnly, CultureInfo.CurrentCulture);
                case 9:
                    string cultDate = cult.DateTimeFormat.ShortDatePattern;
                    string cultTime = cult.DateTimeFormat.ShortTimePattern;
                    return item.ToString($"{cultDate}  {cultTime}", CultureInfo.CurrentCulture);

                default:
                    return item.ToString("MM/dd/yyyy", CultureInfo.InvariantCulture);
            }
        }
        return string.Empty;
    }

    public object ConvertBack(object? value, Type targetType, object? parameter, CultureInfo culture)
    {
        return Binding.DoNothing;
    }
}
