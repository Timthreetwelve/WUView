// Copyright (c) Tim Kennedy. All Rights Reserved. Licensed under the MIT License.

namespace WUView.Converters;

/// <summary>
/// A value converter that subtracts a specified double value from the input value. Used in this application to adjust
/// the MaxWidth of a TextBox by subtracting a fixed value, passed via the ConverterParameter, from the ActualWidth of
/// its container.
/// </summary>
internal sealed class SubtractDoubleConverter : IValueConverter
{
    public object Convert(object? value, Type targetType, object? parameter, CultureInfo culture)
    {
        if (value is double baseWidth && double.TryParse(parameter?.ToString(), out double subtract))
        {
            return Math.Max(0, baseWidth - subtract);
        }

        return Binding.DoNothing;
    }

    public object ConvertBack(object? value, Type targetType, object? parameter, CultureInfo culture)
    {
        return Binding.DoNothing;
    }
}
