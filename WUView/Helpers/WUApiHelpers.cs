// Copyright (c) Tim Kennedy. All Rights Reserved. Licensed under the MIT License.

namespace WUView.Helpers;

internal static class WUApiHelpers
{
    #region Log Windows Update Agent info
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
    #endregion Log Windows Update Agent info

    #region Get Windows Update Agent info
    private static string GetWUAInfo(string wuaObj)
    {
        IWindowsUpdateAgentInfo updateAgentInfo = new();
        return updateAgentInfo.GetInfo(wuaObj).ToString()!;
    }
    #endregion Get Windows Update Agent info

    #region Log Windows Update Service status
    public static void LogWUEnabled()
    {
        try
        {
            IAutomaticUpdates automaticUpdates = new();
            if (automaticUpdates.ServiceEnabled)
            {
                _log.Info("Windows Update Service is enabled.");
            }
            else
            {
                _log.Warn("Windows Update Service is not enabled.");
            }
        }
        catch (Exception ex)
        {
            _log.Error(ex, "Error checking Windows Update service status");
        }
    }
    #endregion Log Windows Update Service status
}
