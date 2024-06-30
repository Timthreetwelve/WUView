// Copyright (c) Tim Kennedy. All Rights Reserved. Licensed under the MIT License.

namespace WUView.Helpers;
/// <summary>
/// Class to localize Operation string.
/// </summary>
public static class OperationHelper
{
    /// <summary>
    /// Localizes the Operation string. Also removes the unwanted "uo" prefix.
    /// </summary>
    /// <param name="operation">Operation type from update history.</param>
    /// <returns>Operation type as localized string.</returns>
    public static string TranslateOperation(UpdateOperation operation)
    {
        return operation switch
        {
            UpdateOperation.uoInstallation => GetStringResource("OperationType_Installation"),
            UpdateOperation.uoUninstallation => GetStringResource("OperationType_Uninstallation"),
            _ => "unknown",
        };
    }
}
