// Copyright (c) Tim Kennedy. All Rights Reserved. Licensed under the MIT License.

namespace WUView.Helpers;

internal static class WUApiHelpers
{
    #region NLog Instance
    private static readonly Logger _log = LogManager.GetLogger("logTemp");
    #endregion NLog Instance

    public static void LogWUAInfo()
    {
        try
        {
            foreach (KeyValuePair<string, string> item in GetWUAInfo())
            {
                _log.Debug($"Windows Update Agent {item.Key}: {item.Value}");
            }
        }
        catch (Exception ex)
        {
            _log.Error(ex, $"Error attempting to get Windows Update Agent info. {ex.Message}");
        }
    }

    private static Dictionary<string, string> GetWUAInfo()
    {
        WindowsUpdateAgentInfo updateAgentInfo = new();

        return new()
        {
            ["Product Version"] = updateAgentInfo.GetInfo("ProductVersionString").ToString(),
            ["Major Version"] = updateAgentInfo.GetInfo("ApiMajorVersion").ToString(),
            ["Minor Version"] = updateAgentInfo.GetInfo("ApiMinorVersion").ToString()
        };
    }
}
