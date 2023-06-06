// Copyright (c) Tim Kennedy. All Rights Reserved. Licensed under the MIT License.

namespace WUView.Configuration;

/// <summary>
/// Class for methods used for creating, reading and saving settings.
/// </summary>
public static class ConfigHelpers
{
    #region Properties
    public static string SettingsFileName { get; set; }
    #endregion Properties

    #region Initialize settings
    /// <summary>
    ///  Initialization method. Gets the file name for settings file and creates it if it
    ///  doesn't exist.
    /// </summary>
    /// <param name="settingsFile">Option name of settings file</param>
    public static void InitializeSettings(string settingsFile = "usersettings.json")
    {
        string settingsDir = Path.GetDirectoryName(AppContext.BaseDirectory);
        SettingsFileName = Path.Combine(settingsDir, settingsFile);

        if (!File.Exists(SettingsFileName))
        {
            UserSettings.Setting = new UserSettings();
            SaveSettings();
        }
        ConfigManager<UserSettings>.Setting = ReadConfiguration();

        ConfigManager<TempSettings>.Setting = new TempSettings();
    }
    #endregion Initialize settings

    #region Read setting from file
    /// <summary>
    /// Read settings from JSON file.
    /// </summary>
    /// <returns>UserSettings</returns>
    public static UserSettings ReadConfiguration()
    {
        try
        {
            return JsonSerializer.Deserialize<UserSettings>(File.ReadAllText(SettingsFileName));
        }
        catch (Exception ex)
        {
            _ = MessageBox.Show($"Error reading settings file.\n{ex.Message}",
                     "Error",
                     MessageBoxButton.OK,
                     MessageBoxImage.Error);
            return new UserSettings();
        }
    }
    #endregion Read setting from file

    #region Save settings to JSON file
    /// <summary>
    /// Write settings to JSON file.
    /// </summary>
    public static void SaveSettings()
    {
        try
        {
            JsonSerializerOptions options = new() { WriteIndented = true };
            string json = JsonSerializer.Serialize(UserSettings.Setting, options);
            File.WriteAllText(SettingsFileName, json);
        }
        catch (Exception ex)
        {
            _ = MessageBox.Show($"Error saving settings file.\n{ex.Message}",
                     "Error",
                     MessageBoxButton.OK,
                     MessageBoxImage.Error);
        }
    }
    #endregion Save settings to JSON file
}
