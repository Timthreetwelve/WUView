// Copyright (c) Tim Kennedy. All Rights Reserved. Licensed under the MIT License.

namespace WUView.ViewModels;

internal partial class MainViewModel : ObservableObject
{
    #region NLog Instance
    private static readonly Logger _log = LogManager.GetLogger("logTemp");
    #endregion NLog Instance

    #region Event record and WUpdate Lists
    public static List<EventRecord> EventLogRecords { get; set; } = new();
    public static ObservableCollection<WUpdate> UpdatesFullList { get; set; } = new();
    public static ObservableCollection<WUpdate> UpdatesWithoutExcludedItems { get; set; } = new();
    #endregion Event record and WUpdate Lists

    #region Kick off the process of gathering the information
    public static void GatherInfo()
    {
        Mouse.OverrideCursor = Cursors.Wait;
        FileHelpers.GetExcludes();
        GetEventLog();
        GetListOfUpdates();
        PopulateExcludedList();
        Mouse.OverrideCursor = null;
        DisplayCount();
    }
    #endregion Kick off the process of gathering the information

    #region Get the list of Windows updates
    /// <summary>
    /// Gets list of Windows Updates
    /// </summary>
    public static void GetListOfUpdates()
    {
        Stopwatch sw = new();
        sw.Start();
        UpdateSession updateSession = new();
        IUpdateSearcher updateSearcher = updateSession.CreateUpdateSearcher();
        int count = updateSearcher.GetTotalHistoryCount();
        _log.Debug($"Read {count} Windows Update records in {sw.Elapsed.TotalMilliseconds:N2} milliseconds");
        sw.Restart();
        // Believe it or not, it's possible to not have any updates.
        if (count >= 1)
        {
            Stopwatch gkbsw = new();
            Stopwatch updsw = new();
            foreach (IUpdateHistoryEntry hist in updateSearcher.QueryHistory(0, count))
            {
                gkbsw.Start();
                string kbNum = GetKB(hist.Title);
                gkbsw.Stop();
                updsw.Start();
                WUpdate update = new()
                {
                    Title = hist.Title,
                    KBNum = kbNum,
                    Date = hist.Date.ToLocalTime(),
                    ResultCode = hist.ResultCode.ToString(),
                    HResult = hist.HResult.ToString(),
                    Operation = hist.Operation.ToString(),
                    UpdateID = hist.UpdateIdentity.UpdateID,
                    Description = hist.Description,
                    SupportURL = hist.SupportUrl ?? string.Empty,
                    ELDescription = FindEventLogs(kbNum)
                };
                UpdatesFullList.Add(update);
                updsw.Stop();
                if (hist.HResult != 0)
                {
                    string HResultHex = string.Format($"0x{int.Parse(update.HResult):X8}");
                    _log.Warn($"KB: {update.KBNum,-10} Date: {update.Date,-23} HResult: {HResultHex,-10} " +
                             $" Operation: {update.Operation,-12}  UpdateID: {update.UpdateID}");
                }
            }
            _log.Debug($"Extracting KB numbers from update titles took {gkbsw.Elapsed.TotalMilliseconds:N2} milliseconds");
            _log.Debug($"Building WUpdate object took {updsw.Elapsed.TotalMilliseconds:N2} milliseconds");
        }
        else
        {
            _log.Info($"No updates found! IUpdateSearcher.GetTotalHistoryCount returned {count}.");
            new MDCustMsgBox("No updates were found.",
                    "Windows Update Viewer",
                    ButtonType.Ok,
                    false,
                    true,
                    null,
                    false).Show();
        }
        _log.Debug($"Building the list of updates took {sw.Elapsed.TotalMilliseconds:N2} milliseconds");
    }
    #endregion Get the list of Windows updates

    #region Get KB Number from Title
    /// <summary>
    /// Finds a KB number in the title of the update
    /// </summary>
    /// <param name="title">Update title</param>
    /// <returns>Returns either the KB number or n/a</returns>
    private static string GetKB(string title)
    {
        title = title.Replace("(", "").Replace(")", "");
        int pos = title.IndexOf("KB");
        if (pos > -1)
        {
            int endpos = title.IndexOf(" ", pos);
            if (endpos > -1)
            {
                return title[pos..endpos];
            }
            return title[pos..];
        }
        return "n/a";
    }
    #endregion Get KB Number from Title

