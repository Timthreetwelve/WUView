// Copyright (c) Tim Kennedy. All Rights Reserved. Licensed under the MIT License.

namespace WUView;

/// <summary>
/// Interaction logic for App.xaml
/// </summary>
public partial class App : Application
{
    #region Properties
    /// <summary>
    /// Number of language strings in a resource dictionary
    /// </summary>
    public static int LanguageStrings { get; set; }
    /// <summary>
    /// Uri of the resource dictionary
    /// </summary>
    public static string LanguageFile { get; set; }
    #endregion Properties

    /// <summary>
    /// Override the Startup Event.
    /// </summary>
    protected override void OnStartup(StartupEventArgs e)
    {
        base.OnStartup(e);

        // Only allows a single instance of the application to run.
        SingleInstance.Create(AppInfo.AppName);

        // Initialize settings here so that saved language can be accessed below.
        ConfigHelpers.InitializeSettings();

        // Resource dictionary for language
        ResourceDictionary resDict = new();

        try
        {
            string currentLanguage = Thread.CurrentThread.CurrentCulture.Name;

            // If option to use OS language is true and it exists in the list of defined languages, use it but do not change current culture.
            if (UserSettings.Setting.UseOSLanguage &&
                UILanguage.DefinedLanguages.Exists(x => x.LanguageCode == currentLanguage))
            {
                resDict.Source = new Uri($"Languages/Strings.{currentLanguage}.xaml", UriKind.RelativeOrAbsolute);
            }
            // If a language is defined in settings and it exists in the list of defined languages, set the current culture and language to it.
            else if (!UserSettings.Setting.UseOSLanguage &&
                     !string.IsNullOrEmpty(UserSettings.Setting.UILanguage) &&
                     UILanguage.DefinedLanguages.Exists(x => x.LanguageCode == UserSettings.Setting.UILanguage))
            {
                Thread.CurrentThread.CurrentCulture = new CultureInfo(UserSettings.Setting.UILanguage);
                Thread.CurrentThread.CurrentUICulture = new CultureInfo(UserSettings.Setting.UILanguage);
                resDict.Source = new Uri($"Languages/Strings.{UserSettings.Setting.UILanguage}.xaml", UriKind.RelativeOrAbsolute);
            }
        }
        // If the above fails, set culture and language to en-US.
        catch (Exception)
        {
            Thread.CurrentThread.CurrentCulture = new CultureInfo("en-US");
            Thread.CurrentThread.CurrentUICulture = new CultureInfo("en-US");
            resDict.Source = new Uri("Languages/Strings.en-US.xaml", UriKind.RelativeOrAbsolute);
        }

        Resources.MergedDictionaries.Add(resDict);
        LanguageStrings = resDict.Count;
        LanguageFile = resDict.Source.OriginalString;
    }
}
