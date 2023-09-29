// Copyright (c) Tim Kennedy. All Rights Reserved. Licensed under the MIT License.

namespace WUView.Constants;

/// <summary>
/// Class for constant Uris
/// </summary>
internal static class AppConstUri
{
    /// <summary>
    /// Gets the HResult URL.
    /// </summary>
    /// <value>
    /// The HResult URL as Uri.
    /// </value>
    public static Uri HResultCodeUrl { get; } = new("https://docs.microsoft.com/en-us/windows/deployment/update/windows-update-error-reference");

    /// <summary>
    /// Gets the Result code URL.
    /// </summary>
    /// <value>
    /// The result code URL as Uri.
    /// </value>
    public static Uri ResultCodeUrl { get; } = new("https://github.com/Timthreetwelve/WUView/wiki/Result-Codes");
}