    #region Match event logs to update items
    /// <summary>
    /// Matches up any Event Log records with a Windows Update bases on KB number
    /// </summary>
    /// <param name="kb"></param>
    /// <returns>Returns a string containing relevant Event Log records or a message saying that none could be found</returns>
    public static string FindEventLogs(string kb)
    {
        if (kb == "n/a")
        {
            return "No Event Logs records were found for this update.";
        }
        StringBuilder sbEventLog = new();
        foreach (EventRecord item in EventLogRecords)
        {
            if (item.Properties[0].Value.ToString().Contains(kb))
            //if (item.FormatDescription().Contains(kb))  This took forever
            {
                string tc = string.Format($"{item.TimeCreated} - {item.FormatDescription()}  Event ID: {item.Id}.");
                _ = sbEventLog.AppendLine(tc);
            }
        }
        if (sbEventLog.Length == 0)
        {
            _ = sbEventLog.Append("No Event Log records containing \"").Append(kb).AppendLine("\" were found.");
        }
        return sbEventLog.ToString();
    }
    #endregion Match event logs to update items

    #region Remove excluded items and create list without excludes
    /// <summary>
    /// Creates a List that doesn't include any records on the exclude list
    /// </summary>
    public static void PopulateExcludedList()
    {
        Stopwatch esw = new();
        esw.Start();
        UpdatesWithoutExcludedItems.Clear();
        foreach (WUpdate upd in UpdatesFullList)
        {
            bool skip = false;
            foreach (ExcludedItems exc in ExcludedItems.ExcludedStrings)
            {
                if (upd.Title.Contains(exc.ExcludedString, StringComparison.OrdinalIgnoreCase) && !skip)
                {
                    skip = true;
                }
            }
            if (!skip)
            {
                UpdatesWithoutExcludedItems.Add(upd.GetClone());
            }
        }
        esw.Stop();
        _log.Debug($"Removing {UpdatesFullList.Count - UpdatesWithoutExcludedItems.Count} " +
                   $"excluded items took {esw.Elapsed.TotalMilliseconds:N2} milliseconds");

        if (UpdatesFullList.Count > 0 && UpdatesWithoutExcludedItems.Count == 0)
        {
            new MDCustMsgBox("All displayable updates have been excluded.",
                "Windows Update Viewer",
                ButtonType.Ok,
                false,
                true,
                Application.Current.MainWindow,
                false).Show();
        }
    }
    #endregion Remove excluded items and create list without excludes

    #region Get Event Log details
    /// <summary>
    /// Gets all "Setup" records from the Event Log
    /// </summary>
    public static void GetEventLog()
    {
        Stopwatch swe = new();
        swe.Start();
        const string query = "*[System/Provider/@Name=\"Microsoft-Windows-Servicing\"]";
        EventLogQuery eventsQuery = new("Setup", PathType.LogName, query);

        try
        {
            EventLogReader logReader = new(eventsQuery);
            for (EventRecord eventdetail = logReader.ReadEvent(); eventdetail != null; eventdetail = logReader.ReadEvent())
            {
                if (eventdetail.FormatDescription().Contains("KB"))
                {
                    EventLogRecords.Add(eventdetail);
                }
            }
        }
        catch (EventLogNotFoundException e)
        {
            _log.Error($"Error while reading the event logs\n{e.Message}");
        }
        swe.Stop();
        _log.Debug($"Read {EventLogRecords.Count} Setup event log records in {swe.Elapsed.TotalMilliseconds:N2} milliseconds");
    }
    #endregion Get Event Log details

    #region Edit exclude file
    /// <summary>
    /// Edit the exclude file
    /// </summary>
    public static async void EditExcludes()
    {
        bool result;
        if (!DialogHost.IsDialogOpen("MainDialogHost"))
        {
            result = await DialogHelpers.ShowEditExcludesDialog();
        }
        else
        {
            DialogHost.Close("MainDialogHost");
            result = await DialogHelpers.ShowEditExcludesDialog();
        }

        if (result)
        {
            FileHelpers.SaveExcludeFile();
            PopulateExcludedList();
            MainPage.Instance.UpdateGrid();
            DisplayCount();
        }
    }
    #endregion Edit exclude file

    #region Display update count
    /// <summary>
    /// Displays the count of updates in a snackbar message
    /// </summary>
    internal static void DisplayCount()
    {
        int total = UpdatesFullList.Count;
        int displayed = MainPage.Instance.dataGrid.Items.Count;
        SnackbarMsg.ClearAndQueueMessage($"Displaying {displayed} of {total} updates");
    }
    #endregion Display update count

    #region Clear the lists
    /// <summary>
    /// Clears the updates and event log lists.
    /// </summary>
    public static void ClearLists()
    {
        UpdatesWithoutExcludedItems.Clear();
        UpdatesFullList.Clear();
        EventLogRecords.Clear();
    }
    #endregion
}
