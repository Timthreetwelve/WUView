// Copyright (c) Tim Kennedy. All Rights Reserved. Licensed under the MIT License.

namespace WUView.ViewModels;

public partial class AboutViewModel
{
    #region Relay Commands
    [RelayCommand]
    public static void ViewLicense()
    {
        string dir = AppInfo.AppDirectory;
        TextFileViewer.ViewTextFile(Path.Combine(dir, "License.txt"));
    }

    [RelayCommand]
    public static void ViewReadMe()
    {
        string dir = AppInfo.AppDirectory;
        TextFileViewer.ViewTextFile(Path.Combine(dir, "ReadMe.txt"));
    }

    [RelayCommand]
    public static void GoToGitHub(string url)
    {
        Process p = new();
        p.StartInfo.FileName = url;
        p.StartInfo.UseShellExecute = true;
        p.Start();
    }

    [RelayCommand]
    public static async Task CheckReleaseAsync()
    {
        await GitHubHelpers.CheckRelease();
    }
    #endregion Relay Commands
}
