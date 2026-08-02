// Copyright (c) Tim Kennedy. All Rights Reserved. Licensed under the MIT License.

namespace WUView.Helpers;

/// <summary>
/// Class to return information about the current application
/// </summary>
internal static class AppInfo
{
    /// <summary>
    /// Returns the process architecture e.g. X64, Arm64, etc.
    /// </summary>
    public static string Architecture => RuntimeInformation.ProcessArchitecture.ToString().ToLowerInvariant();

    /// <summary>
    /// Returns the full version number as String
    /// </summary>
    public static string AppVersion => Assembly.GetEntryAssembly()!.GetName().Version!.ToString();

    /// <summary>
    /// Returns the full version number as Version
    /// </summary>
    public static Version AppVersionVer => Assembly.GetEntryAssembly()!.GetName().Version!;

    /// <summary>
    /// Returns the app's full path including the EXE name
    /// </summary>
    public static string AppPath => Environment.ProcessPath!;

    /// <summary>
    /// Returns the app's full path excluding the EXE name
    /// </summary>
    public static string AppDirectory => Path.GetDirectoryName(Assembly.GetEntryAssembly()!.Location) ?? "missing";

    /// <summary>
    /// Returns the app's name without the extension
    /// </summary>
    public static string AppName => (Assembly.GetEntryAssembly()!.GetName().Name) ?? "missing";

    /// <summary>
    /// Returns the product version from the Assembly info
    /// </summary>
    // Don't be fooled by 0 references to this property. It is used in the About window.
    public static string AppProductVersion => FileVersionInfo.GetVersionInfo(Assembly.GetEntryAssembly()!.Location).ProductVersion ?? "missing";

    /// <summary>
    /// Returns the Copyright info from the Assembly info
    /// </summary>
    public static string AppCopyright => FileVersionInfo.GetVersionInfo(Assembly.GetEntryAssembly()!.Location).LegalCopyright ?? "missing";

    /// <summary>
    /// Returns the Product Name from the Assembly info
    /// </summary>
    public static string AppProduct => FileVersionInfo.GetVersionInfo(Assembly.GetEntryAssembly()!.Location).ProductName ?? "missing";

    /// <summary>
    /// Returns the Process ID as Int
    /// </summary>
    public static int AppProcessID => Environment.ProcessId;

    /// <summary>
    /// True if running as administrator
    /// </summary>
    public static bool IsAdmin => new WindowsPrincipal(WindowsIdentity.GetCurrent()).IsInRole(WindowsBuiltInRole.Administrator);

    /// <summary>
    /// Returns the operating system description e.g. Microsoft Windows 10.0.19044
    /// </summary>
    public static string OsPlatform => RuntimeInformation.OSDescription;

    /// <summary>
    /// Returns the framework description
    /// </summary>
    public static string RuntimeVersion => RuntimeInformation.FrameworkDescription;
}
