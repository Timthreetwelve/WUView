// Copyright (c) Tim Kennedy. All Rights Reserved. Licensed under the MIT License.

namespace WUView.Helpers;

internal static class WUApiHelpers
{
    public static void LogWUAInfo()
    {
        try
        {
            _log.Debug($"Windows Update Agent product version: {GetWUAInfo("ProductVersionString")}");
            _log.Debug($"Windows Update Agent major version: {GetWUAInfo("ApiMajorVersion")} minor version: {GetWUAInfo("ApiMinorVersion")}");
        }
        catch (Exception ex)
        {
            _log.Error(ex, $"Error attempting to get Windows Update Agent info. {ex.Message}");
        }
    }

    private static string GetWUAInfo(string wuaObj)
    {
        IWindowsUpdateAgentInfo updateAgentInfo = new();
        string value = updateAgentInfo.GetInfo(wuaObj).ToString()!;
        return value ?? string.Empty;
    }

    public static void LogWUEnabled()
    {
        string msg = string.Empty;
        try
        {
            IAutomaticUpdates automaticUpdates = new();
            if (automaticUpdates.ServiceEnabled)
            {
                msg = "Windows Update service is enabled.";
            }
            else
            {
                msg = "Windows Update service is not enabled.";
            }
        }
        catch (Exception ex)
        {
            msg = ex.Message;
        }
        finally
        {
            _log.Debug(msg);
        }
    }
}
