// Copyright (c) Tim Kennedy. All Rights Reserved. Licensed under the MIT License.

using Octokit;

namespace WUView.Helpers;

/// <summary>
/// Class for methods that check GitHub for releases
/// </summary>
internal static class GitHubHelpers
{
    #region MainWindow Instance
    private static readonly MainWindow? _mainWindow = System.Windows.Application.Current.MainWindow as MainWindow;
    #endregion MainWindow Instance

    #region Check for newer release
    /// <summary>
    /// Checks to see if a newer release is available.
    /// </summary>
    /// <remarks>
    /// If the release version is greater than the current version
    /// a message box will be shown asking to go to the releases page.
    /// </remarks>
    public static async Task CheckRelease()
    {
        try
        {
            SnackbarMsg.ClearAndQueueMessage(GetStringResource("MsgText_AppUpdateChecking"));
            Release release = await GetLatestReleaseAsync(AppConstString.RepoOwner, AppConstString.RepoName);
            if (release == null)
            {
                CheckFailed();
                return;
            }

            string tag = release.TagName;
            if (string.IsNullOrEmpty(tag))
            {
                CheckFailed();
                return;
            }

            if (tag.StartsWith("v", StringComparison.InvariantCultureIgnoreCase))
            {
                tag = tag.ToLower(CultureInfo.InvariantCulture).TrimStart('v');
            }

            Version latestVersion = new(tag);

            _log.Debug($"Latest version is {latestVersion} released on {release.PublishedAt!.Value.UtcDateTime} UTC");

            if (latestVersion <= AppInfo.AppVersionVer)
            {
                string msg = GetStringResource("MsgText_AppUpdateNoneFound");
                _log.Debug(msg);
                _ = new MDCustMsgBox(msg,
                    "Windows Update Viewer",
                    ButtonType.Ok,
                    false,
                    true,
                    _mainWindow).ShowDialog();
            }
            else
            {
                _log.Debug($"A newer release ({latestVersion}) has been found.");
                string msg = string.Format(CultureInfo.InvariantCulture, MsgTextAppUpdateNewerFound, latestVersion);
                _ = new MDCustMsgBox($"{msg}\n\n" +
                                            $"{GetStringResource("MsgText_AppUpdateGoToRelease")}\n\n" +
                                            $"{GetStringResource("MsgText_AppUpdateClose")}",
                    "Windows Update Viewer",
                    ButtonType.YesNo,
                    false,
                    true,
                    _mainWindow).ShowDialog();

                if (MDCustMsgBox.CustResult == CustResultType.Yes)
                {
                    string opening = GetStringResource("MsgText_Opening");
                    _log.Debug($"{opening} {release.HtmlUrl}");
                    string url = release.HtmlUrl;
                    Process p = new();
                    p.StartInfo.FileName = url;
                    p.StartInfo.UseShellExecute = true;
                    p.Start();
                    System.Windows.Application.Current.Shutdown();
                }
            }
        }
        catch (Exception ex)
        {
            _log.Error(ex, "Error encountered while checking version");
            CheckFailed();
        }
    }
    #endregion Check for newer release

    #region Get latest release
    /// <summary>
    /// Gets the latest release.
    /// </summary>
    /// <param name="repoOwner">The repository owner.</param>
    /// <param name="repoName">Name of the repository.</param>
    /// <returns>Release object</returns>
    private static async Task<Release> GetLatestReleaseAsync(string repoOwner, string repoName)
    {
        try
        {
            GitHubClient client = new(new ProductHeaderValue(repoName));
            _log.Debug("Checking GitHub for latest release.");

            return await client.Repository.Release.GetLatest(repoOwner, repoName);
        }
        catch (Exception ex)
        {
            _log.Error(ex, "Get latest release from GitHub failed.");
            return null!;
        }
    }
    #endregion Get latest release

    #region Check failed message
    /// <summary>
    /// Display a message box stating that the release check failed.
    /// </summary>
    private static void CheckFailed()
    {
        _ = new MDCustMsgBox(GetStringResource("MsgText_AppUpdateCheckFailed"),
            "Windows Update Viewer",
            ButtonType.Ok,
            false,
            true,
            _mainWindow,
            true).ShowDialog();
    }
    #endregion Check failed message
}
