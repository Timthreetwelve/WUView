// Copyright (c) Tim Kennedy. All Rights Reserved. Licensed under the MIT License.

namespace WUView.Configuration;

/// <summary>
/// Class for methods used for creating, reading and saving settings.
/// </summary>
public static class ConfigHelpers
{
    #region Properties
    public static string SettingsFileName { get; private set; } = null!;

    private static JsonSerializerOptions JsonOptions { get; } = new()
    {
        WriteIndented = true
    };
    #endregion Properties

    #region Initialize settings
    /// <summary>
    ///  Initialization method. Gets the file name for settings file and creates it if it
    ///  doesn't exist.
    /// </summary>
    /// <param name="settingsFile">Option name of settings file</param>
    public static void InitializeSettings(string settingsFile = "usersettings.json")
    {
        string? settingsDir = Path.GetDirectoryName(AppContext.BaseDirectory);
        SettingsFileName = Path.Combine(settingsDir!, settingsFile);

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
    private static UserSettings ReadConfiguration()
    {
        try
        {
            string json = File.ReadAllText(SettingsFileName);
            UserSettings? settings = JsonSerializer.Deserialize<UserSettings>(json);
            return settings!;
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
            string json = JsonSerializer.Serialize(UserSettings.Setting, JsonOptions);
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
