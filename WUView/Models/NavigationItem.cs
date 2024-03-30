// Copyright (c) Tim Kennedy. All Rights Reserved. Licensed under the MIT License.

namespace WUView.Models;

/// <summary>
/// Class defining properties of a Navigation Item
/// </summary>
public partial class NavigationItem : ObservableObject
{
    [ObservableProperty]
    private PackIconKind _iconKind = PackIconKind.QuestionMark;

    [ObservableProperty]
    private bool _isExit;

    [ObservableProperty]
    private string? _name;

    [ObservableProperty]
    private NavPage _navPage;

    [ObservableProperty]
    private string _pageTitle = "Page Title Goes Here";

    [ObservableProperty]
    private object? _viewModelType;
}
