// Copyright (c) Tim Kennedy. All Rights Reserved. Licensed under the MIT License.

namespace WUView.Helpers;

/// <summary>
/// Helper methods for reading the keyboard shortcuts from the resource dictionary and creating a list of shortcut
/// objects for the view model.
/// </summary>
internal static class ShortcutHelpers
{
    #region Constants including curated order
    private const string ShortcutPrefix = "Shortcut_";
    private const string DescriptionSuffix = "_Description";
    private const string GestureSuffix = "_Gesture";
    private const string FallbackLanguage = "en-US";

    // Curated display order (matches NavigationViewModel.KeyDown intent)
    private static readonly List<string> CuratedShortcutBaseKeys =
    [
        "Shortcut_GoToAbout",
        "Shortcut_RefreshUpdates",
        "Shortcut_ClearFilter",

        "Shortcut_ToggleDetails",
        "Shortcut_ToggleExcluded",
        "Shortcut_FocusFilter",
        "Shortcut_EditExcludeList",
        "Shortcut_RemoveColumnSort",
        "Shortcut_CycleDateFormat",
        "Shortcut_GoToUpdates",
        "Shortcut_GoToSettings",
        "Shortcut_IncreaseUISize",
        "Shortcut_DecreaseUISize",

        "Shortcut_CycleAccentColor",
        "Shortcut_OpenAppFolder",
        "Shortcut_CompareLanguage",
        "Shortcut_CycleRowHeight",
        "Shortcut_OpenSettingsFile",
        "Shortcut_CycleTheme",
        "Shortcut_IncreaseFontSize",
        "Shortcut_DecreaseFontSize"
    ];
    #endregion Constants including curated order

    #region Public methods
    /// <summary>
    /// Gets a list of localized keyboard shortcuts suitable for binding to a DataGrid or ListView.
    /// </summary>
    /// <returns>List of shortcut items containing description and gesture text.</returns>
    public static List<ShortcutItem> GetShortcutList()
    {
        ResourceDictionary fallbackDictionary = LoadDictionary(FallbackLanguage);
        ResourceDictionary currentDictionary = GetCurrentOrFallbackDictionary();

        List<ShortcutItem> shortcutList = [];

        List<string> shortcutBaseKeys = GetShortcutBaseKeys(fallbackDictionary);
        foreach (string baseKey in shortcutBaseKeys)
        {
            string descriptionKey = $"{baseKey}{DescriptionSuffix}";
            string gestureKey = $"{baseKey}{GestureSuffix}";

            string? description = GetStringFromDictionary(currentDictionary, descriptionKey) ??
                                  GetStringFromDictionary(fallbackDictionary, descriptionKey);

            string? gesture = GetStringFromDictionary(currentDictionary, gestureKey) ??
                              GetStringFromDictionary(fallbackDictionary, gestureKey);

            if (!string.IsNullOrWhiteSpace(description) && !string.IsNullOrWhiteSpace(gesture))
            {
                shortcutList.Add(new ShortcutItem(description, gesture));
            }
        }

        return shortcutList;
    }
    #endregion Public methods

    #region Private helper methods
    private static ResourceDictionary GetCurrentOrFallbackDictionary()
    {
        string language = Thread.CurrentThread.CurrentUICulture.Name;

        try
        {
            return LoadDictionary(language);
        }
        catch (Exception ex)
        {
            _log.Warn(ex, $"Shortcut dictionary for language \"{language}\" not found. Falling back to {FallbackLanguage}.");
            return LoadDictionary(FallbackLanguage);
        }
    }

    private static ResourceDictionary LoadDictionary(string language)
    {
        ResourceDictionary dictionary = [];
        dictionary.Source = new Uri($"Languages/Strings.{language}.xaml", UriKind.RelativeOrAbsolute);
        return dictionary;
    }

    private static List<string> GetShortcutBaseKeys(ResourceDictionary dictionary)
    {
        List<string> keys = [.. CuratedShortcutBaseKeys];

        // Safety: include any shortcut keys present in resources but not yet curated
        List<string> discoveredKeys = GetDiscoveredShortcutBaseKeys(dictionary);
        foreach (string discoveredKey in discoveredKeys)
        {
            bool exists = keys.Any(x => string.Equals(x, discoveredKey, StringComparison.Ordinal));
            if (!exists)
            {
                keys.Add(discoveredKey);
            }
        }

        return keys;
    }

    private static List<string> GetDiscoveredShortcutBaseKeys(ResourceDictionary dictionary)
    {
        List<string> keys = [];

        foreach (object keyObj in dictionary.Keys)
        {
            string? key = keyObj.ToString();
            if (string.IsNullOrWhiteSpace(key))
            {
                continue;
            }

            if (!key.StartsWith(ShortcutPrefix, StringComparison.Ordinal) ||
                !key.EndsWith(DescriptionSuffix, StringComparison.Ordinal))
            {
                continue;
            }

            string baseKey = key[..^DescriptionSuffix.Length];
            keys.Add(baseKey);
        }

        return keys;
    }

    private static string? GetStringFromDictionary(ResourceDictionary dictionary, string key)
    {
        if (!dictionary.Contains(key))
        {
            return null;
        }

        return dictionary[key]?.ToString();
    }
    #endregion Private helper methods
}

/// <summary>
/// Represents one keyboard shortcut row for display.
/// </summary>
internal sealed class ShortcutItem
{
    #region Constructor
    public ShortcutItem(string description, string shortcut)
    {
        Description = description;
        Shortcut = shortcut;
    }
    #endregion Constructor

    #region Properties
    public string Description { get; }
    public string Shortcut { get; }
    public string GestureText => Shortcut;
    #endregion Properties
}
