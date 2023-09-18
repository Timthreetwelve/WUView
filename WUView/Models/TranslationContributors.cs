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

        public string Note { get; set; } = string.Empty;

        public static List<TranslationContributors> Contributors { get; set; } = new()
        {
            new TranslationContributors {Language = "English (en-US)", Contributor = "@Timthreetwelve", Note = "Default" },
            new TranslationContributors {Language = "English (en-GB)", Contributor = "@Timthreetwelve"},
            new TranslationContributors {Language = "Español (es-ES)", Contributor = "My AWESOME brother Steve"}
        };
    }
}
