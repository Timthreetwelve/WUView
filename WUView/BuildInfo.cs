// Copyright (c) Tim Kennedy. All Rights Reserved. Licensed under the MIT License.

namespace WUView;

/// <summary>
/// BuildInfo class provides information about the build, including commit ID, commit date, and version string.
/// If the build is a prerelease, the version string will include the prerelease identifier.
/// </summary>
internal static class BuildInfo
{
    public static readonly string CommitIDString = VersionInfo.GitRevShort;

    public static readonly string CommitIDFullString = VersionInfo.GitRevLong;

    public static readonly string? Prerelease = VersionInfo.VersionPrerelease;

    /// <summary>
    /// The UTC date and time of the last commit. Returns <see cref="DateTime.MinValue"/>
    /// if the commit date cannot be parsed.
    /// </summary>
    public static readonly DateTime CommitDateUtc =
        DateTime.TryParse(
            VersionInfo.GitCommitterDate,
            CultureInfo.InvariantCulture,
            DateTimeStyles.AdjustToUniversal,
            out DateTime parsedDate)
        ? parsedDate
        : DateTime.MinValue;

    public static readonly string CommitDateStringUtc = $"{CommitDateUtc:f} (UTC)";
    public static readonly string CommitDateStringLocal = $"{CommitDateUtc.ToLocalTime():f} (Local)";

    public static readonly string VersionString = string.IsNullOrWhiteSpace(Prerelease)
        ? VersionInfo.Version
        : $"{VersionInfo.Version}-{Prerelease}";
}
