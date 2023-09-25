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
    /// List of defined languages
    /// </summary>
    /// <remarks>
    /// This is a list of languages that the user will be able to change to in the Settings page.
    /// The list should be ordered by LanguageNative.
    /// </remarks>
    public static List<UILanguage> DefinedLanguages { get; set; } = new()
    {
        new UILanguage {Language = "English", LanguageCode = "en-US", LanguageNative = "English (en-US)"},
        new UILanguage {Language = "English", LanguageCode = "en-GB", LanguageNative = "English (en-GB)"},
        new UILanguage {Language = "Spanish", LanguageCode = "es-ES", LanguageNative = "Español (es-ES) - Spanish"},
        new UILanguage {Language = "Italian", LanguageCode = "it-IT", LanguageNative = "Italiano (it-IT) - Italian"},
        new UILanguage {Language = "Dutch",   LanguageCode = "nl-NL", LanguageNative = "Nederlands (nl-NL) - Dutch"},
        new UILanguage {Language = "German",  LanguageCode = "de-DE", LanguageNative = "Deutsch (de-DE) - German"},
    };
}
