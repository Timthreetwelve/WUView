// Copyright (c) Tim Kennedy. All Rights Reserved. Licensed under the MIT License.

namespace WUView.Helpers;

internal static class ResourceHelpers
{
    #region Composite format properties
    internal static CompositeFormat MsgTextAppUpdateNewerFound { get; } = GetCompositeResource("MsgText_AppUpdateNewerFound");
    internal static CompositeFormat MsgTextDisplayedAllUpdates { get; } = GetCompositeResource("MsgText_DisplayedAllUpdates");
    internal static CompositeFormat MsgTextDisplayedUpdates { get; } = GetCompositeResource("MsgText_DisplayedUpdates");
    internal static CompositeFormat MsgTextErrorOpeningFile { get; } = GetCompositeResource("MsgText_ErrorOpeningFile");
    internal static CompositeFormat MsgTextErrorReadingFile { get; } = GetCompositeResource("MsgText_ErrorReadingFile");
    internal static CompositeFormat MsgTextEventLogNoRecords { get; } = GetCompositeResource("MsgText_EventLogNoRecords");
    internal static CompositeFormat MsgTextFilterRowsShown { get; } = GetCompositeResource("MsgText_FilterRowsShown");
    internal static CompositeFormat MsgTextHResultCopiedToClipboard { get; } = GetCompositeResource("MsgText_HResultCopiedToClipboard");
    internal static CompositeFormat MsgTextUIColorSet { get; } = GetCompositeResource("MsgText_UIColorSet");
    internal static CompositeFormat MsgTextUISizeSet { get; } = GetCompositeResource("MsgText_UISizeSet");
    internal static CompositeFormat MsgTextUIThemeSet { get; } = GetCompositeResource("MsgText_UIThemeSet");
    #endregion Composite format properties

    #region Get a string resource
    /// <summary>
    /// Gets the string resource for the key.
    /// </summary>
    /// <param name="key">The key.</param>
    /// <returns>String</returns>
    /// <remarks>
    /// Want to throw here so that missing resource doesn't make it into a release.
    /// </remarks>
    public static string GetStringResource(string key)
    {
        object description;
        try
        {
            description = Application.Current.TryFindResource(key);
        }
        catch (Exception ex)
        {
            if (Debugger.IsAttached)
            {
                throw new ArgumentException($"Resource not found: {key}");
            }
            else
            {
                _log.Error(ex, $"Resource not found: {key}");
                return $"Resource not found: {key}";
            }
        }

        if (description is null)
        {
            if (Debugger.IsAttached)
            {
                throw new ArgumentNullException($"Resource not found: {key}");
            }
            else
            {
                _log.Error($"Resource not found: {key}");
                return $"Resource not found: {key}";
            }
        }

        return description.ToString()!;
    }
    #endregion Get a string resource

    #region Get composite format for a resource string
    private static CompositeFormat GetCompositeResource(string key)
    {
        return CompositeFormat.Parse(GetStringResource(key));
    }
    #endregion Get composite format for a resource string

    #region Compute percentage of language strings
    /// <summary>
    /// Compute percentage of strings by dividing the number of strings
    /// for the supplied language by the total of en-US strings.
    /// </summary>
    /// <param name="language">Language code</param>
    /// <returns>The percentage with no decimal places as a string. Includes the "%".</returns>
    public static string GetLanguagePercent(string language)
    {
        ResourceDictionary dictionary = new()
        {
            Source = new Uri($"Languages/Strings.{language}.xaml", UriKind.RelativeOrAbsolute)
        };
        double percent = (double)dictionary.Count / TotalCount;
        return percent.ToString("P0", CultureInfo.InvariantCulture);
    }
    #endregion Compute percentage of language strings

    #region Properties
    private static int _totalCount;
    private static int TotalCount
    {
        get
        {
            if (_totalCount == 0)
            {
                _totalCount = GetTotalDefaultLanguageCount();
            }
            return _totalCount;
        }
    }
    #endregion Properties

    #region Count default (en-US) strings
    /// <summary>
    /// Gets the count of strings in the default resource dictionary.
    /// </summary>
    /// <returns>Count as int.</returns>
    public static int GetTotalDefaultLanguageCount()
    {
        ResourceDictionary dictionary = new()
        {
            Source = new Uri("Languages/Strings.en-US.xaml", UriKind.RelativeOrAbsolute)
        };
        return dictionary.Count;
    }
    #endregion Count default (en-US) strings

