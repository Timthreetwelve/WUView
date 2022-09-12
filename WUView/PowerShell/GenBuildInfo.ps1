Param(
    [Parameter(Mandatory = $true)] [string] $assemblyName,
    [Parameter(Mandatory = $true)] [string] $assemblyVersion,
    [Parameter(Mandatory = $false)] [string] $outputFile="BuildInfo.cs"
)
Write-Host "GenBuildInfo: Assembly name parameter: $assemblyName"
Write-Host "GenBuildInfo: Assembly version parameter: $assemblyVersion"

$nowUTC = (Get-Date).ToUniversalTime()

$commitID = git rev-parse --short HEAD
if ($commitID.Length -lt 1 ) {
    $commitID = "n/a"
}

$class =
"// Copyright(c) Tim Kennedy. All Rights Reserved. Licensed under the MIT License.
//
// This file is generated during the pre-build event by GenBuildInfo.ps1.
// Any edits to this file will be overwritten during the next build!

using System;

namespace $assemblyName
{
    public static class BuildInfo
    {
        public const string CommitIDString = `"$commitID`";

        public const string BuildDateString = `"$nowUTC`";

        public static readonly DateTime BuildDateUtc = DateTime.SpecifyKind(DateTime.Parse(BuildDateString), DateTimeKind.Utc);
    }
}"

Set-Content -Path $outputFile -Value $class

$fullName = Get-Item $outputFile
Write-Host "GenBuildInfo: Output written to $fullName"
