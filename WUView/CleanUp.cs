// Copyright(c) Tim Kennedy. All Rights Reserved. Licensed under the MIT License.

#region using directives
using System.Configuration;
using System.Diagnostics;
using System.IO;
using System.Reflection;
using System.Text.RegularExpressions;
#endregion

namespace TKUtils
{
    /// <summary>
    ///  Class for cleaning up settings folders from previous versions
    /// </summary>
    public static class CleanUp
    {
        private static string GetVersion()
        {
            // Determine current app version
            return Assembly.GetEntryAssembly().GetName().Version.ToString();
        }

        /// <summary>
        /// Deletes settings folders in %localappdata%\AppName that are left over after an upgrade
        /// </summary>
        public static void CleanupPrevSettings()
        {
            // Determine what folder the user.config file is in
            string config = ConfigurationManager.OpenExeConfiguration
                (ConfigurationUserLevel.PerUserRoamingAndLocal).FilePath;

            // Jump up a directory (parent directory)
            DirectoryInfo parentPath = Directory.GetParent(Directory.GetParent(config).FullName);

            //  Get a list of all the directories
            DirectoryInfo[] dirs = parentPath.GetDirectories();
            int count = dirs.Length;

            // If there is more than one directory, then the others are from a previous version
            if (count > 1)
            {
                // Directory name must match 'num.num.num.num' with no alpha characters
                Regex regex = new Regex(@"(\A\d+.\d+.\d+.\d+\z)");
                foreach (DirectoryInfo dir in dirs)
                {
                    // Delete all the directories that aren't for the current version
                    Match match = regex.Match(dir.Name);
                    if (dir.Name != GetVersion() && match.Success)
                    {
                        dir.Delete(true);
                        Debug.WriteLine($">>> Delete {dir.FullName}");
                    }
                }
            }
        }
    }
}
