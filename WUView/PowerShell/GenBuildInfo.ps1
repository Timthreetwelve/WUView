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

public static class BuildInfo
{
    public static readonly string CommitIDString = ThisAssembly.GitCommitId[..7];

    public static readonly string CommitIDFullString = ThisAssembly.GitCommitId;

    public const string BuildDateString = `"$nowUTC`";

    public static readonly DateTime BuildDateUtc =
        DateTime.SpecifyKind(
            DateTime.ParseExact(BuildDateString, `"yyyy/MM/dd HH:mm:ss`", CultureInfo.InvariantCulture),
            DateTimeKind.Utc);

    public static readonly string BuildDateStringUtc = $`"{BuildDateUtc:f}  (UTC)`";
}"

$outputPath = Join-Path -Path $(Get-Location).Path -ChildPath $outputFile
Set-Content -Path "$outputPath" -Value $class