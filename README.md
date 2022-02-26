## Windows Update Viewer

[![GitHub](https://img.shields.io/github/license/Timthreetwelve/WUView?style=plastic)](https://github.com/Timthreetwelve/WUView/blob/main/LICENSE)
[![NET6win](https://img.shields.io/badge/.NET-6.0--Windows-blueviolet?style=plastic)](https://dotnet.microsoft.com/en-us/download) 
[![GitHub release (latest by date)](https://img.shields.io/github/v/release/Timthreetwelve/WUView?style=plastic)](https://github.com/Timthreetwelve/WUView/releases/latest) 
[![GitHub Release Date](https://img.shields.io/github/release-date/timthreetwelve/WUView?style=plastic&color=orange)](https://github.com/Timthreetwelve/WUView/releases/latest) 
[![GitHub last commit](https://img.shields.io/github/last-commit/timthreetwelve/WUView?style=plastic)](https://github.com/Timthreetwelve/WUView/commits/main)
[![GitHub all releases](https://img.shields.io/github/downloads/Timthreetwelve/WUView/total?style=plastic&color=teal)](https://github.com/Timthreetwelve/WUView/releases) 

Windows Update Viewer (WUView) is an application that displays information about Windows Updates. WUView uses the Windows Update API and Windows event logs to display details of installed updates. Event log entries are associated with individual updates by using the "KB" number. If an update does not use a KB number, or it isn't presented in a consistent format, no event log entries will be displayed.

### Features
* View details for each update.
* Easily exclude entries, such as Defender.
* Link to the Support URL.
* Link to HResult explanation.
* Hide details pane.
* Save to a text or CSV file.
* Open Windows Update from the app.
* Open Event Viewer from the app. 

✨ WUView requires up-to-date .Net 6.0

### The Main Window
![WUView screenshot](https://github.com/Timthreetwelve/WUView/blob/main/Images/WUView.png)

### Oddities
#### Update: It seems as this issue is not present under Windows 11

Several updates on my machines show the HResult code 0x80242014 even though the Windows Update History shows that the updates were successfully installed. 0x80242014 translates to WU_E_UH_POSTREBOOTSTILLPENDING "The post-reboot operation for the update is still in progress."  This persists over multiple reboots.
