// Copyright (c) Tim Kennedy. All Rights Reserved. Licensed under the MIT License.

namespace WUView.Models;

/// <summary>
/// Defines properties for Windows Update and Event Log
/// </summary>
public partial class WUpdate : ObservableObject
{
    #region Properties from WUApi

    [ObservableProperty]
    private string _title;

    [ObservableProperty]
    private string _kBNum;

    [ObservableProperty]
    private string _description;

    [ObservableProperty]
    private string _updateID;

    [ObservableProperty]
    private DateTime _date;

    [ObservableProperty]
    private string _resultCode;

    [ObservableProperty]
    private string _hResult;

    [ObservableProperty]
    private string _operation;

    [ObservableProperty]
    private string _supportURL;
    #endregion Properties from WUApi

    #region Properties from Event Log
    [ObservableProperty]
    private int _eLEventID;

    [ObservableProperty]
    private string _eLDescription;

    [ObservableProperty]
    private string _eLProvider;

    [ObservableProperty]
    private DateTime? _eLDate;
    #endregion Properties from Event Log

    #region GetClone Method
    internal WUpdate GetClone()
    {
        return (WUpdate)MemberwiseClone();
    }
    #endregion GetClone Method
}
