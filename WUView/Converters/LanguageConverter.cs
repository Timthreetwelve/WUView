// Copyright (c) Tim Kennedy. All Rights Reserved. Licensed under the MIT License.

namespace WUView.Converters;

internal sealed class LanguageConverter : IValueConverter
{
    /// <summary>
    /// Convert the UILanguage object.
    /// </summary>
    /// <returns>The language name.</returns>
    /// <remarks>Used by the language ComboBox on the settings page. </remarks>
    public object Convert(object? value, Type targetType, object? parameter, CultureInfo culture)
    {
        if (value is UILanguage language)
        {
            return language.Language!;
        }
        return "unknown";
    }

    /// <summary>
    /// Convert the UILanguage object.
    /// </summary>
    /// <returns>The language code.</returns>
    public object ConvertBack(object? value, Type targetType, object? parameter, CultureInfo culture)
    {
        if (value is UILanguage language)
        {
            return language.LanguageCode!;
        }
        return "unknown";
    }
}
