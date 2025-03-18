// Copyright (c) Tim Kennedy. All Rights Reserved. Licensed under the MIT License.

namespace WUView.ViewModels;

internal sealed class MainViewModel : ObservableObject
{
    #region Event record and WUpdate Lists
    private static List<WuEventRecord> EventLogRecords { get; } = [];
    public static ObservableCollection<WUpdate> UpdatesFullList { get; } = [];
    public static ObservableCollection<WUpdate> UpdatesWithoutExcludedItems { get; } = [];
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
        if (UserSettings.Setting!.AutoSelectFirstRow && MainPage.Instance!.DataGrid.Items.Count > 0)
        {
            MainPage.Instance.DataGrid.SelectedIndex = 0;
        }
    }
    #endregion Kick off the process of gathering the information

    #region Get the list of Windows updates
    /// <summary>
    /// Gets list of Windows Updates
    /// </summary>
    private static void GetListOfUpdates()
    {
        Stopwatch sw = new();
        sw.Start();
        IUpdateSession updateSession = new();
        IUpdateSearcher updateSearcher = updateSession.CreateUpdateSearcher();
        int count = updateSearcher.GetTotalHistoryCount();
        _log.Debug($"Collected {count} Windows Update records in {sw.Elapsed.TotalMilliseconds:N2} milliseconds");
        sw.Restart();
        // Believe it or not, it's possible to not have any updates.
        if (count >= 1)
        {
            int maxUpdates = UserSettings.Setting!.MaxUpdates switch
            {
                MaxUpdates.All => count,
                MaxUpdates.Max50 => 50,
                MaxUpdates.Max100 => 100,
                MaxUpdates.Max250 => 250,
                MaxUpdates.Max500 => 500,
                _ => count,
            };
            maxUpdates = Math.Min(maxUpdates, count);
            _log.Debug(UserSettings.Setting.MaxUpdates == MaxUpdates.All ? $"Using all {maxUpdates} update records" : $"Using {maxUpdates} update records");

            foreach (IUpdateHistoryEntry hist in updateSearcher.QueryHistory(0, maxUpdates))
            {
                string kbNum = GetKB(hist.Title);
                try
                {
                    WUpdate update = new()
                    {
                        Title = hist.Title,
                        KBNum = kbNum,
                        Date = hist.Date.ToLocalTime(),
                        ResultCode = ResultCodeHelper.TranslateResultCode(hist.ResultCode),
                        HResult = hist.UnmappedResultCode.ToString(CultureInfo.InvariantCulture),
                        Operation = OperationHelper.TranslateOperation(hist.Operation),
                        UpdateID = hist.UpdateIdentity.UpdateID,
                        Description = hist.Description ?? string.Empty,
                        // ReSharper disable once NullCoalescingConditionIsAlwaysNotNullAccordingToAPIContract
                        SupportURL = hist.SupportUrl ?? string.Empty,
                        ELDescription = FindEventLogs(kbNum)
                    };
                    UpdatesFullList.Add(update);
                    if (hist.HResult != 0 && UserSettings.Setting.ShowLogWarnings)
                    {
                        string operation = update.Operation.Replace("uo", "");
                        string HResultHex = string.Format(CultureInfo.InvariantCulture, $"0x{int.Parse(update.HResult, CultureInfo.InvariantCulture):X8}");
                        _log.Warn($"KB: {update.KBNum,-10} Date: {update.Date,-23} HResult: {HResultHex,-10} " +
                                 $" Operation: {operation,-12}  UpdateID: {update.UpdateID}");
                    }
                }
                catch (Exception ex)
                {
                    _log.Error(ex, "Error while parsing Windows Update records.");
                }
            }
        }
        else
        {
            _log.Info($"No updates found! IUpdateSearcher.GetTotalHistoryCount returned {count}.");
            new MDCustMsgBox(GetStringResource("MsgText_NoUpdatesFound"),
                    "Windows Update Viewer",
                    ButtonType.Ok).Show();
        }
        _log.Debug($"Building the list of {count} updates took {sw.Elapsed.TotalMilliseconds:N2} milliseconds");
    }
    #endregion Get the list of Windows updates

    #region Get KB Number from Title
    /// <summary>
    /// Finds a KB number in the title of the update
    /// </summary>
    /// <param name="title">Update title</param>
    /// <returns>Returns either the KB number or n/a</returns>
    private static string GetKB(string? title)
    {
        if (title == null)
        {
            _log.Error("Null title found while parsing Windows Update records.");
            return "n/a";
        }
        title = title.Replace("(", "").Replace(")", "");
        int pos = title.IndexOf("KB", StringComparison.InvariantCulture);
        if (pos > -1)
        {
            int endPosition = title.IndexOf(' ', pos);
            if (endPosition > -1)
            {
                return title[pos..endPosition];
            }
            return title[pos..];
        }
        return "n/a";
    }
    #endregion Get KB Number from Title

    #region Match event logs to update items
    /// <summary>
    /// Matches up any Event Log records with a Windows Update based on KB number
    /// </summary>
    /// <param name="kb">KB number to search for</param>
    /// <returns>Returns a string containing relevant Event Log records or a message saying that none could be found</returns>
    public static string FindEventLogs(string kb)
    {
        if (kb == "n/a")
        {
            return GetStringResource("MsgText_EventLogNA");
        }
        StringBuilder sbEventLog = new();
        foreach (WuEventRecord item in EventLogRecords.Where(item => item.KayBee!.Contains(kb, StringComparison.InvariantCultureIgnoreCase)))
        {
            string tc = string.Format(CultureInfo.InvariantCulture, $"{item.TimeCreated} - {item.Description}  Event ID: {item.EventId}.  Record ID: {item.RecordId}.");
            _ = sbEventLog.AppendLine(tc);
        }

        if (sbEventLog.Length == 0)
        {
            string message = string.Format(CultureInfo.InvariantCulture, MsgTextEventLogNoRecords, kb);
            _ = sbEventLog.AppendLine(message);
        }
        return sbEventLog.ToString();
    }
    #endregion Match event logs to update items

    #region Remove excluded items and create list without excludes
    /// <summary>
    /// Creates a List that doesn't include any records on the exclude list
    /// </summary>
    private static void PopulateExcludedList()
    {
        Stopwatch esw = new();
        esw.Start();
        UpdatesWithoutExcludedItems.Clear();
        for (int i = 0; i < UpdatesFullList.Count; i++)
        {
            WUpdate upd = UpdatesFullList[i];
            try
            {
                bool skip = false;
                for (int j = 0; j < ExcludedItems.ExcludedStrings.Count; j++)
                {
                    ExcludedItems exc = ExcludedItems.ExcludedStrings[j];
                    if (skip)
                    {
                        break;
                    }
                    if (UserSettings.Setting!.ExcludeKBandResult)
                    {
                        if (upd.Title!.Contains(exc.ExcludedString!, StringComparison.OrdinalIgnoreCase) ||
                            upd.KBNum!.Contains(exc.ExcludedString!, StringComparison.OrdinalIgnoreCase) ||
                            upd.ResultCode!.Contains(exc.ExcludedString!, StringComparison.OrdinalIgnoreCase) ||
                            upd.UpdateID!.Contains(exc.ExcludedString!, StringComparison.OrdinalIgnoreCase))
                        {
                            skip = true;
                        }
                    }
                    else if (upd.Title!.Contains(exc.ExcludedString!, StringComparison.OrdinalIgnoreCase))
                    {
                        skip = true;
                    }
                }
                if (!skip)
                {
                    UpdatesWithoutExcludedItems.Add(upd.GetClone());
                }
            }
            catch (Exception ex)
            {
                _log.Error(ex, "Error building Exclude list.");
            }
        }
        esw.Stop();
        _log.Debug($"Excluding {UpdatesFullList.Count - UpdatesWithoutExcludedItems.Count} records took {esw.Elapsed.TotalMilliseconds:N2} milliseconds");

        if (UpdatesFullList.Count > 0 && UpdatesWithoutExcludedItems.Count == 0)
        {
            new MDCustMsgBox(GetStringResource("MsgText_AllUpdatesExcluded"),
                "Windows Update Viewer",
                ButtonType.Ok,
                false,
                true,
                Application.Current.MainWindow).Show();
        }
    }
    #endregion Remove excluded items and create list without excludes

    #region Get Event Log details
    /// <summary>
    /// Gets all "Setup" records from the Event Log
    /// </summary>
    private static void GetEventLog()
    {
        Stopwatch swe = Stopwatch.StartNew();
        const string query = "*[System/Provider/@Name=\"Microsoft-Windows-Servicing\"]";
        EventLogQuery eventsQuery = new("Setup", PathType.LogName, query);

        try
        {
            using EventLogReader logReader = new(eventsQuery);
            EventRecord eventDetail = logReader.ReadEvent();
            while (eventDetail != null)
            {
                string kb = eventDetail.Properties[0].Value.ToString()!;
                if (kb.StartsWith("KB", StringComparison.InvariantCultureIgnoreCase))
                {
                    WuEventRecord eventRecord = new()
                    {
                        KayBee = kb,
                        TimeCreated = eventDetail.TimeCreated!.Value.ToLocalTime(),
                        Description = eventDetail.FormatDescription(),
                        EventId = eventDetail.Id,
                        RecordId = eventDetail.RecordId
                    };
                    EventLogRecords.Add(eventRecord);
                }
                eventDetail = logReader.ReadEvent();
            }
        }
        catch (UnauthorizedAccessException ex)
        {
            _log.Error(ex, $"Unauthorized Access Error while reading the event logs.\n{ex.Message}");
            _ = MessageBox.Show($"Unauthorized Access Error while reading the event logs.\n{ex.Message} ",
                            "Windows Update Viewer",
                            MessageBoxButton.OK,
                            MessageBoxImage.Error);
        }
        catch (Exception ex)
        {
            _log.Error(ex, $"Error while reading the event logs\n{ex.Message}");
        }
        finally
        {
            swe.Stop();
            _log.Debug($"Collected {EventLogRecords.Count} Setup event log records in {swe.Elapsed.TotalMilliseconds:N2} milliseconds");
        }
    }
    #endregion Get Event Log details

    #region Edit exclude file
    /// <summary>
    /// Edit the exclude file
    /// </summary>
    public static async Task EditExcludes()
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
            await FileHelpers.SaveExcludeFile();
            PopulateExcludedList();
            MainPage.Instance!.UpdateGrid();
            DisplayCount();
        }
    }
    #endregion Edit exclude file

    #region Display update count
    /// <summary>
    /// Displays the count of updates in a snack bar message
    /// </summary>
    private static void DisplayCount()
    {
        int total = UpdatesFullList.Count;
        int displayed = MainPage.Instance!.DataGrid.Items.Count;
        string message = string.Format(CultureInfo.InvariantCulture, MsgTextDisplayedUpdates, displayed, total);
        SnackbarMsg.ClearAndQueueMessage(message);
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
