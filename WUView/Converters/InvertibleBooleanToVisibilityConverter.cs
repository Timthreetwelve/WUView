// Copyright (c) Tim Kennedy. All Rights Reserved. Licensed under the MIT License.

namespace WUView.Converters;

/// <summary>
/// Converts a boolean value to a Visibility value.
/// Inspired by https://stackoverflow.com/a/2427307/15237757
/// </summary>
internal sealed class InvertibleBooleanToVisibilityConverter : IValueConverter
{
    public object Convert(object? value, Type targetType, object? parameter, CultureInfo culture)
    {
        if (value is not bool val)
        {
            return Binding.DoNothing;
        }
        return string.Equals(parameter?.ToString(), "Inverted", StringComparison.OrdinalIgnoreCase)
            ? !val ? Visibility.Visible : Visibility.Collapsed
            : (val ? Visibility.Visible : Visibility.Collapsed);
    }

    public object ConvertBack(object? value, Type targetType, object? parameter, CultureInfo culture)
    {
        return Binding.DoNothing;
    }
}
