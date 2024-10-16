// Copyright(c) Tim Kennedy. All Rights Reserved. Licensed under the MIT License.

namespace WUView.Models;

/// <summary>
/// Class for the excluded items
/// </summary>
public class ExcludedItems : ObservableObject
{
    /// <summary>
    /// Gets or sets the excluded string.
    /// </summary>
    /// <value>
    /// The excluded string.
    /// </value>
    public string? ExcludedString { get; init; }

    /// <summary>
    /// Collection of excluded strings
    /// </summary>
    /// <value>
    /// The excluded strings.
    /// </value>
    public static ObservableCollection<ExcludedItems> ExcludedStrings { get; set; } = [];
}
