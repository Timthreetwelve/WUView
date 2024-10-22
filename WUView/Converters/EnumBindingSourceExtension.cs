// Copyright (c) Tim Kennedy. All Rights Reserved. Licensed under the MIT License.
#nullable disable
namespace WUView.Converters;

/// <summary>
/// Markup Extension used to allow an Enum to be used as an ItemsSource
/// </summary>
/// <remarks>
/// Based on https://brianlagunas.com/a-better-way-to-data-bind-enums-in-wpf/
/// </remarks>
/// <seealso cref="System.Windows.Markup.MarkupExtension" />
internal sealed class EnumBindingSourceExtension : MarkupExtension
{
    private Type _enumType;
    public Type EnumType
    {
        set
        {
            if (value != _enumType)
            {
                if (value != null)
                {
                    Type enumType = Nullable.GetUnderlyingType(value) ?? value;
                    if (!enumType.IsEnum)
                        throw new ArgumentException("Type must be for an Enum.");
                }

                _enumType = value;
            }
        }
    }

    public EnumBindingSourceExtension(Type enumType)
    {
        EnumType = enumType;
    }

    public override object ProvideValue(IServiceProvider serviceProvider)
    {
        if (_enumType == null)
            throw new InvalidOperationException("The EnumType must be specified.");

        Type actualEnumType = Nullable.GetUnderlyingType(_enumType) ?? _enumType;
        Array enumValues = Enum.GetValues(actualEnumType);

        if (actualEnumType == _enumType)
            return enumValues;

        Array tempArray = Array.CreateInstance(actualEnumType, enumValues.Length + 1);
        enumValues.CopyTo(tempArray, 1);
        return tempArray;
    }
}
