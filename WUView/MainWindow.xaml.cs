// Copyright(c) Tim Kennedy. All Rights Reserved. Licensed under the MIT License.

namespace WUView;

public partial class MainWindow : Window
{
    #region NLog
    internal static readonly Logger log = LogManager.GetCurrentClassLogger();
    #endregion NLog

    #region Stopwatch
    private readonly Stopwatch stopwatch = new();
    #endregion Stopwatch

    #region Event record and WUpdate Lists
    private readonly List<EventRecord> eventLogRecords = new();
    private readonly List<WUpdate> updatesFullList = new();
    private readonly List<WUpdate> updatesWithoutExcludesList = new();
    #endregion Event record and WUpdate Lists

    public MainWindow()
    {
        InitializeComponent();

        InitializeSettings();

        ReadSettings();
    }

    #region Settings
    private void InitializeSettings()
    {
        stopwatch.Start();

        UserSettings.Init(UserSettings.AppFolder, UserSettings.DefaultFilename, true);
    }

    public void ReadSettings()
    {
        // Set NLog configuration
        NLHelpers.NLogConfig(UserSettings.Setting.NewLog);

        // Unhandled exception handler
        AppDomain.CurrentDomain.UnhandledException += CurrentDomain_UnhandledException;

        // Put the version number in the title bar
        Title = $"{AppInfo.AppName} - {AppInfo.AppFileVersion}";

        // Log the version, build date and commit id
        log.Info($"{AppInfo.AppName} ({AppInfo.AppProduct}) {AppInfo.AppVersion} is starting up");
        log.Info($"{AppInfo.AppName} {AppInfo.AppCopyright}");
        log.Debug($"{AppInfo.AppName} Build date: {BuildInfo.BuildDateObj:f}");
        log.Debug($"{AppInfo.AppName} Commit ID: {BuildInfo.CommitIDString} ");

        // Log the .NET version, app framework and OS platform
        string version = Environment.Version.ToString();
        log.Debug($".NET version: {AppInfo.RuntimeVersion.Replace(".NET", "")}  ({version})");
        log.Debug(AppInfo.Framework);
        log.Debug(AppInfo.OsPlatform);

        // Window position
        Top = UserSettings.Setting.WindowTop;
        Left = UserSettings.Setting.WindowLeft;
        Height = UserSettings.Setting.WindowHeight;
        Width = UserSettings.Setting.WindowWidth;
        Topmost = UserSettings.Setting.KeepOnTop;

        // Light or dark
        MainWindowUIHelpers.SetBaseTheme((ThemeType)UserSettings.Setting.DarkMode);

        // Primary color
        MainWindowUIHelpers.SetPrimaryColor((AccentColor)UserSettings.Setting.PrimaryColor);

        // UI size
        double size = MainWindowUIHelpers.UIScale((MySize)UserSettings.Setting.UISize);
        MainGrid.LayoutTransform = new ScaleTransform(size, size);

        // Font weight
        SetFontWeight((Weight)UserSettings.Setting.GridFontWeight);

        // DataGrid row spacing
        SetRowSpacing((Spacing)UserSettings.Setting.RowSpacing);

        // Details pane
        deetsRow.Height = !UserSettings.Setting.ShowDetails
            ? new GridLength(1)
            : new GridLength(UserSettings.Setting.DetailsHeight);

        // Settings change event
        UserSettings.Setting.PropertyChanged += UserSettingChanged;
    }
    #endregion Settings

