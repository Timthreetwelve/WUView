param($outputFile="BuildInfo.cs")

$buildDate = Get-Date
$gitOutput = git rev-parse --short HEAD
if ($gitOutput.Length -lt 1 ) {
    $gitOutput = "n/a"
}
$class =
"// Copyright(c) Tim Kennedy. All Rights Reserved. Licensed under the MIT License.
//
// This file is generated during the pre-build event by GenBuildInfo.ps1.
// Any edits to this file will be overwritten during the next build!

using System;

namespace WUView
{
    public static class BuildInfo
    {
        public const string CommitIDString = `"$gitOutput`";

        public const string BuildDateString = `"$buildDate`";

        public static readonly DateTime BuildDateObj = DateTime.Parse(BuildDateString);
    }
}"

Set-Content -Path $outputFile -Value $class

$fullName = Get-Item $outputFile
Write-Host "GenBuildInfo completed. Output written to $fullName"