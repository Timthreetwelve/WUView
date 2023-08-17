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
    /// </remarks>
    public static List<UILanguage> DefinedLanguages { get; set; } = new()
    {
        new UILanguage {Language = "English  (en-US)", LanguageCode = "en-US"},
        new UILanguage {Language = "English  (en-GB)", LanguageCode = "en-GB"},
        new UILanguage {Language = "Spanish  (es-ES)", LanguageCode = "es-ES"},
    };
}