    #region Setting change
    /// <summary>
    /// My way of handling changes in UserSettings
    /// </summary>
    /// <param name="sender"></param>
    private void UserSettingChanged(object sender, PropertyChangedEventArgs e)
    {
        PropertyInfo prop = sender.GetType().GetProperty(e.PropertyName);
        object newValue = prop?.GetValue(sender, null);
        log.Debug($"Setting change: {e.PropertyName} New Value: {newValue}");
        switch (e.PropertyName)
        {
            case nameof(UserSettings.Setting.KeepOnTop):
                Topmost = (bool)newValue;
                break;

            case nameof(UserSettings.Setting.IncludeDebug):
                NLHelpers.SetLogLevel((bool)newValue);
                break;

            case nameof(UserSettings.Setting.DarkMode):
                MainWindowUIHelpers.SetBaseTheme((ThemeType)newValue);
                break;

            case nameof(UserSettings.Setting.HideExcluded):
                UpdateGrid();
                break;

            case nameof(UserSettings.Setting.PrimaryColor):
                MainWindowUIHelpers.SetPrimaryColor((AccentColor)newValue);
                break;

            case nameof(UserSettings.Setting.GridFontWeight):
                SetFontWeight((Weight)newValue);
                break;

            case nameof(UserSettings.Setting.RowSpacing):
                SetRowSpacing((Spacing)newValue);
                break;

            case nameof(UserSettings.Setting.ShowDetails):
                if ((bool)newValue)
                {
                    deetsRow.Height = new GridLength(UserSettings.Setting.DetailsHeight);
                    splitter.Visibility = Visibility.Visible;
                }
                else
                {
                    deetsRow.Height = new GridLength(1);
                    splitter.Visibility = Visibility.Collapsed;
                }
                break;

            case nameof(UserSettings.Setting.UISize):
                int size = (int)newValue;
                double newSize = MainWindowUIHelpers.UIScale((MySize)size);
                MainGrid.LayoutTransform = new ScaleTransform(newSize, newSize);
                break;
        }
    }
    #endregion Setting change

    #region Smaller/Larger
    /// <summary>
    /// Scale the UI according to user preference
    /// </summary>
    private void Window_MouseWheel(object sender, MouseWheelEventArgs e)
    {
        if (Keyboard.Modifiers != ModifierKeys.Control)
            return;

        if (e.Delta > 0)
        {
            EverythingLarger();
        }
        else if (e.Delta < 0)
        {
            EverythingSmaller();
        }
    }
    private void DataGrid_PreviewMouseWheel(object sender, MouseWheelEventArgs e)
    {
        if (Keyboard.Modifiers != ModifierKeys.Control)
            return;

        if (e.Delta > 0)
        {
            EverythingLarger();
        }
        else if (e.Delta < 0)
        {
            EverythingSmaller();
        }
    }
    public void EverythingSmaller()
    {
        int size = UserSettings.Setting.UISize;
        if (size > 0)
        {
            size--;
            UserSettings.Setting.UISize = size;
            double newSize = MainWindowUIHelpers.UIScale((MySize)size);
            MainGrid.LayoutTransform = new ScaleTransform(newSize, newSize);
            SnackbarMsg.ClearAndQueueMessage($"Size set to {(MySize)UserSettings.Setting.UISize}");
        }
    }
    public void EverythingLarger()
    {
        int size = UserSettings.Setting.UISize;
        if (size < 4)
        {
            size++;
            UserSettings.Setting.UISize = size;
            double newSize = MainWindowUIHelpers.UIScale((MySize)size);
            MainGrid.LayoutTransform = new ScaleTransform(newSize, newSize);
            SnackbarMsg.ClearAndQueueMessage($"Size set to {(MySize)UserSettings.Setting.UISize}");
        }
    }
    #endregion Smaller/Larger

    #region Window Events
    private void Window_ContentRendered(object sender, EventArgs e)
    {
        Mouse.OverrideCursor = Cursors.Wait;
        GetExcludes();
        GetEventLog();
        GetListOfUpdates();
        PopulateExcludedList();
        WhichList();
        Mouse.OverrideCursor = null;
        _ = dataGrid.Focus();
        dataGrid.SelectedIndex = 0;
        log.Debug($"Total startup time {stopwatch.Elapsed.TotalMilliseconds:N2} milliseconds");
    }

