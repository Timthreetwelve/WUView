# WUView
Repository for the Windows Update Viewer project

Introduction
============
Windows Update Viewer is an application that consolidates information about Windows Updates.
Windows Update Viewer uses the Windows Update API and event logs to display details of installed
updated. Event log entries are associated with individual updates by using the "KB" number. If an
update does not use a KB number, or it isn't presented in a consistent format, no event log entries
will be displayed.


HResult Codes
=============
The HResult code for each update is shown in the details pane. Clicking on it will open the Windows
Update error code page in your default browser. The value of the error code is also copied to the
clipboard. One the error code page is open in your browser, you can simply type Ctrl+F followed by
Ctrl+V to find the error code. The Help menu also contains a selection that will open the error code
page.


Getting Started
===============
After installing Windows Update Viewer, launch it from the Start menu. You will see that the window
has a grid on the top and a details pane on the bottom. Select a row in the grid and details for
that update will be displayed in the details pane. The details pane can be closed by unchecking
Show Detail Pane on the Options menu. The details pane can be resized by dragging the splitter up
or down.


Filtering the Grid
==================
There is a text box to the right of the Help menu. Typing in this box will add a filter to any of the
text columns in the grid. The Date column is not included. Fore example, if you only want to see the
updates for the malicious software remover tool, then type "malicious" in the filter text box. The
grid will update as you type. To exclude updates, type a minus sign or dash "-" in the first position
immediately followed by the text you wish to exclude.  For example, to exclude the malicious software
removal tool, you can type "-malicious".  To see all of the updates, simply clear the filter text box.
The filter will be reset each time Windows Update Viewer is started.


The Exclude List
================
To exclude updates without the need to type a filter every time, there is an exclude list. To use it
click on the Options menu then on Edit Exclude List. A small window will open. This window is where
you can specify strings to be excluded. When you first open this window you will see that "Defender"
has already been added. Consequently any update that has the word "Defender" in the update title will
be excluded. Feel free to add any other strings to this list. For example, you don't want to see the
malicious software remover tool you cold add "malicious" to the list. The words on this list are not
case sensitive. The exclude list will be saved and reapplied the next time Windows Update Viewer is
started.

If you want to temporarily see all the installed updates, you can uncheck Hide Excluded on the
Options menu instead of deleting everything from the exclude list. You can also copy just the
information in the grid portion to the Windows clipboard by selecting that item from the File menu.


Save to a File
==============
You may save the details for all updates that haven't been excluded to a text file by selecting Save
Details to Text File from the File menu. You can also gave the grid data to a CSV file.


Other Options
=============
There are other menu options to change the zoom level of the window, choose another font, show grid
lines and alternate row shading. It is also possible to open the Windows Event Log and Windows Update
from the File menu. You can change the background of the details pane by selecting the option from
the Options menu.


Notices and License
===================
MyScheduledTasks was written in C# by Tim Kennedy.

MyScheduledTasks uses the following icons & packages:

Fugue Icons set https://p.yusukekamiyamane.com/

Json.net v12.0.3 from Newtonsoft https://www.newtonsoft.com/json

NLog v4.7.5 https://nlog-project.org/


MIT License
Copyright (c) 2020 Tim Kennedy

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
