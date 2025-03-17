// Copyright (c) Tim Kennedy. All Rights Reserved. Licensed under the MIT License.

namespace WUView;

/// <summary>
/// Interaction logic for App.xaml
/// </summary>
public partial class App : Application
{
    #region Properties
    /// <summary>
    /// Number of language strings in the test resource dictionary
    /// </summary>
    private static int TestLanguageStrings { get; set; }

    /// <summary>
    /// Uri of the test resource dictionary
    /// </summary>
    private static string? TestLanguageFile { get; set; }

    /// <summary>
    /// Number of language strings in the default resource dictionary
    /// </summary>
    public static int DefaultLanguageStrings { get; private set; }
    #endregion Properties

    #region On startup event
    /// <summary>
    /// Override the Startup Event.
    /// </summary>
    protected override void OnStartup(StartupEventArgs e)
    {
        base.OnStartup(e);

        // Unhandled exception handler
        AppDomain.CurrentDomain.UnhandledException += CurrentDomain_UnhandledException;

        // Only allows a single instance of the application to run.
        SingleInstance.Create(AppInfo.AppName);

        // Initialize settings here so that saved language can be accessed below.
        ConfigHelpers.InitializeSettings();

        // Log startup messages
        MainWindowHelpers.LogStartup();

        // Set the UI language
        SetLanguage();

        // Enable language testing if requested.
        CheckLanguageTesting();
    }
    #endregion On startup event

    #region Set the UI language
    /// <summary>
    /// Set the UI language.
    /// </summary>
    /// <remarks>
    /// Strings.en-US.xaml is loaded in App.xaml as the fallback language.
    /// Consequently there is no need to explicitly load it in case of an error.
    /// </remarks>
    private void SetLanguage()
    {
        // Get the number of strings in the default language file
        DefaultLanguageStrings = GetTotalDefaultLanguageCount();

        // Resource dictionary for language
        ResourceDictionary LanguageDictionary = [];

        // Log culture info at startup
        _log.Debug($"Startup culture: {LocalizationHelpers.GetCurrentCulture()}  UI: {LocalizationHelpers.GetCurrentUICulture()}");

        // Get the current UI language
        string currentLanguage = Thread.CurrentThread.CurrentUICulture.Name;

        // Check the UseOSLanguage setting. If true try to use the language. Do not change current culture. 
        if (LocalizationHelpers.CheckUseOsLanguage(currentLanguage))
        {
            if (currentLanguage == "en-US")
            {
                LocalizationHelpers.LanguageStrings = DefaultLanguageStrings;
                _log.Debug("Use OS Language option is true. Language is en-US. No need to load language file.");
                return;
            }
            try
            {
                LanguageDictionary.Source = new Uri($"Languages/Strings.{currentLanguage}.xaml", UriKind.RelativeOrAbsolute);
                Resources.MergedDictionaries.Add(LanguageDictionary);
                _log.Debug($"Use OS Language option is true. Language {currentLanguage} loaded.");
            }
            catch (Exception ex)
            {
                LanguageDictionary.Source = new Uri("Languages/Strings.en-US.xaml", UriKind.RelativeOrAbsolute);
                _log.Warn(ex, $"Language {currentLanguage} could not be located. Defaulting to en-US");
            }
            LocalizationHelpers.ApplyLanguageSettings(LanguageDictionary);
            return;
        }

        // If a language is defined in settings, and it exists in the list of defined languages, set the current culture and language to it.
        if (!string.IsNullOrEmpty(UserSettings.Setting!.UILanguage) &&
            UILanguage.DefinedLanguages.Exists(x => x.LanguageCode == UserSettings.Setting.UILanguage))
        {
            try
            {
                LanguageDictionary.Source = new Uri($"Languages/Strings.{UserSettings.Setting.UILanguage}.xaml", UriKind.RelativeOrAbsolute);
                Thread.CurrentThread.CurrentCulture = new CultureInfo(UserSettings.Setting.UILanguage);
                Thread.CurrentThread.CurrentUICulture = new CultureInfo(UserSettings.Setting.UILanguage);
                Resources.MergedDictionaries.Add(LanguageDictionary);
            }
            catch (Exception ex)
            {
                LanguageDictionary.Source = new Uri("Languages/Strings.en-US.xaml", UriKind.RelativeOrAbsolute);
                _log.Warn(ex, $"Error using language \"{UserSettings.Setting.UILanguage}\". Defaulting to en-US");
            }
            LocalizationHelpers.ApplyLanguageSettings(LanguageDictionary);
            return;
        }

        // If language is not found in settings, or the language is not defined in UILanguage.DefinedLanguages, use en-US.
        // Strings.en-US.xaml is loaded in App.xaml therefore there is no need to explicitly load it here.
        LanguageDictionary.Source = new Uri("Languages/Strings.en-US.xaml", UriKind.RelativeOrAbsolute);
        UserSettings.Setting.UILanguage = "en-US";
        ConfigHelpers.SaveSettings();
        _log.Warn("Language defaulting to en-US");
        LocalizationHelpers.ApplyLanguageSettings(LanguageDictionary);
    }
    #endregion Set the UI language

    #region Language testing
    private void CheckLanguageTesting()
    {
        if (UserSettings.Setting!.LanguageTesting)
        {
            _log.Info("Language testing enabled");
            ResourceDictionary testDict = [];
            string testLanguageFile = Path.Combine(AppInfo.AppDirectory, "Strings.test.xaml");
            if (File.Exists(testLanguageFile))
            {
                try
                {
                    testDict.Source = new Uri(testLanguageFile, UriKind.RelativeOrAbsolute);
                    if (testDict.Source != null)
                    {
                        Resources.MergedDictionaries.Add(testDict);
                        TestLanguageStrings = testDict.Count;
                        TestLanguageFile = testDict.Source.OriginalString;
                        _log.Debug($"{TestLanguageStrings} strings loaded from {TestLanguageFile}");
                    }
                }
                catch (Exception ex)
                {
                    _log.Error(ex, $"Error loading test language file {TestLanguageFile}");
                    string msg = string.Format(CultureInfo.CurrentCulture,
                                               $"{GetStringResource("MsgText_Error_TestLanguage")}\n\n{ex.Message}\n\n{ex.InnerException}");
                    _ = MessageBox.Show(msg,
                        GetStringResource("MsgText_ErrorCaption"),
                        MessageBoxButton.OK,
                        MessageBoxImage.Error);
                }
            }
            else
            {
                _log.Error($"Error loading test language file {TestLanguageFile}. File not found.");
            }
        }
    }
    #endregion Language testing

    #region Unhandled Exception Handler
    /// <summary>
    /// Handles any exceptions that weren't caught by a try-catch statement.
    /// </summary>
    /// <remarks>
    /// This uses default message box.
    /// </remarks>
    private static void CurrentDomain_UnhandledException(object sender, UnhandledExceptionEventArgs args)
    {
        _log.Error("Unhandled Exception");
        Exception e = (Exception)args.ExceptionObject;
        _log.Error(e.Message);
        if (e.InnerException != null)
        {
            _log.Error(e.InnerException.ToString());
        }
        _log.Error(e.StackTrace);

        _ = MessageBox.Show($"An error has occurred.\n{e.Message}\n\nSee the log file. ",
            GetStringResource("MsgText_ErrorCaption"),
            MessageBoxButton.OK,
            MessageBoxImage.Error);
    }
    #endregion Unhandled Exception Handler
}
