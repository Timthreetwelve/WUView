// Copyright (c) Tim Kennedy. All Rights Reserved. Licensed under the MIT License.

namespace WUView.Models;

/// <summary>
/// Class for language properties.
/// </summary>
/// <seealso cref="CommunityToolkit.Mvvm.ComponentModel.ObservableObject" />
internal partial class UILanguage : ObservableObject
{
    [ObservableProperty]
    private string _language;

    [ObservableProperty]
    private string _languageCode;

    [ObservableProperty]
    private string _languageNative;

    /// <summary>
    /// Overrides the ToString method.
    /// </summary>
    /// <returns>
    /// The language code as a string.
    /// </returns>
    public override string ToString()
    {
        return LanguageCode;
    }

    /// <summary>
    /// List of languages with language code
    /// </summary>
    private static List<UILanguage> LanguageList { get; } = new()
    {
        new UILanguage {Language = "English", LanguageCode = "en-US", LanguageNative = "English (en-US)"},
        new UILanguage {Language = "English", LanguageCode = "en-GB", LanguageNative = "English (en-GB)"},
        new UILanguage {Language = "Spanish", LanguageCode = "es-ES", LanguageNative = "Español (es-ES) - Spanish"},
        new UILanguage {Language = "Italian", LanguageCode = "it-IT", LanguageNative = "Italiano (it-IT) - Italian"},
        new UILanguage {Language = "Dutch",   LanguageCode = "nl-NL", LanguageNative = "Nederlands (nl-NL) - Dutch"},
        new UILanguage {Language = "German",  LanguageCode = "de-DE", LanguageNative = "Deutsch (de-DE) - German"},
        new UILanguage {Language = "French",  LanguageCode = "fr-FR", LanguageNative = "Français (fr-FR) - French"},
    };

    /// <summary>
    /// List of defined languages ordered by LanguageNative.
    /// </summary>
    public static List<UILanguage> DefinedLanguages => LanguageList.OrderBy(x => x.LanguageNative).ToList();

}
