// Copyright(c) Tim Kennedy. All Rights Reserved. Licensed under the MIT License.

namespace WUView.Helpers;

internal static class DialogHelpers
{
    /// <summary>
    /// Shows the dialog used to view and edit the excludes file
    /// </summary>
    /// <returns>Returns <see langword="true"/> if the user clicked OK, <see langword="false"/> otherwise.</returns>
    internal static async Task<bool> ShowEditExcludesDialog()
    {
        ExcludesEditor ee = new();
        object retval = await DialogHost.Show(ee, "MainDialogHost");
        return retval != null && (bool)retval;
    }
}
