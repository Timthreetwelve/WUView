﻿// Copyright (c) Tim Kennedy. All Rights Reserved. Licensed under the MIT License.

namespace WUView.Converters;

/// <summary>
/// Converts excluded items in the observable collection to strings for use in a textbox
/// </summary>
/// <seealso cref="System.Windows.Data.IValueConverter" />
internal sealed class ExcludedItemsConverter : IValueConverter
{
    public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
    {
        if (value != null)
        {
            StringBuilder sb = new();
            foreach (ExcludedItems item in (ObservableCollection<ExcludedItems>)value)
            {
                _ = sb.AppendLine(item.ExcludedString);
            }
            return sb.ToString();
        }
        return string.Empty;
    }

    public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
    {
        if (value is not null)
        {
            string[] items = value.ToString()!.Split(new string[] { Environment.NewLine }, StringSplitOptions.RemoveEmptyEntries);
            ObservableCollection<ExcludedItems> excludedItems = [];
            foreach (string item in items)
            {
                ExcludedItems excluded = new()
                {
                    ExcludedString = item
                };
                excludedItems.Add(excluded);
            }
            return excludedItems;
        }
        return string.Empty;
    }
}
