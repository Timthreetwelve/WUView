// Copyright (c) Tim Kennedy. All Rights Reserved. Licensed under the MIT License.

namespace WUView.ViewModels;

public partial class SettingsViewModel : ObservableObject
{
    #region Properties
    public static List<FontFamily>? FontList { get; private set; }
    #endregion Properties

    #region Constructor
    public SettingsViewModel()
    {
        FontList ??= [.. Fonts.SystemFontFamilies.OrderBy(x => x.Source)];
    }
    #endregion Constructor

    #region Relay commands
    [RelayCommand]
    private static void OpenAppFolder()
    {
        string filePath = string.Empty;
        try
        {
            filePath = Path.Combine(AppInfo.AppDirectory, "Strings.test.xaml");
            if (File.Exists(filePath))
            {
                _ = Process.Start("explorer.exe", string.Format(CultureInfo.InvariantCulture, "/select,\"{0}\"", filePath));
            }
            else
            {
                using Process p = new();
                p.StartInfo.FileName = AppInfo.AppDirectory;
                p.StartInfo.UseShellExecute = true;
                p.StartInfo.ErrorDialog = false;
                _ = p.Start();
            }
        }
        catch (Exception ex)
        {
            _log.Error(ex, $"Error trying to open {filePath}: {ex.Message}");
            _ = new MDCustMsgBox(GetStringResource("MsgText_Error_FileExplorer"),
                     GetStringResource("MsgText_ErrorCaption"),
                     ButtonType.Ok,
                     false,
                     true,
                     null,
                     true).ShowDialog();
        }
    }
    #endregion Relay commands
}
