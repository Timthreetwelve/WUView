// Copyright (c) Tim Kennedy. All Rights Reserved. Licensed under the MIT License.

namespace WUView.Converters;

/// <summary>
/// Updates strings to the desired format.
/// </summary>
/// <seealso cref="System.Windows.Data.IValueConverter" />
class ResultsConverter : IValueConverter
{
    public object? Convert(object value, Type targetType, object parameter, CultureInfo culture)
    {
        if (value == null || parameter == null)
        {
            return string.Empty;
        }
        else if (parameter is string paramString && paramString == "HResult")
        {
            if (value is string hrString)
            {
                return string.Format($"0x{int.Parse(hrString):X8}");
            }
            return null; // Handle the case where value is not a string
        }
        return value.ToString();
    }

    public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
    {
        return Binding.DoNothing;
    }
}
