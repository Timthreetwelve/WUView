// Copyright(c) Tim Kennedy. All Rights Reserved. Licensed under the MIT License.

#region Using directives
using System;
using System.Diagnostics;
using System.IO;
using System.Reflection;
#endregion

namespace TKUtils
{
    /// <summary>
    /// Class to return information about the current application
    /// </summary>
    public static class AppInfo
    {
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
                return version.ToString().Remove(version.ToString().LastIndexOf("."));
            }
        }

        /// <summary>
        /// Returns the full version number
        /// </summary>
        public static string AppVersion =>
                Assembly.GetEntryAssembly().GetName().Version.ToString();

        /// <summary>
        /// Returns the app's full path including the EXE name
        /// </summary>
        public static string AppPath =>
                Assembly.GetEntryAssembly().Location;

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
                if (!string.IsNullOrWhiteSpace(info))
                {
                    return info;
                }
                else
                {
                    return "missing";
                }
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
                if (!string.IsNullOrWhiteSpace(info))
                {
                    return info;
                }
                else
                {
                    return "missing";
                }
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
                if (!string.IsNullOrWhiteSpace(info))
                {
                    return info;
                }
                else
                {
                    return "missing";
                }
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
                if (!string.IsNullOrWhiteSpace(info))
                {
                    return info;
                }
                else
                {
                    return "missing";
                }
            }
        }

        /// <summary>
        /// Returns the Process Name
        /// </summary>
        public static string AppProcessName =>
                Process.GetCurrentProcess().ProcessName;

        /// <summary>
        /// Returns the Process ID as Int
        /// </summary>
        public static int AppProcessID =>
                Process.GetCurrentProcess().Id;

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
    }
}
