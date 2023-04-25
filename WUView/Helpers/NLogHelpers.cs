// Copyright (c) Tim Kennedy. All Rights Reserved. Licensed under the MIT License.

namespace WUView.Helpers;

/// <summary>
/// Class for NLog helper methods
/// </summary>
internal static class NLogHelpers
{
    #region Create the NLog configuration
    /// <summary>
    /// Configure NLog
    /// </summary>
    /// <param name="newfile">True to start with new log file. False to append to current file.</param>
    public static void NLogConfig(bool newfile)
    {
        // Throw exception if there are error in configuration
        LogManager.ThrowConfigExceptions = true;

        // New NLog configuration
        LoggingConfiguration config = new();

        // create log file Target for NLog
        FileTarget logfile = new("logfile")
        {
            // new file on startup
            DeleteOldFileOnStartup = newfile,

            // create the file if needed
            FileName = CreateFilename(),

            // message and footer layouts
            Footer = "${date:format=yyyy/MM/dd HH\\:mm\\:ss}",
            Layout = "${date:format=yyyy/MM/dd HH\\:mm\\:ss} " +
                         "${pad:padding=-5:inner=${level:uppercase=true}}  " +
                         "${message}${onexception:${newline}${exception:format=tostring}}"
        };

        // add the log file target
        config.AddTarget(logfile);

        // add the rule for the log file
        LoggingRule file = new("*", LogLevel.Debug, logfile)
        {
            RuleName = "LogToFile"
        };
        config.LoggingRules.Add(file);

        // create debugger target
        DebuggerTarget debugger = new("debugger")
        {
            Layout = "${processtime} >>> ${message} "
        };

        // add the target
        config.AddTarget(debugger);

        // add the rule
        LoggingRule bug = new("*", LogLevel.Debug, debugger);
        config.LoggingRules.Add(bug);

        // add the configuration to NLog
        LogManager.Configuration = config;

        // Lastly, set the logging level based on setting
        SetLogLevel(UserSettings.Setting.IncludeDebug);
    }
    #endregion Create the NLog configuration

    #region Create a filename in the temp folder
    private static string CreateFilename()
    {
        // create filename string
        string myname = AppInfo.AppName;
        string today = DateTime.Now.ToString("yyyyMMdd");
        string filename;
        if (Debugger.IsAttached)
        {
            filename = $"{myname}.{today}.debug.log";
        }
        else
        {
            filename = $"{myname}.{today}.log";
        }

        // combine temp folder with filename
        string tempdir = Path.GetTempPath();
        return Path.Combine(tempdir, "T_K", filename);
    }
    #endregion Create a filename in the temp folder

    #region Set NLog logging level
    /// <summary>
    /// Set the NLog logging level to Debug or Info
    /// </summary>
    /// <param name="debug">If true set level to Debug, otherwise set to Info</param>
    public static void SetLogLevel(bool debug)
    {
        LoggingConfiguration config = LogManager.Configuration;

        LoggingRule rule = config.FindRuleByName("LogToFile");
        if (rule != null)
        {
            if (debug)
            {
                rule.SetLoggingLevels(LogLevel.Debug, LogLevel.Fatal);
            }
            else
            {
                rule.SetLoggingLevels(LogLevel.Info, LogLevel.Fatal);
            }
            LogManager.ReconfigExistingLoggers();
        }
    }
    #endregion Set NLog logging level

    #region Get the log file name
    /// <summary>
    /// Gets the filename for the NLog log fie
    /// </summary>
    /// <returns></returns>
    public static string GetLogfileName()
    {
        LoggingConfiguration config = LogManager.Configuration;
        return (config.FindTargetByName("logfile")
                as FileTarget)?.FileName.Render(new LogEventInfo { TimeStamp = DateTime.Now });
    }
    #endregion Get the log file name
}
