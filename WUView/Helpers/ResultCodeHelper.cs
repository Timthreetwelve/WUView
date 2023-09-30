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
        switch (resultCode)
        {
            case OperationResultCode.orcNotStarted:
                return GetStringResource("ResultCode_NotStarted");
            case OperationResultCode.orcInProgress:
                return GetStringResource("ResultCode_InProgress");
            case OperationResultCode.orcSucceeded:
                return GetStringResource("ResultCode_Succeeded");
            case OperationResultCode.orcSucceededWithErrors:
                return GetStringResource("ResultCode_SucceededWithErrors");
            case OperationResultCode.orcFailed:
                return GetStringResource("ResultCode_Failed");
            case OperationResultCode.orcAborted:
                return GetStringResource("ResultCode_Aborted");
            default:
                return "unknown";
        }
    }
}
