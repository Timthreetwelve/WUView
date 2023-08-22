## Windows Update Viewer

[![GitHub](https://img.shields.io/github/license/Timthreetwelve/WUView?style=plastic)](https://github.com/Timthreetwelve/WUView/blob/main/LICENSE)
[![NET6win](https://img.shields.io/badge/.NET-6.0--Windows-blueviolet?style=plastic)](https://dotnet.microsoft.com/en-us/download) 
[![GitHub release (latest by date)](https://img.shields.io/github/v/release/Timthreetwelve/WUView?style=plastic)](https://github.com/Timthreetwelve/WUView/releases/latest) 
[![GitHub Release Date](https://img.shields.io/github/release-date/timthreetwelve/WUView?style=plastic&color=orange)](https://github.com/Timthreetwelve/WUView/releases/latest) 
[![GitHub all releases](https://img.shields.io/github/downloads/Timthreetwelve/WUView/total?style=plastic&label=total%20downloads&color=teal)](https://github.com/Timthreetwelve/WUView/releases) 
[![GitHub release (by tag)](https://img.shields.io/github/downloads/timthreetwelve/wuview/latest/total?style=plastic&color=2196F3)](https://github.com/Timthreetwelve/WUView/releases/latest) 


Windows Update Viewer (WUView) is an application that displays Windows Update history. It is meant to be a lightweight application that is easy to use. There aren't any confusing categories; every update is listed in one place. Updates that you don't want to see can be permanently excluded or temporarily filtered.

WUView uses the Windows Update Agent API and Windows event logs to display details of installed updates. Event log entries are associated with individual updates by using the "KB" number. If an update does not use a KB number or isn't presented in a consistent format, no event log entries will be displayed.

Please be aware that Windows Update Viewer can only display updates provided by the [Windows Update Agent API](https://learn.microsoft.com/en-us/windows/win32/wua_sdk/portal-client). If you have reason to suspect that the application isn't returning correct or complete information, see the [Troubleshooting](https://github.com/Timthreetwelve/WUView/wiki/Troubleshooting) and [Known Issues](https://github.com/Timthreetwelve/WUView/wiki/Known-Issues) topics in the Wiki.

If you are using Windows 10 please see [Known Issues](https://github.com/Timthreetwelve/WUView/wiki/Known-Issues). 

See the [Wiki](https://github.com/Timthreetwelve/WUView/wiki) for additional information.

### Get Windows Update Viewer via winget   [![Publish to WinGet](https://github.com/Timthreetwelve/WUView/actions/workflows/publish-to-winget.yml/badge.svg)](https://github.com/Timthreetwelve/WUView/actions/workflows/publish-to-winget.yml)
Windows update viewer is now available through Windows Package Manager (a.k.a. winget)! Many thanks to [@ottnorml](https://github.com/ottnorml) for making this happen!👏 Using winget, the name is `"Windows Update Viewer"` (use double quotes since the name has spaces). The ID is `Timthreetwelve.WUView`.

### Download Windows Update Viewer
You can always download the latest release from the [releases page](https://github.com/Timthreetwelve/WUView/releases). Note that a "portable" release (the one with "NonInstall.zip" in the file name) is provided as well as the traditional installer.

### Features
* View details for each update.
* Easily exclude entries, such as Defender.
* Link to the Support URL.
* Link to HResult explanation (the HResult is placed in the clipboard).
* Toggle the details pane.
* Save to a text or CSV file.
* Open Windows Update from the app.
* Choose accent color and one of three themes.
* Adjust app size and row spacing. (Helpful for us users that don't see as well as we used to.)

#### WUView requires up-to-date .Net 6.0

### The Main Window
![WUView screenshot](https://github.com/Timthreetwelve/WUView/blob/main/Images/WUView550.png)

