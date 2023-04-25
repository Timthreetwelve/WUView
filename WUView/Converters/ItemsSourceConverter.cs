// Copyright (c) Tim Kennedy. All Rights Reserved. Licensed under the MIT License.

namespace WUView.Converters;

/// <summary>
/// Sets the desired of updates.
/// </summary>
/// <seealso cref="System.Windows.Data.IValueConverter" />
internal class ItemsSourceConverter : IValueConverter
{
    public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
    {
        int fullCount = MainViewModel.UpdatesFullList.Count;
        int withoutCount = MainViewModel.UpdatesWithoutExcludedItems.Count;

        if (UserSettings.Setting.HideExcluded)
        {
            if (MainPage.Instance is not null)
            {
                SnackbarMsg.ClearAndQueueMessage($"Displaying {withoutCount} of {fullCount} updates");
            }
            return MainViewModel.UpdatesWithoutExcludedItems;
        }
        else
        {
            if (MainPage.Instance is not null)
            {
                SnackbarMsg.ClearAndQueueMessage($"Displaying all {fullCount} updates");
            }
            return MainViewModel.UpdatesFullList;
        }
    }

    public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
    {
        return Binding.DoNothing;
    }
}
