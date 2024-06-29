// Copyright (c) Tim Kennedy. All Rights Reserved. Licensed under the MIT License.

namespace WUView.Converters;

/// <summary>
/// Converter that changes Spacing to Thickness
/// </summary>
/// <seealso cref="System.Windows.Data.IValueConverter" />
internal class SpacingConverter : IValueConverter
{
    public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
    {
        return UserSettings.Setting!.RowSpacing switch
        {
            Spacing.Compact => new Thickness(15, 2, 15, 1),
            Spacing.Comfortable => new Thickness(15, 4, 15, 3),
            _ => (object)new Thickness(15, 8, 15, 7),
        };
    }

    public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
    {
        return Binding.DoNothing;
    }
}
