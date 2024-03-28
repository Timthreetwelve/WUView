// Copyright (c) Tim Kennedy. All Rights Reserved. Licensed under the MIT License.

namespace WUView.Converters;

/// <summary>
/// Allows a Description Attribute in an Enum to be localized
/// </summary>
/// <remarks>
/// Based on https://brianlagunas.com/localize-enum-descriptions-in-wpf/
/// </remarks>
/// <seealso cref="System.ComponentModel.DescriptionAttribute" />
internal class LocalizedDescriptionAttribute(string resourceKey) : DescriptionAttribute
{
    readonly string _resourceKey = resourceKey;

    public override string Description
    {
        get
        {
            object description;
            try
            {
                description = Application.Current.TryFindResource(_resourceKey);
            }
            catch (Exception)
            {
                return $"{_resourceKey} value is null";
            }

            if (description is null)
            {
                return $"{_resourceKey} resource not found";
            }

            return description.ToString();
        }
    }
}
