// ---------------------------------------------------------
// This is the .NET Framework version using Newtonsoft.Json
// ---------------------------------------------------------
// Inspired by these answers on Stack Overflow:
// https://stackoverflow.com/a/56961180/10277297 and https://stackoverflow.com/a/62220399/10277297

#region Using directives
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Reflection;
using Newtonsoft.Json;
using System.Windows;
#endregion Using directives

namespace TKUtils
{
    /// <summary>
    /// A class and methods for reading, updating and saving user settings in a JSON file
    /// </summary>
    /// <typeparam name="T">Class name of user settings</typeparam>
    public abstract class SettingsManager<T> where T : SettingsManager<T>, new()
    {
        #region Constants
        internal const string AppData = "APPDATA_FOLDER";
        internal const string AppFolder = "APPLICATION_FOLDER";
        internal const string LocalAppData = "LOCAL_APPDATA_FOLDER";
        internal const string DefaultFilename = "DEFAULT_FILENAME";
        #endregion Constants

        #region Properties
        private static string Folder { get; set; }
        private static string FileName { get; set; }
        private static string FilePath { get; set; }
        public static T Setting { get; private set; }
        #endregion Properties

        #region Initialization
        /// <summary>
        ///  Initialization method. Gets the file name for settings file and creates it if it
        ///  doesn't exist. Optionally loads the settings file.
        /// </summary>
        /// <param name="folder">Folder name can be a path or one of the const values</param>
        /// <param name="fileName">File name can be a file name (without path) or DEFAULT</param>
        /// <param name="load">Read and load the settings file during initialization</param>
        public static void Init(string folder, string fileName, bool load)
        {
            Folder = folder;
            FileName = fileName;
            GetSettingsFile(Folder, FileName);
            if (!File.Exists(FilePath))
            {
                CreateNewSettingsJson(FilePath);
            }
            if (load)
            {
                LoadSettings();
            }
        }
        #endregion Initialization

        #region Get the settings file path
        /// <summary>
        /// Returns path to settings file. Accepts constants for folder and filename
        /// </summary>
        /// <param name="Folder"></param>
        /// <param name="FileName"></param>
        /// <returns>Path to settings file</returns>
        private static void GetSettingsFile(string Folder, string FileName)
        {
            string folderPath;
            string appName = Assembly.GetEntryAssembly().GetName().Name;
            string companyName = FileVersionInfo
                                 .GetVersionInfo(Assembly.GetEntryAssembly().Location)
                                 .CompanyName ?? string.Empty;

            switch (Folder)
            {
                case "LOCAL_APPDATA_FOLDER":
                    folderPath = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData),
                        companyName, appName);
                    break;
                case "APPDATA_FOLDER":
                    folderPath = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData),
                        companyName, appName);
                    break;
                case "APPLICATION_FOLDER":
                    folderPath = Path.GetDirectoryName(Assembly.GetEntryAssembly().Location);
                    break;
                default:
                    folderPath = Folder;
                    break;
            }
            if (FileName == "DEFAULT_FILENAME")
            {
                FileName = "usersettings.json";
            }
            FilePath = Path.Combine(folderPath, FileName);
        }
        #endregion Get the settings file path

        #region Read settings file
        /// <summary>
        /// Reads settings from a JSON format settings file
        /// </summary>
        public static void LoadSettings()
        {
            if (File.Exists(FilePath))
            {
                try
                {
                    Setting = JsonConvert.DeserializeObject<T>(File.ReadAllText(FilePath));
                }
                catch (Exception ex)
                {
                    _ = MessageBox.Show($"Error reading settings file.\n{ex}",
                                        "Error",
                                        MessageBoxButton.OK,
                                        MessageBoxImage.Error);
                }
            }
            else
            {
                Setting = new T();
            }
        }
        #endregion Read settings file

        #region Save settings
        /// <summary>
        /// Writes settings to settings file
        /// </summary>
        public static void SaveSettings()
        {
            try
            {
                string json = JsonConvert.SerializeObject(Setting, Formatting.Indented);
                File.WriteAllText(FilePath, json);
            }
            catch (Exception ex)
            {
                _ = MessageBox.Show($"Error saving settings file.\n{ex}",
                                     "Error",
                                     MessageBoxButton.OK,
                                     MessageBoxImage.Error);
            }
        }
        #endregion Save settings

        #region Create a new settings file
        /// <summary>
        /// Creates a new, empty JSON settings file
        /// </summary>
        /// <param name="filepath">Complete path and file name</param>
        private static void CreateNewSettingsJson(string filepath)
        {
            try
            {
                if (!Directory.Exists(Path.GetDirectoryName(filepath)))
                {
                    _ = Directory.CreateDirectory(Path.GetDirectoryName(filepath));
                }
                File.Create(filepath).Dispose();
                const string braces = "{ }";
                File.WriteAllText(filepath, braces);
            }
            catch (Exception ex)
            {
                _ = MessageBox.Show($"Error creating settings file.\n{ex}",
                                     "Error",
                                     MessageBoxButton.OK,
                                     MessageBoxImage.Error);
            }
        }
        #endregion Create a new settings file

        #region List all properties and their values
        public static Dictionary<string, object> ListSettings()
        {
            Type type = typeof(T);
            Dictionary<string, object> properties = new Dictionary<string, object>();
            foreach (PropertyInfo p in type.GetProperties())
            {
                properties.Add(p.Name, p.GetValue(Setting));
            }
            return properties;
        }
        #endregion List all properties and their values

        #region Get settings file name
        internal static string GetSettingsFilename()
        {
            return FilePath;
        }
        #endregion Get settings file name
    }
}
