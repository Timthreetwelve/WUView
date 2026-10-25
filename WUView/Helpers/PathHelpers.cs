// Copyright (c) Tim Kennedy. All Rights Reserved. Licensed under the MIT License.

namespace WUView.Helpers;

internal static class PathHelpers
{
    /// <summary>
    /// If a path to a file includes the user profile name replace it with %USERPROFILE%.
    /// </summary>
    /// <remarks>
    /// Users may not want to have their user names visible in the log file, especially when sending that log with a bug
    /// report. This method accomplishes that while still keeping the logged path usable.
    /// </remarks>
    /// <returns>
    /// A string representing the path.
    /// </returns>
    public static string AnonymizePath(string path)
    {
        if (string.IsNullOrEmpty(path))
        {
            return string.Empty;
        }

        string userProfile = Environment.GetFolderPath(Environment.SpecialFolder.UserProfile);
        if (!path.StartsWith(userProfile, StringComparison.OrdinalIgnoreCase))
        {
            return path;
        }
        int profileLength = userProfile.Length;
        if (profileLength < path.Length &&
            path[profileLength] != Path.DirectorySeparatorChar &&
            path[profileLength] != Path.AltDirectorySeparatorChar)
        {
            return path;
        }
        return "%USERPROFILE%" + path[profileLength..];
    }

    #region Search PATH for a file
    /// <summary>
    /// Find a file name in the path and optionally, the current directory.
    /// </summary>
    /// <param name="filename">File name to search for</param>
    /// <param name="includeCurrentDirectory">Whether to include the current directory in the search</param>
    /// <returns>Path to the file if found; otherwise, an empty string</returns>
    /// <remarks>
    /// This method searches for the specified file in the directories listed in the PATH environment variable.
    /// If includeCurrentDirectory is true, it also searches in the current directory.
    /// There are potential security risks associated with including the current directory in the search, as it
    /// may lead to executing unintended files. Use this option with caution.
    /// </remarks>
    public static string FindOnPath(string filename, bool includeCurrentDirectory = false)
    {
        if (string.IsNullOrEmpty(filename) || filename.IndexOfAny(Path.GetInvalidFileNameChars()) >= 0)
        {
            throw new ArgumentException("Filename cannot be null, empty, or contain invalid characters.", nameof(filename));
        }
        if (Path.GetExtension(filename) == string.Empty)
        {
            throw new ArgumentException("Filename must have an extension.", nameof(filename));
        }
        if (Path.IsPathRooted(filename))
        {
            throw new ArgumentException("Filename must not be a rooted path.", nameof(filename));
        }
        string pathVar = Environment.GetEnvironmentVariable("PATH") ?? string.Empty;
        string[] folders = pathVar.Split(Path.PathSeparator, StringSplitOptions.RemoveEmptyEntries);

        if (includeCurrentDirectory)
        {
            folders = [Environment.CurrentDirectory, .. folders];
        }

        foreach (string folder in folders)
        {
            string path = Path.Combine(folder.Trim('\"'), filename);
            if (File.Exists(path))
            {
                return path;
            }
        }

        return string.Empty;
    }
    #endregion Search PATH for a file
}
