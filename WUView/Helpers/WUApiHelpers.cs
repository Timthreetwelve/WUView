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

#pragma warning disable S125 // Sections of code should not be commented out
    // to enable the following, change <EmbedInteropTypes>true</EmbedInteropTypes> to false in .csproj

    //public static bool IsWUEnabled()

                            //{
                            //    AutomaticUpdatesClass automaticUpdatesClass = new();
                            //    return automaticUpdatesClass.ServiceEnabled;
                            //}

    //public static void LogWUEnabled()
    //{
    //    _log.Debug($"Windows Update service enabled: {IsWUEnabled()}");
    //}
}
#pragma warning restore S125 // Sections of code should not be commented out
