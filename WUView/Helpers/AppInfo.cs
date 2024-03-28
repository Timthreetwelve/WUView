// Copyright (c) Tim Kennedy. All Rights Reserved. Licensed under the MIT License.

namespace WUView.Helpers;

/// <summary>
/// Class to return information about the current application
/// </summary>
public static class AppInfo
{
    /// <summary>
    /// Returns the operating system description e.g. Microsoft Windows 10.0.19044
    /// </summary>
    public static string OsPlatform => RuntimeInformation.OSDescription;

    /// <summary>
    /// Returns the framework name
    /// </summary>
    public static string Framework => Assembly.GetEntryAssembly()?.GetCustomAttribute<TargetFrameworkAttribute>()?.FrameworkName;

    /// <summary>
    /// Returns the framework description
    /// </summary>
    public static string RuntimeVersion => RuntimeInformation.FrameworkDescription;

    /// <summary>
    ///  Returns the version number in Major.Minor.Build format
    /// </summary>
    public static string TitleVersion
    {
        get
        {
            // Get the assembly version
            Version version = Assembly.GetEntryAssembly().GetName().Version;

            // Remove the release (last) node
            return version.ToString().Remove(version.ToString().LastIndexOf('.'));
        }
    }

    /// <summary>
    /// Returns the file version
    /// </summary>
    public static string AppFileVersion =>
        Assembly.GetEntryAssembly().GetCustomAttribute<AssemblyFileVersionAttribute>().Version;

    /// <summary>
    /// Returns the full version number as String
    /// </summary>
    public static string AppVersion =>
            Assembly.GetEntryAssembly().GetName().Version.ToString();

    /// <summary>
    /// Returns the full version number as Version
    /// </summary>
    public static Version AppVersionVer =>
            Assembly.GetEntryAssembly().GetName().Version;

    /// <summary>
    /// Returns the app's full path including the EXE name
    /// </summary>
    public static string AppPath =>
            Environment.ProcessPath;

    /// <summary>
    /// Returns the app's full path excluding the EXE name
    /// </summary>
    public static string AppDirectory =>
            Path.GetDirectoryName(Assembly.GetEntryAssembly().Location);

    /// <summary>
    /// Returns the app's name without the extension
    /// </summary>
    public static string AppName =>
            Assembly.GetEntryAssembly().GetName().Name;

    /// <summary>
    /// Returns the app's name with the extension
    /// </summary>
    public static string AppExeName =>
            Path.GetFileName(Assembly.GetEntryAssembly().Location);

    /// <summary>
    /// Returns the app's full name (name, version, culture, etc.)
    /// </summary>
    public static string AppFullName =>
            Assembly.GetEntryAssembly().GetName().FullName;

    /// <summary>
    /// Returns the Company Name from the Assembly info
    /// </summary>
    public static string AppCompany
    {
        get
        {
            var info = FileVersionInfo.GetVersionInfo(Assembly.GetEntryAssembly().Location).CompanyName;
            return !string.IsNullOrWhiteSpace(info) ? info : "missing";
        }
    }

    /// <summary>
    /// Returns the Author from the Assembly info
    /// </summary>
    public static string AppDescription
    {
        get
        {
            string info = FileVersionInfo.GetVersionInfo(Assembly.GetEntryAssembly().Location).FileDescription;
            return !string.IsNullOrWhiteSpace(info) ? info : "missing";
        }
    }

    /// <summary>
    /// Returns the Copyright info from the Assembly info
    /// </summary>
    public static string AppCopyright
    {
        get
        {
            var info = FileVersionInfo.GetVersionInfo(Assembly.GetEntryAssembly().Location).LegalCopyright;
            return !string.IsNullOrWhiteSpace(info) ? info : "missing";
        }
    }

    /// <summary>
    /// Returns the Product Name from the Assembly info
    /// </summary>
    public static string AppProduct
    {
        get
        {
            string info = FileVersionInfo.GetVersionInfo(Assembly.GetEntryAssembly().Location).ProductName;
            return !string.IsNullOrWhiteSpace(info) ? info : "missing";
        }
    }

    /// <summary>
    /// Returns the File Name from the Assembly info
    /// </summary>
    public static string AppFileName
    {
        get
        {
            string info = FileVersionInfo.GetVersionInfo(Assembly.GetEntryAssembly().Location).FileName;
            return !string.IsNullOrWhiteSpace(info) ? info : "missing";
        }
    }

    /// <summary>
    /// Combines the product name with the title version.
    /// </summary>
    /// <value>
    /// String in the format: AppName - 0.0.1
    /// </value>
    public static string ToolTipVersion => $"{AppProduct} - {TitleVersion}";

    /// <summary>
    /// Returns the Process Name
    /// </summary>
    public static string AppProcessName =>
            Process.GetCurrentProcess().ProcessName;

    /// <summary>
    /// Returns the Process ID as Int
    /// </summary>
    public static int AppProcessID =>
            Environment.ProcessId;

    /// <summary>
    /// Returns the Process Start Time as DateTime
    /// </summary>
    public static DateTime AppProcessStart =>
            Process.GetCurrentProcess().StartTime;

    /// <summary>
    /// Returns the Process MainModule
    /// </summary>
    public static string AppProcessMainModule =>
            Process.GetCurrentProcess().MainModule.ModuleName;

    /// <summary>
    /// The CLR version
    /// </summary>
    public static string CLRVersion => Environment.Version.ToString();
}
