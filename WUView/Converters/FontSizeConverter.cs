// Copyright (c) Tim Kennedy. All Rights Reserved. Licensed under the MIT License.

namespace WUView.Converters;

/// <summary>
/// Converter to increase or decrease font size.
/// Amount of increase or decrease is passed as a parameter.
/// Increase by passing a positive number, decrease by passing a negative number.
/// </summary>
internal sealed class FontSizeConverter : IValueConverter
{
    public object Convert(object? value, Type targetType, object? parameter, CultureInfo culture)
    {
        if (value is not null
            && targetType == typeof(double)
            && parameter is string parm
            && double.TryParse(parm, out double newFontSize))
        {
            return (double)value + newFontSize;
        }
        return value!;
    }

    public object ConvertBack(object? value, Type targetType, object? parameter, CultureInfo culture)
    {
        return Binding.DoNothing;
    }
}
