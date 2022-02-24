// Copyright(c) Tim Kennedy. All Rights Reserved. Licensed under the MIT License.

namespace WUView;

internal static class DialogHelpers
{
    /// <summary>
    /// Shows the About dialog.
    /// </summary>
    internal static async void ShowAboutDialog()
    {
        About about = new();
        _ = await DialogHost.Show(about, "MainDialogHost");
    }

    /// <summary>
    /// Shows the Settings dialog
    /// </summary>
    internal static async void ShowSettingsDialog()
    {
        Settings settings = new();
        _ = await DialogHost.Show(settings, "MainDialogHost");
    }

    /// <summary>
    /// Shows the dialog used to view and edit the excludes file
    /// </summary>
    /// <returns>Returns <see langword="true"/> if the user clicked OK, <see langword="false"/> otherwise.</returns>
    internal static async Task<bool> ShowEditExcludesDialog()
    {
        ExcludesEditor ee = new();
        object retval = await DialogHost.Show(ee, "MainDialogHost");
        if (retval != null && (bool)retval)
        {
            List<ExcludedItems> exItems = new();
            for (int line = 0; line < ee.tb1.LineCount; line++)
            {
                ExcludedItems xi = new();
                string tbline = ee.tb1.GetLineText(line).TrimEnd('\n').TrimEnd('\r');
                if (!string.IsNullOrWhiteSpace(tbline))
                {
                    xi.ExcludedString = tbline;
                    if (!exItems.Contains(xi))
                    {
                        exItems.Add(xi);
                    }
                }
            }
            ExcludedItems.ExcludedStrings.Clear();
            ExcludedItems.ExcludedStrings = exItems;
            return true;
        }
        return false;
    }
}
