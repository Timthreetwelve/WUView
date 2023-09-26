// Copyright (c) Tim Kennedy. All Rights Reserved. Licensed under the MIT License.

namespace WUView.Models
{
    /// <summary>
    /// Class to describe contributors to language translations.
    /// </summary>
    public class TranslationContributors
    {
        public string Contributor { get; set; }

        public string Language { get; set; }

        public string LanguageCode { get; set; }

        public string Note { get; set; } = string.Empty;

        /// <summary>
        /// Add language contributions to the bottom. List should appear in the order that they were added.
        /// Note property is not currently being used but could be used for percent complete, etc.
        /// </summary>
        public static List<TranslationContributors> Contributors { get; set; } = new()
        {
            new TranslationContributors {Language = "English", LanguageCode = "en-US", Contributor = "@Timthreetwelve", Note = "Default" },
            new TranslationContributors {Language = "English", LanguageCode = "en-GB", Contributor = "@Timthreetwelve"},
            new TranslationContributors {Language = "Spanish", LanguageCode = "es-ES", Contributor = "My AWESOME brother Steve"},
            new TranslationContributors {Language = "Dutch",   LanguageCode = "nl-NL", Contributor = "Tim" },
            new TranslationContributors {Language = "Italian", LanguageCode = "it-IT", Contributor = "RB" },
            new TranslationContributors {Language = "German",  LanguageCode = "de-DE", Contributor = "@Timthreetwelve - using Google translate" },
            new TranslationContributors {Language = "French",  LanguageCode = "fr-FR", Contributor = "@Timthreetwelve - using Google translate" },
            new TranslationContributors {Language = "Catalan", LanguageCode = "ca-ES", Contributor = "@Timthreetwelve - using Google translate" },
        };
    }
}