    private void Window_Closing(object sender, CancelEventArgs e)
    {
        stopwatch.Stop();
        log.Info($"{AppInfo.AppName} is shutting down.  Elapsed time: {stopwatch.Elapsed:h\\:mm\\:ss\\.ff}");

        // Shut down NLog
        LogManager.Shutdown();

        // Save settings
        UserSettings.Setting.WindowLeft = Math.Floor(Left);
        UserSettings.Setting.WindowTop = Math.Floor(Top);
        UserSettings.Setting.WindowWidth = Math.Floor(Width);
        UserSettings.Setting.WindowHeight = Math.Floor(Height);
        UserSettings.SaveSettings();
    }
    #endregion Window Events

    #region Keyboard Events
    private void Window_KeyDown(object sender, KeyEventArgs e)
    {
        // CTRL key combos
        if (e.KeyboardDevice.Modifiers == ModifierKeys.Control)
        {
            if (e.Key == Key.D)
            {
                ToggleDetails();
            }
            if (e.Key == Key.E)
            {
                ToggleExcluded();
            }
            if (e.Key == Key.F)
            {
                _ = tbxSearch.Focus();
            }
            if (e.Key == Key.M)
            {
                switch (UserSettings.Setting.DarkMode)
                {
                    case (int)ThemeType.Light:
                        UserSettings.Setting.DarkMode = (int)ThemeType.Dark;
                        break;
                    case (int)ThemeType.Dark:
                        UserSettings.Setting.DarkMode = (int)ThemeType.System;
                        break;
                    case (int)ThemeType.System:
                        UserSettings.Setting.DarkMode = (int)ThemeType.Light;
                        break;
                }
                SnackbarMsg.ClearAndQueueMessage($"Theme set to {(ThemeType)UserSettings.Setting.DarkMode}");
            }
            if (e.Key == Key.N)
            {
                if (UserSettings.Setting.PrimaryColor >= (int)AccentColor.BlueGray)
                {
                    UserSettings.Setting.PrimaryColor = 0;
                }
                else
                {
                    UserSettings.Setting.PrimaryColor++;
                }
                SnackbarMsg.ClearAndQueueMessage($"Accent color set to {(AccentColor)UserSettings.Setting.PrimaryColor}");
            }
            if (e.Key == Key.R)
            {
                ClearColumnSort();
            }
            if (e.Key == Key.Add)
            {
                EverythingLarger();
            }
            if (e.Key == Key.Subtract)
            {
                EverythingSmaller();
            }
            if (e.Key == Key.OemComma)
            {
                if (!DialogHost.IsDialogOpen("MainDialogHost"))
                {
                    DialogHelpers.ShowSettingsDialog();
                }
                else
                {
                    DialogHost.Close("MainDialogHost");
                    DialogHelpers.ShowSettingsDialog();
                }
            }
        }
        // No CTRL key
        if (e.Key == Key.F1)
        {
            if (!DialogHost.IsDialogOpen("MainDialogHost"))
            {
                DialogHelpers.ShowAboutDialog();
            }
            else
            {
                DialogHost.Close("MainDialogHost");
                DialogHelpers.ShowAboutDialog();
            }
        }
    }
    #endregion Keyboard Events

    #region Read the Exclude file
    /// <summary>
    ///  REad the JSON file containing the exclude items
    /// </summary>
    private static void GetExcludes()
    {
        Stopwatch rxsw = new();
        rxsw.Start();
        string json = File.ReadAllText(GetExcludesFile());
        ExcludedItems.ExcludedStrings = JsonSerializer.Deserialize<List<ExcludedItems>>(json);
        rxsw.Stop();
        int xCount = ExcludedItems.ExcludedStrings.Count;
        string xRecs;
        if (xCount > 0)
        {
            foreach (ExcludedItems item in ExcludedItems.ExcludedStrings)
            {
                log.Info($"Excluding updates containing: \"{item.ExcludedString}\"");
            }
        }
        xRecs = xCount == 1 ? "record" : "records";
        log.Debug($"Read {ExcludedItems.ExcludedStrings.Count} exclude {xRecs} from disk in {rxsw.Elapsed.TotalMilliseconds:N2} milliseconds");
    }
    #endregion Read the Exclude file

