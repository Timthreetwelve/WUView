// Copyright (c) Tim Kennedy. All Rights Reserved. Licensed under the MIT License.

// Comment out the following if MessageBox is not to be used
#define messagebox

namespace WUView.Helpers;

/// <summary>
///  Class for viewing text files. If the file extension is not associated
///  with an application, notepad.exe will be attempted.
/// </summary>
public static class TextFileViewer
{
    #region Text file viewer
    /// <summary>
    /// Opens specified text file
    /// </summary>
    /// <param name="textFile">Full path for text file</param>
    ///
    public static void ViewTextFile(string textFile)
    {
        try
        {
            using Process p = new();
            p.StartInfo.FileName = textFile;
            p.StartInfo.UseShellExecute = true;
            p.StartInfo.ErrorDialog = false;
            _ = p.Start();
        }
        catch (Win32Exception ex)
        {
            if (ex.NativeErrorCode == 1155)
            {
                using Process p = new();
                p.StartInfo.FileName = "notepad.exe";
                p.StartInfo.Arguments = textFile;
                p.StartInfo.UseShellExecute = true;
                p.StartInfo.ErrorDialog = false;
                _ = p.Start();
                _log.Debug($"Opening {textFile} in Notepad.exe");
            }
            else
            {
#if messagebox
                string msg = string.Format(CultureInfo.InvariantCulture, MsgTextErrorReadingFile, textFile);
                _ = MessageBox.Show($"{msg}\n{ex.Message}",
                                    GetStringResource("MsgText_ErrorCaption"),
                                    MessageBoxButton.OK,
                                    MessageBoxImage.Error);
#endif
                _log.Error(ex, $"Unable to open {textFile}");
            }
        }
        catch (Exception ex)
        {
#if messagebox
            string msg = string.Format(CultureInfo.InvariantCulture, MsgTextErrorOpeningFile, textFile);
            _ = MessageBox.Show($"{msg}\n{ex.Message}",
                                GetStringResource("MsgText_ErrorCaption"),
                                MessageBoxButton.OK,
                                MessageBoxImage.Error);
#endif
            _log.Error(ex, $"Unable to open {textFile}");
        }
    }
    #endregion Text file viewer
}
