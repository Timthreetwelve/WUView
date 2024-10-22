// Copyright (c) Tim Kennedy. All Rights Reserved. Licensed under the MIT License.

namespace WUView.Converters;

/// <summary>
/// Converter that changes Spacing to Thickness
/// </summary>
/// <seealso cref="System.Windows.Data.IValueConverter" />
internal sealed class SpacingConverter : IValueConverter
{
    public object Convert(object? value, Type targetType, object? parameter, CultureInfo culture)
    {
        if (value is Spacing spacing)
        {
            switch (spacing)
            {
                case Spacing.Compact:
                    return new Thickness(15, 2, 15, 2);
                case Spacing.Comfortable:
                    return new Thickness(15, 5, 15, 5);
                case Spacing.Wide:
                    return new Thickness(15, 7, 15, 7);
            }
        }

        return new Thickness(15, 10, 15, 10);
    }

    public object ConvertBack(object? value, Type targetType, object? parameter, CultureInfo culture)
    {
        return Binding.DoNothing;
    }
}
