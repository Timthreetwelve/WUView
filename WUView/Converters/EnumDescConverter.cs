// Copyright (c) Tim Kennedy. All Rights Reserved. Licensed under the MIT License.

namespace WUView.Converters;

/// <summary>
/// Enum description converter.
/// </summary>
/// <remarks>
/// Allows use of "Light Blue" instead of LightBlue or Light_Blue.
/// </remarks>
internal abstract class EnumDescConverter : IValueConverter
{
    /// <summary>
    /// Converts a value.
    /// </summary>
    /// <param name="value">The value produced by the binding source.</param>
    /// <param name="targetType">The type of the binding target property.</param>
    /// <param name="parameter">The converter parameter to use.</param>
    /// <param name="culture">The culture to use in the converter.</param>
    /// <returns>
    /// A converted value. If the method returns <see langword="null" />, the valid null value is used.
    /// </returns>
    public object? Convert(object? value, Type targetType, object? parameter, CultureInfo culture)
    {
        if (value is not Enum myEnum)
        {
            return null;
        }
        string description = GetEnumDescription(myEnum);
        return !string.IsNullOrEmpty(description) ? description : myEnum.ToString();
    }

    /// <summary>
    /// Gets the enum description.
    /// </summary>
    /// <param name="enumObj">The enum</param>
    /// <returns>The description</returns>
    public static string GetEnumDescription(Enum enumObj)
    {
        FieldInfo? field = enumObj.GetType().GetField(enumObj.ToString());
        object[] attrArray = field!.GetCustomAttributes(false);

        if (attrArray.Length > 0)
        {
            DescriptionAttribute? attribute = attrArray[0] as DescriptionAttribute;
            return attribute!.Description;
        }
        else
        {
            return enumObj.ToString();
        }
    }

    /// <summary>
    /// Not used.
    /// </summary>
    public object ConvertBack(object? value, Type targetType, object? parameter, CultureInfo culture)
    {
        return string.Empty;
    }
}
