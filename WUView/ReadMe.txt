ReadMe file for Windows Update Viewer


Introduction
============
Windows Update Viewer is an application that consolidates information about Windows Updates.
Windows Update Viewer uses the Windows Update API and event logs to display details of installed
updated. Event log entries are associated with individual updates by using the "KB" number. If an
update does not use a KB number, or it isn't presented in a recognized format, no event log entries
will be displayed.


Getting Started
===============
After installing Windows Update Viewer, launch it from the Start menu. You will see that the window
has a grid on the top and a details pane on the bottom. Select a row in the grid and details for
that update will be displayed in the details pane. The details pane can be closed and opened by
selecting Toggle Details Pane on the View menu. The details pane can be resized by dragging the
splitter up or down.


Navigation
==========
Click the three-lined (hamburger) icon at the top left and use the bar that appears on the left to
access the various options in the application.


Filtering the Grid
==================
There is a text box at the top of the main window. Typing in this box will add a filter to any of
the text columns in the grid. The Date column is not included. For example, if you only want to
see the updates for the malicious software remover tool, then type "malicious" in the filter text
box. The grid will update as you type. To see all of the updates, simply clear the filter text box.
The filter will be reset each time Windows Update Viewer is started.


The Exclude List
================
To exclude updates without the need to type a filter every time, there is an exclude list. To access
it select Edit Exclude List from the Settings page or by pressing Ctrl + L. A small dialog will open.
This window is where you can specify strings to be excluded. When you first open this window you will
see that "Defender" has already been added. Consequently any update that has the word "Defender" in
the update title will be excluded. Feel free to add any other strings to this list. For example, if
you don't want to see the malicious software remover tool you cold add "malicious" to the list. The
words on this list are not case sensitive. The exclude list will be saved and reapplied the next time
Windows Update Viewer is started.

If you want to temporarily see all the installed updates, you can select Toggle Excluded Items from
the View Menu or by pressing Ctrl + E instead of removing everything from the exclude list.


Save to a File
==============
You may save the details for all updates that haven't been excluded to a text file by selecting Save
Details to Text File from the File menu. You can also save the current grid data to a CSV file.


Other Options
=============
Other options on the navigation bar include the Settings dialog. You can select between Light,
Metro Dark, Darker and System themes. You can select the accent color and you can choose from seven
size options for the app. You can from three options for row spacing in the grid. You can also control
the visibility of the Details pane and to show or hide updated that match items on the exclude list.
There are also options to have WUView stay on top of other windows and you can control the detail of
the log file.

Selecting About will display the About dialog which shows information about the app such as the
version number and has a link to the GitHub repository where you can check for updates.

You can view the log file, the exclude file or this ReadMe file by clicking on the three dot icon
at the right end of the banner at the top of the screen.


Keyboard Shortcuts
==================
These keyboard shortcuts are available:

	F1 = Go to the About screen
    Ctrl + D = Toggle the Details pane
    Ctrl + E = Toggle display of excluded items
	Ctrl + L = Open the Excludes Editor
	Ctrl + N = Open/close the navigation drawer
	Ctrl + R = Reset column sort
	Ctrl + T = Change the date & time format
	Ctrl + U = Navigate to the Updates page
    Ctrl + Comma = Go to Settings
	Ctrl + Numpad Plus = Increase size
	Ctrl + Numpad Minus = Decrease size
    Ctrl + Shift + C = Change the accent Color
    Ctrl + Shift + T = Change the Theme


Uninstalling
============
To uninstall use the regular Windows add/remove programs feature.


Notices and License
===================
Windows Update Viewer was written by Tim Kennedy.

Windows Update Viewer uses the following packages:

    * Material Design in XAML Toolkit https://github.com/MaterialDesignInXAML/MaterialDesignInXamlToolkit

    * Community Toolkit MVVM https://github.com/CommunityToolkit/dotnet

    * NLog https://nlog-project.org/

    * GitVersion https://github.com/GitTools/GitVersion

    * Inno Setup was used to create the installer. https://jrsoftware.org/isinfo.php



MIT License
Copyright (c) 2023 Tim Kennedy

Permission is hereby granted, free of charge, to any person obtaining a copy of this software and
associated documentation files (the "Software"), to deal in the Software without restriction, including
without limitation the rights to use, copy, modify, merge, publish, distribute, sublicense, and/or
sell copies of the Software, and to permit persons to whom the Software is furnished to do so, subject
to the following conditions:

The above copyright notice and this permission notice shall be included in all copies or substantial
portions of the Software.

THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR IMPLIED, INCLUDING BUT NOT
LIMITED TO THE WARRANTIES OF MERCHANTABILITY, FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT.
IN NO EVENT SHALL THE AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER LIABILITY,
WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM, OUT OF OR IN CONNECTION WITH THE
SOFTWARE OR THE USE OR OTHER DEALINGS IN THE SOFTWARE.
