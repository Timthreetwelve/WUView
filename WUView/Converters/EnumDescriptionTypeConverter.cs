// Copyright (c) Tim Kennedy. All Rights Reserved. Licensed under the MIT License.

namespace WUView.Converters;

/// <summary>
/// Enum description converter.
/// </summary>
/// <remarks>
///  Based on https://brianlagunas.com/localize-enum-descriptions-in-wpf/
/// </remarks>
internal class EnumDescriptionTypeConverter : EnumConverter
{
    public EnumDescriptionTypeConverter(Type type) : base(type)
    {
    }

    public override object ConvertTo(ITypeDescriptorContext context, CultureInfo culture, object value, Type destinationType)
    {
        if (destinationType == typeof(string))
        {
            if (value != null)
            {
                FieldInfo fi = value.GetType().GetField(value.ToString());
                if (fi != null)
                {
                    DescriptionAttribute[] attributes = (DescriptionAttribute[])fi.GetCustomAttributes(typeof(DescriptionAttribute), false);
                    if ((attributes.Length > 0) && (!string.IsNullOrEmpty(attributes[0].Description)))
                    {
                        return attributes[0].Description;
                    }
                    return value.ToString();
                }
            }
            return string.Empty;
        }
        return base.ConvertTo(context, culture, value, destinationType);
    }
}
