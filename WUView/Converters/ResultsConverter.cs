// Copyright (c) Tim Kennedy. All Rights Reserved. Licensed under the MIT License.

namespace WUView.Converters;

/// <summary>
/// Updates strings to the desired format.
/// </summary>
/// <seealso cref="System.Windows.Data.IValueConverter" />
internal sealed class ResultsConverter : IValueConverter
{
    public object? Convert(object? value, Type targetType, object? parameter, CultureInfo culture)
    {
        if (value is null || parameter is null)
        {
            return string.Empty;
        }

        return parameter is string paramString && paramString == "HResult" && value is string hrString
            ? $"0x{int.Parse(hrString, CultureInfo.InvariantCulture):X8}"
            : value.ToString();
    }

    public object ConvertBack(object? value, Type targetType, object? parameter, CultureInfo culture)
    {
        return Binding.DoNothing;
    }
}
