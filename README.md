<p align="center">
  <a target="_blank" rel="noopener noreferrer">
    <img width="128" src="https://github.com/Timthreetwelve/WUView/blob/main/WUView/Images/UV.png" alt="Windows Updaet Viewer Logo">
  </a>
</p>
<h1 align="center">
  Windows Update Viewer
</h1>

<div align="center">
  
[![GitHub](https://img.shields.io/github/license/Timthreetwelve/WUView?style=plastic&color=seagreen)](https://github.com/Timthreetwelve/WUView/blob/main/LICENSE)
[![NET6win](https://img.shields.io/badge/.NET-8.0--Windows-blueviolet?style=plastic)](https://dotnet.microsoft.com/en-us/download) 
[![GitHub release (latest by date)](https://img.shields.io/github/v/release/Timthreetwelve/WUView?style=plastic)](https://github.com/Timthreetwelve/WUView/releases/latest) 
[![GitHub Release Date](https://img.shields.io/github/release-date/timthreetwelve/WUView?style=plastic&color=orange)](https://github.com/Timthreetwelve/WUView/releases/latest) 
[![GitHub commits since latest release (by date)](https://img.shields.io/github/commits-since/timthreetwelve/WUView/latest?style=plastic)](https://github.com/Timthreetwelve/WUView/commits/main)
[![GitHub last commit](https://img.shields.io/github/last-commit/timthreetwelve/WUView?style=plastic)](https://github.com/Timthreetwelve/WUView/commits/main)
[![GitHub commits](https://img.shields.io/github/commit-activity/m/timthreetwelve/WUView?style=plastic)](https://github.com/Timthreetwelve/WUView/commits/main)
[![GitHub Stars](https://img.shields.io/github/stars/timthreetwelve/wuview?style=plastic&color=goldenrod&logo=github)](https://docs.github.com/en/get-started/exploring-projects-on-github/saving-repositories-with-stars)
[![GitHub all releases](https://img.shields.io/github/downloads/Timthreetwelve/WUView/total?style=plastic&label=total%20downloads&color=teal)](https://github.com/Timthreetwelve/WUView/releases) 
[![GitHub release (by tag)](https://img.shields.io/github/downloads/timthreetwelve/wuview/latest/total?style=plastic&color=2196F3&label=downloads%20latest%20version)](https://github.com/Timthreetwelve/WUView/releases/latest)
[![GitHub Issues](https://img.shields.io/github/issues/timthreetwelve/wuview?style=plastic&color=orangered)](https://github.com/Timthreetwelve/WUView/issues)
[![GitHub Issues](https://img.shields.io/github/issues-closed/timthreetwelve/wuview?style=plastic&color=slateblue)](https://github.com/Timthreetwelve/WUView/issues)

</div>

#### Windows Update Viewer uses .NET 8
Self-contained versions are available if .NET 8 isn't installed. See the [releases page](https://github.com/Timthreetwelve/WUView/releases).

Windows Update Viewer (WUView) is an application that displays Windows Update history. It is meant to be a lightweight application that is easy to use. There aren't any confusing categories; every update is listed in one place. Updates that you don't want to see can be permanently excluded or temporarily filtered.

WUView uses the Windows Update Agent API and Windows event logs to display details of installed updates. Event log entries are associated with individual updates by using the "KB" number. If an update does not use a KB number or isn't presented in a consistent format, no event log entries will be displayed.

Please be aware that Windows Update Viewer can only display updates provided by the [Windows Update Agent API](https://learn.microsoft.com/en-us/windows/win32/wua_sdk/portal-client). If you have reason to suspect that the application isn't returning correct or complete information, see the [Troubleshooting](https://github.com/Timthreetwelve/WUView/wiki/Troubleshooting) and [Known Issues](https://github.com/Timthreetwelve/WUView/wiki/Known-Issues) topics in the Wiki.

If you are using Windows 10 please see [Known Issues](https://github.com/Timthreetwelve/WUView/wiki/Known-Issues). 

See the [Wiki](https://github.com/Timthreetwelve/WUView/wiki) for additional information.

### Windows Update Viewer is multilingual!
Languages are being added as of version 0.5.21. Please see [Contribute a Translation](https://github.com/Timthreetwelve/WUView/wiki/Contribute-a-Translation) topic in the Wiki if you would like to contribute a translation. 

### Download Windows Update Viewer
You can always download the latest release from the [releases page](https://github.com/Timthreetwelve/WUView/releases). Note that "portable" releases are provided as well as the traditional installers.

### Features
* View details for each update, including Event Log entries, if available.
* Easily exclude entries, such as Defender.
* Temporarily filter entries.
* Link to the Support URL.
* Link to HResult explanation (the HResult is placed in the clipboard).
* Toggle the visibility of the details pane.
* Save to a text or CSV file.
* Open Windows Update from the app.
* Choose accent color and one of three themes.
* Adjust app size and row spacing. (Helpful for us users that don't see as well as we used to.)
* Select the interface language.

### The Main Window
![WUView screenshot](https://github.com/Timthreetwelve/WUView/blob/main/Images/WUView_2024-04-04_17-11-03.png)