    #region Get the exclude file name
    /// <summary>
    /// Determine the full path for the exclude file. Create it if it doesn't exist.
    /// </summary>
    /// <returns>Full path to exclude file</returns>
    private static string GetExcludesFile()
    {
        string filePath = Path.Combine(AppInfo.AppDirectory, "WUViewExcludes.json");
        if (!File.Exists(filePath))
        {
            File.Create(filePath).Dispose();
            const string braces = "[{ \"ExcludedString\": \"Defender\"}]";
            File.WriteAllText(filePath, braces);
        }
        return filePath;
    }
    #endregion Get the exclude file name

    #region Get Event Log details
    /// <summary>
    /// Gets all "Setup" records from the Event Log
    /// </summary>
    private void GetEventLog()
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
                    eventLogRecords.Add(eventdetail);
                }
            }
        }
        catch (EventLogNotFoundException e)
        {
            log.Error($"Error while reading the event logs\n{e.Message}");
        }
        swe.Stop();
        log.Debug($"Read {eventLogRecords.Count} Setup event log records in {swe.Elapsed.TotalMilliseconds:N2} milliseconds");
    }
    #endregion Get Event Log details

    #region Get the list of Windows updates
    /// <summary>
    /// Gets list of Windows Updates
    /// </summary>
    private void GetListOfUpdates()
    {
        Stopwatch sw = new();
        sw.Start();
        UpdateSession updateSession = new();
        IUpdateSearcher updateSearcher = updateSession.CreateUpdateSearcher();
        int count = updateSearcher.GetTotalHistoryCount();
        log.Debug($"Read {count} Windows Update records in {sw.Elapsed.TotalMilliseconds:N2} milliseconds");
        sw.Restart();
        // Believe it or not, it's possible to not have any updates.
        if (count > 0)
        {
            Stopwatch gkbsw = new();
            Stopwatch updsw = new();
            foreach (IUpdateHistoryEntry x in updateSearcher.QueryHistory(0, count))
            {
                gkbsw.Start();
                string kbNum = GetKB(x.Title);
                gkbsw.Stop();
                updsw.Start();
                WUpdate update = new()
                {
                    Title = x.Title,
                    KBNum = kbNum,
                    Date = x.Date.ToLocalTime(),
                    ResultCode = x.ResultCode.ToString(),
                    HResult = x.HResult.ToString(),
                    Operation = x.Operation.ToString(),
                    UpdateID = x.UpdateIdentity.UpdateID,
                    Description = x.Description,
                    SupportURL = x.SupportUrl,
                    ELDescription = FindEventLogs(kbNum)
                };
                updatesFullList.Add(update);
                updsw.Stop();
                if (x.HResult != 0)
                {
                    log.Warn($"KB:{update.KBNum,-10} Date: {update.Date,-23} HResult: {update.HResult,-10} " +
                             $" Operation: {update.Operation,-12}  UpdateID: {update.UpdateID}");
                }
            }
            log.Debug($"Extracting KB numbers took {gkbsw.Elapsed.TotalMilliseconds:N2} milliseconds");
            log.Debug($"Building WUpdate object took {updsw.Elapsed.TotalMilliseconds:N2} milliseconds");
        }
        else
        {
            log.Info($"No updates found! IUpdateSearcher.GetTotalHistoryCount returned {count}.");
            _ = MessageBox.Show("No updates were found", "WUView",
                MessageBoxButton.OK,
                MessageBoxImage.Information);
        }
        log.Debug($"Building the list of updates took {sw.Elapsed.TotalMilliseconds:N2} milliseconds");
    }
    #endregion Get the list of Windows updates

    #region Match event logs to update items
    /// <summary>
    /// Matches up any Event Log records with a Windows Update bases on KB number
    /// </summary>
    /// <param name="kb"></param>
    /// <returns>Returns a string containing relevant Event Log records or a message saying that none could be found</returns>
    private string FindEventLogs(string kb)
    {
        if (kb == "n/a")
        {
            return "Event logs records cannot be located without a \"KB\" number.";
        }
        StringBuilder sbEventLog = new();
        foreach (EventRecord item in eventLogRecords)
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
    private void PopulateExcludedList()
    {
        Stopwatch esw = new();
        esw.Start();
        updatesWithoutExcludesList.Clear();
        foreach (WUpdate upd in updatesFullList)
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
                updatesWithoutExcludesList.Add(upd.GetClone());
            }
        }
        esw.Stop();
        log.Debug($"Removing excluded items took {esw.Elapsed.TotalMilliseconds:N2} milliseconds");
    }
    #endregion Remove excluded items and create list without excludes

    #region Full list or list without excludes
    /// <summary>
    /// Determines which list to use
    /// </summary>
    private void WhichList()
    {
        Stopwatch wsw = new();
        wsw.Start();
        if (UserSettings.Setting.HideExcluded)
        {
            dataGrid.ItemsSource = updatesWithoutExcludesList;
        }
        else
        {
            dataGrid.ItemsSource = updatesFullList;
        }
        dataGrid.Items.Refresh();
        int cnt = dataGrid.Items.Count;
        string msg = string.Format($"Displaying {cnt} of {updatesFullList.Count} updates");
        log.Info(msg);
        SnackbarMsg.ClearAndQueueMessage(msg, 2000);
        wsw.Stop();
        log.Debug($"Loading data grid took {wsw.Elapsed.TotalMilliseconds:N2} milliseconds");
    }
    #endregion Full list or list without excludes

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

    #region Set the row spacing
    /// <summary>
    /// Sets the padding around the rows in the datagrid
    /// </summary>
    /// <param name="spacing"></param>
    public void SetRowSpacing(Spacing spacing)
    {
        switch (spacing)
        {
            case Spacing.Compact:
                DataGridAssist.SetCellPadding(dataGrid, new Thickness(15, 2, 15, 2));
                break;
            case Spacing.Comfortable:
                DataGridAssist.SetCellPadding(dataGrid, new Thickness(15, 4, 15, 4));
                break;
            case Spacing.Wide:
                DataGridAssist.SetCellPadding(dataGrid, new Thickness(15, 8, 15, 8));
                break;
        }
    }
    #endregion Set the row spacing

    #region Set the font weight
    /// <summary>
    /// Sets the weight of the font in the main window
    /// </summary>
    /// <param name="weight"></param>
    public void SetFontWeight(Weight weight)
    {
        switch (weight)
        {
            case Weight.Thin:
                MainDH.FontWeight = FontWeights.Thin;
                break;
            case Weight.Regular:
                MainDH.FontWeight = FontWeights.Regular;
                break;
            case Weight.SemiBold:
                MainDH.FontWeight = FontWeights.SemiBold;
                break;
            case Weight.Bold:
                MainDH.FontWeight = FontWeights.Bold;
                break;
            default:
                MainDH.FontWeight = FontWeights.Regular;
                break;
        }
    }
    #endregion Set the font weight

    #region Edit exclude file
    /// <summary>
    /// Edit the exclude file
    /// </summary>
    private async void EditExcludes()
    {
        bool r;
        if (!DialogHost.IsDialogOpen("MainDialogHost"))
        {
            r = await DialogHelpers.ShowEditExcludesDialog();
        }
        else
        {
            DialogHost.Close("MainDialogHost");
            r = await DialogHelpers.ShowEditExcludesDialog();
        }

        if (r)
        {
            JsonSerializerOptions opts = new()
            {
                WriteIndented = true
            };
            string json = JsonSerializer.Serialize(ExcludedItems.ExcludedStrings, opts);
            File.WriteAllText(GetExcludesFile(), json);
            PopulateExcludedList();
            UpdateGrid();
            foreach (ExcludedItems item in ExcludedItems.ExcludedStrings)
            {
                log.Info($"Excluding updates containing: \"{item.ExcludedString}\"");
            }
        }
    }
    #endregion Edit exclude file

    #region Update the grid
    /// <summary>
    /// Update the datagrid after changes have occurred
    /// </summary>
    private void UpdateGrid()
    {
        Mouse.OverrideCursor = Cursors.Wait;
        WhichList();
        dataGrid.Items.Refresh();
        Mouse.OverrideCursor = null;
    }
    #endregion Update the grid

    #region GridSplitter drag event
    /// <summary>
    /// Save detail pane height after a GridSplitter drag event
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    private void GridSplitter_DragCompleted(object sender, DragCompletedEventArgs e)
    {
        UserSettings.Setting.DetailsHeight = Math.Floor(deetsRow.Height.Value);
    }
    #endregion GridSplitter drag event

    #region HResult click event
    /// <summary>
    /// Put HResult in clipboard and open Windows Update error code site
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    private void HResult_PreviewMouseDown(object sender, MouseButtonEventArgs e)
    {
        if (hypHResult.Inlines.FirstInline is Run run)
        {
            Clipboard.SetText(run.Text);
        }
        log.Debug($"Opening {UserSettings.Setting.ResultCodeUrl}");
        Process p = new();
        p.StartInfo.FileName = UserSettings.Setting.ResultCodeUrl;
        p.StartInfo.UseShellExecute = true;
        p.Start();
        e.Handled = true;
    }
    #endregion HResult click event

    #region URL click event
    /// <summary>
    /// Navigate to website
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    private void Hyperlink_RequestNavigate(object sender, RequestNavigateEventArgs e)
    {
        log.Debug($"Opening {e.Uri.AbsoluteUri}");
        if (!string.IsNullOrWhiteSpace(e.Uri.AbsoluteUri))
        {
            Process p = new();
            p.StartInfo.FileName = e.Uri.AbsoluteUri;
            p.StartInfo.UseShellExecute = true;
            p.Start();
            e.Handled = true;
        }
    }
    #endregion URL click event

    #region Menu events
    private void MnuCopy_Click(object sender, RoutedEventArgs e)
    {
        Copy2Clipboard();
    }

    private void MnuSaveToCsv_Click(object sender, RoutedEventArgs e)
    {
        SaveToCSV();
    }

    private void MnuLarger_Click(object sender, RoutedEventArgs e)
    {
        EverythingLarger();
    }

    private void MnuSmaller_Click(object sender, RoutedEventArgs e)
    {
        EverythingSmaller();
    }

    private void MnuRemoveSort_Click(object sender, RoutedEventArgs e)
    {
        ClearColumnSort();
    }
    private void MnuToggleExclude_Click(object sender, RoutedEventArgs e)
    {
        ToggleExcluded();
    }
    private void MnuSaveToText_Click(object sender, RoutedEventArgs e)
    {
        SaveToFile();
    }

    private void MnuToggleDetails_Click(object sender, RoutedEventArgs e)
    {
        ToggleDetails();
    }
    #endregion Menu events

    #region Button events
    private void BtnLog_Click(object sender, RoutedEventArgs e)
    {
        TextFileViewer.ViewTextFile(NLHelpers.GetLogfileName());
    }

    private void BtnReadme_Click(object sender, RoutedEventArgs e)
    {
        string dir = AppInfo.AppDirectory;
        TextFileViewer.ViewTextFile(Path.Combine(dir, "ReadMe.txt"));
    }

    private void BtnExcludes_Click(object sender, RoutedEventArgs e)
    {
        TextFileViewer.ViewTextFile(GetExcludesFile());
    }
    #endregion Button events

    #region Clear column sort
    /// <summary>
    ///  Clears any sorts that may have been applied to columns in the datagrid
    /// </summary>
    private void ClearColumnSort()
    {
        foreach (DataGridColumn column in dataGrid.Columns)
        {
            column.SortDirection = null;
        }
        dataGrid.Items.SortDescriptions.Clear();

        SnackbarMsg.ClearAndQueueMessage("Column sort cleared", 1000);
    }
    #endregion Clear column sort

    #region Filter textbox changed event
    /// <summary>
    /// Used by the "filter" textbox at the top
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    private void TbxSearch_TextChanged(object sender, TextChangedEventArgs e)
    {
        FilterTheGrid();
    }

    private void FilterTheGrid()
    {
        string filter = tbxSearch.Text;

        ICollectionView cv = CollectionViewSource.GetDefaultView(dataGrid.ItemsSource);
        if (filter?.Length == 0)
        {
            cv.Filter = null;
            SnackbarMsg.ClearAndQueueMessage("Showing all rows", 2000);
        }
        else
        {
            cv.Filter = o =>
            {
                WUpdate wu = o as WUpdate;
                return wu.Title.Contains(filter, StringComparison.OrdinalIgnoreCase) ||
                       wu.ResultCode.Contains(filter, StringComparison.OrdinalIgnoreCase) ||
                       wu.KBNum.Contains(filter, StringComparison.OrdinalIgnoreCase);
            };
            if (dataGrid.Items.Count == 1)
            {
                SnackbarMsg.ClearAndQueueMessage("1 row shown", 1000);
            }
            else
            {
                SnackbarMsg.ClearAndQueueMessage($"{dataGrid.Items.Count} rows shown", 1000);
            }
        }
    }
    #endregion Filter textbox changed event

    #region Navigation
    private void NavListBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
    {
        NavigateToPage((NavPage)NavListBox.SelectedIndex);
        NavListBox.SelectedItem = null;
    }

    private void NavigateToPage(NavPage selectedIndex)
    {
        switch (selectedIndex)
        {
            case NavPage.Viewer:
                NavDrawer.IsLeftDrawerOpen = false;
                break;

            case NavPage.EditList:
                EditExcludes();
                NavDrawer.IsLeftDrawerOpen = false;
                break;

            case NavPage.Settings:
                NavDrawer.IsLeftDrawerOpen = false;
                DialogHelpers.ShowSettingsDialog();
                break;

            case NavPage.OpenWU:
                NavDrawer.IsLeftDrawerOpen = false;
                OpenWindowsUpdate();
                break;

            case NavPage.OpenEV:
                NavDrawer.IsLeftDrawerOpen = false;
                OpenEventViewer();
                break;

            case NavPage.About:
                NavDrawer.IsLeftDrawerOpen = false;
                DialogHelpers.ShowAboutDialog();
                break;

            case NavPage.Exit:
                Application.Current.Shutdown();
                break;
        }
    }
    #endregion Navigation

    #region Save details to a text file
    private void SaveToFile()
    {
        string fname = "WUView_" + DateTime.Now.Date.ToString("yyyy-MM-dd") + ".txt";
        SaveFileDialog dialog = new()
        {
            Title = "Save Details as Text File",
            Filter = "Text File|*.txt",
            InitialDirectory = Environment.GetFolderPath(Environment.SpecialFolder.Desktop),
            FileName = fname
        };
        bool? result = dialog.ShowDialog();
        if (result == true)
        {
            StringBuilder sb = new();
            _ = sb.Append("Windows Update details for ").Append(Environment.MachineName)
                .Append(" - ").AppendFormat("{0:G}", DateTime.Now).AppendLine();
            string uscore = new('-', sb.Length - 2);
            _ = sb.Append(uscore).AppendLine("\r\n");

            List<WUpdate> listInUse = updatesFullList;
            if (UserSettings.Setting.HideExcluded)
            {
                listInUse = updatesWithoutExcludesList;
            }

            for (int i = 0; i < listInUse.Count; i++)
            {
                _ = sb.Append("Title:        ").AppendLine(listInUse[i].Title)
                    .AppendFormat("Date:         {0:G}\n", listInUse[i].Date)
                    .Append("KB Number:    ").AppendLine(listInUse[i].KBNum)
                    .Append("Operation:    ").AppendLine(listInUse[i].Operation)
                    .Append("Result Code:  ").AppendLine(listInUse[i].ResultCode)
                    .Append("HResult:      ").AppendLine(listInUse[i].HResult)
                    .Append("Update ID:    ").AppendLine(listInUse[i].UpdateID)
                    .Append("Support URL:  ").AppendLine(listInUse[i].SupportURL)
                    .Append("Description:  ").AppendLine(listInUse[i].Description);

                foreach (string line in Regex.Split(FindEventLogs(listInUse[i].KBNum), "\r\n|\r|\n"))
                {
                    if (!string.IsNullOrWhiteSpace(line))
                    {
                        _ = sb.Append("Event Log:    ").AppendLine(line);
                    }
                }
                _ = sb.AppendLine("\r\n");
            }
            File.WriteAllText(dialog.FileName, sb.ToString());
            _ = sb.Clear();
            log.Debug($"Details written to {dialog.FileName}");
        }
    }
    #endregion Save details to a text file

    #region Save grid to CSV file
    private void SaveToCSV()
    {
        string fname = "WUView_" + DateTime.Now.Date.ToString("yyyy-MM-dd") + ".csv";
        SaveFileDialog dialog = new()
        {
            Title = "Save Grid as CSV FIle",
            Filter = "CSV File|*.csv",
            InitialDirectory = Environment.GetFolderPath(Environment.SpecialFolder.Desktop),
            FileName = fname
        };
        bool? result = dialog.ShowDialog();
        if (result == true)
        {
            Copy2Clipboard();
            string gridData = (string)Clipboard.GetData(DataFormats.CommaSeparatedValue);
            File.WriteAllText(dialog.FileName, gridData, Encoding.UTF8);
        }
    }
    #endregion Save grid to CSV file

    #region Copy to clipboard
    /// <summary>
    /// Copies the present contents of the datagrid to the clipboard
    /// </summary>
    private void Copy2Clipboard()
    {
        // Preserve the selected row
        int selIndx = dataGrid.SelectedIndex;

        // Clear the clipboard
        Clipboard.Clear();

        // Include the header row
        dataGrid.ClipboardCopyMode = DataGridClipboardCopyMode.IncludeHeader;

        // Temporarily set selection mode to all rows
        dataGrid.SelectionMode = DataGridSelectionMode.Extended;

        // Select all the cells
        dataGrid.SelectAllCells();

        // Execute the copy
        ApplicationCommands.Copy.Execute(null, dataGrid);

        // Unselect the cells
        dataGrid.UnselectAllCells();

        // Set selection mode back to one row
        dataGrid.SelectionMode = DataGridSelectionMode.Single;

        // re-select the previous row
        dataGrid.SelectedIndex = selIndx;

        SnackbarMsg.ClearAndQueueMessage("Copied", 500);
    }
    #endregion Copy to clipboard

    #region Toggle excluded items / details pane
    private static void ToggleExcluded()
    {
        UserSettings.Setting.HideExcluded = !UserSettings.Setting.HideExcluded;
    }
    private static void ToggleDetails()
    {
        UserSettings.Setting.ShowDetails = !UserSettings.Setting.ShowDetails;
    }
    #endregion Toggle excluded items / details pane

    #region Open other applications
    private static void OpenEventViewer()
    {
        using Process procEV = new();
        procEV.StartInfo.FileName = "MMC.exe";
        procEV.StartInfo.Arguments = "Eventvwr.msc";
        procEV.StartInfo.UseShellExecute = true;
        _ = procEV.Start();
        log.Debug("Launching Event Viewer");
    }

    private static void OpenWindowsUpdate()
    {
        using Process procWU = new();
        procWU.StartInfo.FileName = "ms-settings:windowsupdate";
        procWU.StartInfo.UseShellExecute = true;
        _ = procWU.Start();
        log.Debug("Launching Windows Update");
    }
    #endregion Open other applications

    #region Unhandled Exception Handler
    /// <summary>
    /// Handles any exceptions that weren't caught by a try-catch statement
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="args"></param>
    private static void CurrentDomain_UnhandledException(object sender, UnhandledExceptionEventArgs args)
    {
        log.Error("Unhandled Exception");
        Exception e = (Exception)args.ExceptionObject;
        log.Error(e.Message);
        if (e.InnerException != null)
        {
            log.Error(e.InnerException.ToString());
        }
        log.Error(e.StackTrace);

        _ = new MDCustMsgBox("An error has occurred. See the log file",
            "DailyDocuments Error", ButtonType.Ok).ShowDialog();
    }
    #endregion Unhandled Exception Handler
}