    #region Compare language dictionaries
    /// <summary>
    /// Compares language resource dictionaries to find missing keys
    /// </summary>
    public static void CompareLanguageDictionaries()
    {
        try
        {
            string currentLanguage = Thread.CurrentThread.CurrentCulture.Name;
            string compareLang = $"Languages/Strings.{currentLanguage}.xaml";

            ResourceDictionary dict1 = [];
            ResourceDictionary dict2 = [];

            dict1.Source = new Uri("Languages/Strings.en-US.xaml", UriKind.RelativeOrAbsolute);
            dict2.Source = new Uri(compareLang, UriKind.RelativeOrAbsolute);
            _log.Info($"Comparing keys in {dict1.Source} and {dict2.Source}");

            Dictionary<string, string> enUSDict = [];
            Dictionary<string, string> compareDict = [];

            foreach (DictionaryEntry kvp in dict1)
            {
                enUSDict.Add(kvp.Key.ToString()!, kvp.Value!.ToString()!);
            }
            foreach (DictionaryEntry kvp in dict2)
            {
                compareDict.Add(kvp.Key.ToString()!, kvp.Value!.ToString()!);
            }

            bool same = enUSDict.Count == compareDict.Count && enUSDict.Keys.SequenceEqual(compareDict.Keys);

            if (same)
            {
                _log.Info($"{dict1.Source} and {dict2.Source} have the same keys.");
            }
            else if (enUSDict.Count == compareDict.Count)
            {
                SortedDictionary<string, string> orderedUSDict = new(enUSDict);
                SortedDictionary<string, string> orderedCompareDict = new(compareDict);

                if (orderedUSDict.Keys.SequenceEqual(orderedCompareDict.Keys))
                {
                    _log.Info($"{dict1.Source} and {dict2.Source} have the same keys, however the order differs.");
                }
                else
                {
                    CompareDictionaryKeys(dict1, dict2, enUSDict, compareDict);
                }
            }
            else
            {
                CompareDictionaryKeys(dict1, dict2, enUSDict, compareDict);
            }
        }
        catch (Exception ex)
        {
            _log.Error(ex, "Error in CompareLanguageDictionaries");
        }
    }
    #endregion Compare language dictionaries

    #region Compare keys
    private static void CompareDictionaryKeys(ResourceDictionary dict1,
                                  ResourceDictionary dict2,
                                  Dictionary<string, string> enUSDict,
                                  Dictionary<string, string> compareDict)
    {
        Dictionary<string, string> missingKeysDict = [];
        Dictionary<string, string> unknownKeysDict = [];

        if (enUSDict.Keys.Except(compareDict.Keys).Any())
        {
            string dashes = new('-', 35);
            string header = $"{dashes} Begin Missing Keys {dashes}";
            _log.Warn(header);
            _log.Warn($"[{AppInfo.AppName}] {dict2.Source} is missing the following keys:");
            foreach (string item in enUSDict.Keys.Except(compareDict.Keys).Order())
            {
                missingKeysDict.Add(item, GetStringResource(item));
            }
            WriteDictToLog(missingKeysDict);
            _log.Warn(new string('-', 91));
        }

        if (compareDict.Keys.Except(enUSDict.Keys).Any())
        {
            string dashes = new('-', 35);
            string header = $"{dashes} Begin Unneeded Keys {dashes}";
            _log.Warn(header);
            _log.Warn($"[{AppInfo.AppName}] {dict2.Source} has keys that {dict1.Source} does not have.");
            foreach (string item in compareDict.Keys.Except(enUSDict.Keys).Order())
            {
                unknownKeysDict.Add(item, GetStringResource(item));
            }
            WriteDictToLog(unknownKeysDict);
            _log.Warn(new string('-', 91));
        }
    }
    #endregion Compare keys

    #region Write missing and unneeded keys to the log file
    private static void WriteDictToLog(Dictionary<string, string>? dict)
    {
        if (dict?.Count > 0)
        {
            int maxMissing = dict.Max(s => s.Key.Length);
            foreach (string key in dict.Keys)
            {
                _log.Warn($"Key: {key.PadRight(maxMissing)}  en-US Value: \"{GetStringResource(key)}\"");
            }
        }
    }
    #endregion Write missing and unneeded keys to the log file
}
