// Copyright(c) Tim Kennedy. All Rights Reserved. Licensed under the MIT License.

namespace WUView.Models;

/// <summary>
/// Class for the excluded items
/// </summary>
public class ExcludedItems : ObservableObject
{
    public string ExcludedString { get; set; }
    public static ObservableCollection<ExcludedItems> ExcludedStrings { get; set; } = new();

    //public bool Equals(ExcludedItems other)
    //{
    //    return string.Equals(ExcludedString, other.ExcludedString, StringComparison.OrdinalIgnoreCase);
    //}

    //public override bool Equals(object obj)
    //{
    //    return Equals(obj as ExcludedItems);
    //}

    //public override int GetHashCode()
    //{
    //    return ExcludedStrings.GetHashCode();
    //}
}
