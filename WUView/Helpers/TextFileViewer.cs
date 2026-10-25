// Copyright (c) Tim Kennedy. All Rights Reserved. Licensed under the MIT License.

// Comment out the following if MessageBox is not to be used
#define messagebox

namespace WUView.Helpers;

/// <summary>
///  Class for viewing text files. If the file extension is not associated
///  with an application, notepad.exe will be attempted.
/// </summary>
internal static class TextFileViewer
{
    #region Text file viewer
    public static void ViewTextFile(string textFile)
    {
        string fname = string.Empty;
        try
        {
            fname = PathHelpers.AnonymizePath(textFile);

            using Process p = new();
            p.StartInfo.FileName = $"\"{textFile}\"";
            p.StartInfo.UseShellExecute = true;
            p.StartInfo.ErrorDialog = false;
            _ = p.Start();
            _log.Debug($"Opening {fname}");
        }
        catch (Win32Exception ex)
        {
            const int ERROR_NO_ASSOCIATION = 1155;
            if (ex.NativeErrorCode == ERROR_NO_ASSOCIATION)
            {
                string notepadPath = PathHelpers.FindOnPath("notepad.exe");
                if (string.IsNullOrEmpty(notepadPath))
                {
                    _log.Error("Unable to find notepad.exe in PATH");
                    string msg = string.Format(CultureInfo.InvariantCulture, MsgTextErrorOpeningFile, textFile);
                    _ = MessageBox.Show($"{msg}\n\nUnable to find notepad.exe in PATH",
                                        GetStringResource("MsgText_Error_Caption"),
                                        MessageBoxButton.OK,
                                        MessageBoxImage.Error);
                    return;
                }
                using Process p = new();
                p.StartInfo.FileName = notepadPath;
                p.StartInfo.Arguments = $"\"{textFile}\"";
                p.StartInfo.UseShellExecute = true;
                p.StartInfo.ErrorDialog = false;
                _ = p.Start();
                _log.Debug($"Opening {fname} in Notepad.exe");
            }
            else
            {
                string msg = string.Format(CultureInfo.InvariantCulture, MsgTextErrorOpeningFile, textFile);
                _ = MessageBox.Show($"{msg}\n\n{ex.Message}",
                                    GetStringResource("MsgText_Error_Caption"),
                                    MessageBoxButton.OK,
                                    MessageBoxImage.Error);
                _log.Error(ex, $"Unable to open {fname}");
            }
        }
        catch (Exception ex)
        {
            string msg = string.Format(CultureInfo.InvariantCulture, MsgTextErrorOpeningFile, textFile);
            _ = MessageBox.Show($"{msg}\n\n{ex.Message}",
                                GetStringResource("MsgText_Error_Caption"),
                                MessageBoxButton.OK,
                                MessageBoxImage.Error);
            _log.Error($"Unable to open {fname}. {ex.Message} ");
        }
    }
#endregion Text file viewer

}
