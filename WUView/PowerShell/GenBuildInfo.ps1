Param(
    [Parameter(Mandatory = $true)] [string] $assemblyName,
    [Parameter(Mandatory = $false)] [string] $outputFile="BuildInfo.cs"
)

$nowUTC = (Get-Date).ToUniversalTime().ToString('yyyy/MM/dd HH:mm:ss')

$class =
"// Copyright (c) Tim Kennedy. All Rights Reserved. Licensed under the MIT License.

// This file is generated during a pre-build event by PowerShell\GenBuildInfo.ps1.
// Any edits to this file will be overwritten during the next build!

namespace $assemblyName;

internal static class BuildInfo
{
    public static readonly string CommitIDString = VersionInfo.GitRevShort;

    public static readonly string CommitIDFullString = VersionInfo.GitRevLong;

    public static readonly string? Prerelease = VersionInfo.VersionPrerelease;

    public const string BuildDateString = `"$nowUTC`";

    public static readonly DateTime BuildDateUtc =
        DateTime.SpecifyKind(
            DateTime.ParseExact(BuildDateString, `"yyyy/MM/dd HH:mm:ss`", CultureInfo.InvariantCulture),
            DateTimeKind.Utc);

    public static readonly string BuildDateStringUtc = $`"{BuildDateUtc:f}  (UTC)`";

    public static readonly string VersionString = Prerelease == null ? VersionInfo.Version : $`"{VersionInfo.Version}-{Prerelease}`";
}"

$outputPath = Join-Path -Path $(Get-Location).Path -ChildPath $outputFile
Set-Content -Path "$outputPath" -Value $class -Encoding utf8BOM