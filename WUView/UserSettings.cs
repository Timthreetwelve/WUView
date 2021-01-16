// Copyright(c) Tim Kennedy. All Rights Reserved. Licensed under the MIT License.

#region Using directives
using System.ComponentModel;
using System.Runtime.CompilerServices;
using TKUtils;
#endregion Using directives

namespace WUView
{
    public class UserSettings : SettingsManager<UserSettings>, INotifyPropertyChanged
    {
        #region Constructor
        public UserSettings()
        {
            // Set defaults
            DetailsBackground = "#FFFFF8DC";
            DetailsHeight = 250;
            FontFamily = "Segoe UI";
            GridZoom = 1;
            HideExcluded = true;
            ResultCodeUrl = "https://docs.microsoft.com/en-us/windows/deployment/update/windows-update-error-reference";
            ShadeAltRows = true;
            ShowDetails = true;
            ShowGridLines = true;
            VerboseLogging = true;
            WindowHeight = 600;
            WindowLeft = 100;
            WindowTop = 100;
            WindowWidth = 900;
        }
        #endregion Constructor

        #region Properties
        public string DetailsBackground
        {
            get { return detailsBackground; }
            set
            {
                detailsBackground = value;
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

        public string FontFamily
        {
            get { return fontFamily; }
            set
            {
                fontFamily = value;
                OnPropertyChanged();
            }
        }

        public double GridZoom
        {
            get
            {
                if (gridZoom <= 0)
                {
                    gridZoom = 1;
                }
                return gridZoom;
            }
            set
            {
                gridZoom = value;
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

        public string ResultCodeUrl
        {
            get { return resultCodeUrl; }
            set
            {
                resultCodeUrl = value;
                OnPropertyChanged();
            }
        }

        public bool ShadeAltRows
        {
            get => shadeAltRows;
            set
            {
                shadeAltRows = value;
                OnPropertyChanged();
            }
        }

        public bool ShowGridLines
        {
            get { return showGridLines; }
            set
            {
                showGridLines = value;
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

        public bool VerboseLogging
        {
            get { return verboseLogging; }
            set
            {
                verboseLogging = value;
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
                if (windowLeft < 0)
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
                if (windowTop < 0)
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
        private string detailsBackground;
        private double detailsHeight;
        private string fontFamily;
        private double gridZoom;
        private bool hideExcluded;
        private string resultCodeUrl;
        private bool shadeAltRows;
        private bool showDetails;
        private bool showGridLines;
        private bool verboseLogging;
        private double windowHeight;
        private double windowLeft;
        private double windowTop;
        private double windowWidth;
        #endregion Private backing fields

        #region Handle property change event
        public event PropertyChangedEventHandler PropertyChanged;

        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
        #endregion Handle property change event
    }
}
