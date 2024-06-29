// Copyright (c) Tim Kennedy. All Rights Reserved. Licensed under the MIT License.

namespace WUView.Helpers;
/// <summary>
/// Class to localize Result Codes.
/// </summary>
internal static class ResultCodeHelper
{
    /// <summary>
    /// Localizes the Result Code. Also removes the unwanted "orc" prefix.
    /// </summary>
    /// <param name="resultCode">Result code from update history.</param>
    /// <returns>Result code as localized string.</returns>
    public static string TranslateResultCode(OperationResultCode resultCode)
    {
        return resultCode switch
        {
            OperationResultCode.orcNotStarted => GetStringResource("ResultCode_NotStarted"),
            OperationResultCode.orcInProgress => GetStringResource("ResultCode_InProgress"),
            OperationResultCode.orcSucceeded => GetStringResource("ResultCode_Succeeded"),
            OperationResultCode.orcSucceededWithErrors => GetStringResource("ResultCode_SucceededWithErrors"),
            OperationResultCode.orcFailed => GetStringResource("ResultCode_Failed"),
            OperationResultCode.orcAborted => GetStringResource("ResultCode_Aborted"),
            _ => "unknown",
        };
    }
}
