// Copyright (c) Tim Kennedy. All Rights Reserved. Licensed under the MIT License.

namespace WUView.Helpers;

internal static class PathHelpers
{
    /// <summary>
    /// Returns a condensed representation of a file system path by keeping the specified number of
    /// leading and trailing path segments and replacing the middle portion with an ellipsis ("...").
    /// </summary>
    /// <param name="path">The full path to condense. Segments are determined by <see cref="Path.DirectorySeparatorChar"/>.</param>
    /// <param name="leading">Number of leading segments to keep (may be zero).</param>
    /// <param name="trailing">Number of trailing segments to keep (may be zero).</param>
    /// <remarks>
    /// Example: input "C:\a\b\c\d\e\f\g", leading=2, trailing=2 => "C:\a\...\f\g".
    /// </remarks>
    /// <returns>
    /// A condensed path string. If the path contains fewer than the sum of <paramref name="leading"/>
    /// and <paramref name="trailing"/> segments the original <paramref name="path"/> is returned.
    /// </returns>
    public static string GetCondensedPath(string path, int leading, int trailing)
    {
        if (string.IsNullOrEmpty(path))
        {
            return path;
        }
        if (leading < 0 || trailing < 0)
        {
            return path;
        }

        string[] parts = path.Split(Path.DirectorySeparatorChar);
        if (parts.Length <= leading + trailing)
        {
            return path;
        }

        StringBuilder sb = new(path.Length);
        for (int i = 0; i < leading; i++)
        {
            sb.Append(parts[i]).Append(Path.DirectorySeparatorChar);
        }

        sb.Append("...").Append(Path.DirectorySeparatorChar);

        for (int i = trailing; i > 0; i--)
        {
            sb.Append(parts[^i]).Append(Path.DirectorySeparatorChar);
        }

        return sb.ToString().TrimEnd(Path.DirectorySeparatorChar);
    }

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
        if (!path.StartsWith(userProfile, StringComparison.CurrentCultureIgnoreCase))
        {
            return path;
        }
        return path.Replace(userProfile, "%USERPROFILE%");
    }
}
