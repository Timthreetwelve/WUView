// Copyright(c) Tim Kennedy. All Rights Reserved. Licensed under the MIT License.

namespace WUView;

/// <summary>
/// Class for persisting various application settings determined by the user
/// </summary>
public class UserSettings : SettingsManager<UserSettings>, INotifyPropertyChanged
{
    #region Properties
    public int DarkMode
    {
        get => darkmode;
        set
        {
            darkmode = value;
            OnPropertyChanged();
        }
    }

    public double DetailsHeight
    {
        get { return detailsHeight; }
        set
        {
            detailsHeight = value;
            OnPropertyChanged();
        }
    }

    public int GridFontWeight
    {
        get => gridFontWeight;
        set
        {
            gridFontWeight = value;
            OnPropertyChanged();
        }
    }

    public bool HideExcluded
    {
        get { return hideExcluded; }
        set
        {
            hideExcluded = value;
            OnPropertyChanged();
        }
    }

    public bool IncludeDebug
    {
        get => includeDebug;
        set
        {
            includeDebug = value;
            OnPropertyChanged();
        }
    }

    public bool KeepOnTop
    {
        get => keepOnTop;
        set
        {
            keepOnTop = value;
            OnPropertyChanged();
        }
    }

    public bool NewLog
    {
        get => newLog;
        set
        {
            newLog = value;
            OnPropertyChanged();
        }
    }

    public int PrimaryColor
    {
        get => primaryColor;
        set
        {
            primaryColor = value;
            OnPropertyChanged();
        }
    }

    public string ResultCodeUrl
    {
        get { return resultCodeUrl; }
        set
        {
            resultCodeUrl = value;
            OnPropertyChanged();
        }
    }

    public int RowSpacing
    {
        get => rowSpacing;
        set
        {
            rowSpacing = value;
            OnPropertyChanged();
        }
    }

    public bool ShowDetails
    {
        get { return showDetails; }
        set
        {
            showDetails = value;
            OnPropertyChanged();
        }
    }

    public int UISize
    {
        get => uiSize;
        set
        {
            uiSize = value;
            OnPropertyChanged();
        }
    }

    public double WindowHeight
    {
        get
        {
            if (windowHeight < 100)
            {
                windowHeight = 100;
            }
            return windowHeight;
        }
        set => windowHeight = value;
    }

    public double WindowLeft
    {
        get
        {
            if (windowLeft < 0 || windowLeft >= SystemParameters.VirtualScreenWidth)
            {
                windowLeft = 0;
            }
            return windowLeft;
        }
        set => windowLeft = value;
    }

    public double WindowTop
    {
        get
        {
            if (windowTop < 0 || windowTop >= SystemParameters.VirtualScreenHeight)
            {
                windowTop = 0;
            }
            return windowTop;
        }
        set => windowTop = value;
    }

    public double WindowWidth
    {
        get
        {
            if (windowWidth < 100)
            {
                windowWidth = 100;
            }
            return windowWidth;
        }
        set => windowWidth = value;
    }
    #endregion Properties

    #region Private backing fields
    private int darkmode = (int)ThemeType.Light;
    private double detailsHeight = 250;
    private int gridFontWeight = (int)Weight.Regular;
    private bool hideExcluded = true;
    private bool includeDebug = true;
    private bool keepOnTop = false;
    private bool newLog = true;
    private int primaryColor = (int)AccentColor.Blue;
    private string resultCodeUrl = "https://docs.microsoft.com/en-us/windows/deployment/update/windows-update-error-reference";
    private int rowSpacing = (int)Spacing.Comfortable;
    private bool showDetails = true;
    private int uiSize = (int)MySize.Default;
    private double windowHeight = 500;
    private double windowLeft = 100;
    private double windowTop = 100;
    private double windowWidth = 300;
    #endregion Private backing fields

    #region Property change event
    public event PropertyChangedEventHandler PropertyChanged;

    protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
    {
        PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
    }
    #endregion Property change event
}
