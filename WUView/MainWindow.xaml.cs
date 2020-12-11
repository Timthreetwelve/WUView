// Copyright (c) Tim Kennedy. All Rights Reserved. Licensed under the MIT License.

#region Using directives
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Configuration;
using System.Diagnostics;
using System.Diagnostics.Eventing.Reader;
using System.IO;
using System.Security.Principal;
using System.Text;
using System.Text.RegularExpressions;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using Microsoft.Win32;
using Newtonsoft.Json;
using NLog;
using NLog.Targets;
using TKUtils;
using WUApiLib;
using WUView.Properties;
#endregion

namespace WUView
{
    public partial class MainWindow : Window
    {
        #region NLog Instance
        private static readonly Logger log = LogManager.GetCurrentClassLogger();
        #endregion NLog Instance

        #region Event record and WUpdate Lists
        private readonly List<EventRecord> eventLogRecords = new List<EventRecord>();
        private readonly List<WUpdate> updatesFullList = new List<WUpdate>();
        private readonly List<WUpdate> updatesWithoutExcludesList = new List<WUpdate>();
        #endregion Event record and WUpdate Lists

        #region Color Constants
        private const string bgColorBlue = "#FFF0F8FF";
        private const string bgColorGray = "#FFEAEAEA";
        private const string bgColorGreen = "#FFEAFDE1";
        private const string bgColorYellow = "#FFFFF8DC";
        #endregion Color Constants

        #region Main Stopwatch
        private readonly Stopwatch mainsw = new Stopwatch();
        #endregion

        public MainWindow()
        {
            InitializeComponent();
            ReadSettings();
        }

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
            mainsw.Stop();
            log.Debug($"Total startup time {mainsw.Elapsed.TotalMilliseconds:N2} milliseconds");
        }

        #region Read Settings
        private void ReadSettings()
        {
            mainsw.Start();

            // Change the log file filename when debugging
            string env = Debugger.IsAttached ? "debug" : "temp";
            GlobalDiagnosticsContext.Set("TempOrDebug", env);

            // Startup message in the temp file
            log.Info($"{AppInfo.AppName} {AppInfo.TitleVersion} is starting up");

            // NLog logging level
            LogManager.Configuration.Variables["logLev"] = Settings.Default.VerboseLogging ? "Debug" : "Info";
            LogManager.ReconfigExistingLoggers();

            // Unhandled exception handler
            AppDomain.CurrentDomain.UnhandledException += CurrentDomain_UnhandledException;

            // Settings upgrade
            if (Settings.Default.SettingsUpgradeRequired)
            {
                Settings.Default.Upgrade();
                Settings.Default.SettingsUpgradeRequired = false;
                Settings.Default.Save();
                CleanUp.CleanupPrevSettings();
            }

            // Settings change event
            Settings.Default.SettingChanging += SettingChanging;

            // Window position & Size
            Top = Settings.Default.WindowTop;
            Left = Settings.Default.WindowLeft;
            Height = Settings.Default.WindowHeight;
            Width = Settings.Default.WindowWidth;
            MaxHeight = SystemParameters.MaximizedPrimaryScreenHeight;
            MaxWidth = SystemParameters.MaximizedPrimaryScreenWidth;

            // Set Datagrid zoom
            double curZoom = Settings.Default.GridZoom;
            dataGrid.LayoutTransform = new ScaleTransform(curZoom, curZoom);
            sc1.LayoutTransform = new ScaleTransform(curZoom, curZoom);

            // Put version number in window title
            WindowTitleVersionAdmin();

            // Alternate row shading
            if (Settings.Default.ShadeAltRows)
            {
                AltRowShadingOn();
            }

            // Show grid lines
            if (!Settings.Default.ShowGridLines)
            {
                dataGrid.GridLinesVisibility = DataGridGridLinesVisibility.None;
            }

            // Details pane
            if (!Settings.Default.ShowDetails)
            {
                bottomGrid.Visibility = Visibility.Collapsed;
            }
            if (Settings.Default.ShowDetails && Settings.Default.DetailsHeight == 0)
            {
                Settings.Default.DetailsHeight = 250;
            }

            //Details background menu selection
            switch (Settings.Default.DetailsBackground)
            {
                case bgColorBlue:
                    mnuBlue.IsChecked = true;
                    break;
                case bgColorGray:
                    mnuGray.IsChecked = true;
                    break;
                case bgColorGreen:
                    mnuGreen.IsChecked = true;
                    break;
                case bgColorYellow:
                    mnuYellow.IsChecked = true;
                    break;
                default:
                    log.Info($"Unknown value for DetailsBackground: {Settings.Default.DetailsBackground}");
                    break;
            }
            log.Debug($"Read settings took {mainsw.Elapsed.TotalMilliseconds:N2} milliseconds");
            tb1.Text = "Loading...";
        }
        #endregion Read Settings

        #region Menu Events
        private void MnuExit_Click(object sender, RoutedEventArgs e)
        {
            Application.Current.Shutdown();
        }

        private void MnuCopy_Click(object sender, RoutedEventArgs e)
        {
            Copy2Clipboard();
        }

        private void MnuSaveText_Click(object sender, RoutedEventArgs e)
        {
            SaveToFile();
        }

        private void MnuSaveToCsv_Click(object sender, RoutedEventArgs e)
        {
            SaveToCSV();
        }

        private void OpenWU_Click(object sender, RoutedEventArgs e)
        {
            using (Process procWU = new Process())
            {
                procWU.StartInfo.FileName = "ms-settings:windowsupdate";
                _ = procWU.Start();
                log.Debug("Launching Windows Update");
            }
        }

        private void OpenEV_Click(object sender, RoutedEventArgs e)
        {
            using (Process procEV = new Process())
            {
                procEV.StartInfo.FileName = "MMC.exe";
                procEV.StartInfo.Arguments = "Eventvwr.msc";
                _ = procEV.Start();
                log.Debug("Launching Event Viewer");
            }
        }

        private void MnuAbout_Click(object sender, RoutedEventArgs e)
        {
            About about = new About
            {
                Owner = Application.Current.MainWindow,
                WindowStartupLocation = WindowStartupLocation.CenterOwner
            };
            _ = about.ShowDialog();
        }

        private void MnuHideExcluded_Click(object sender, RoutedEventArgs e)
        {
            if (IsLoaded)
            {
                Mouse.OverrideCursor = Cursors.Wait;
                UpdateGrid();
                Mouse.OverrideCursor = null;
            }
        }

        private void Font_Click(object sender, RoutedEventArgs e)
        {
            FontSelector fs = new FontSelector
            {
                Owner = this
            };
            fs.Show();
        }

        private void MnuEditExcludes_Click(object sender, RoutedEventArgs e)
        {
            EditExcludes();
        }

        private void EditExcludes()
        {
            Excludes excl = new Excludes
            {
                Owner = this
            };
            bool? r = excl.ShowDialog();
            if (r == true)
            {
                string json = JsonConvert.SerializeObject(ExcludedItems.ExcludedStrings, Formatting.Indented);
                File.WriteAllText(GetJsonFile(), json);
                PopulateExcludedList();
                UpdateGrid();
                foreach (ExcludedItems item in ExcludedItems.ExcludedStrings)
                {
                    log.Info($"Excluding updates containing: \"{item.ExcludedString}\"");
                }
            }
        }

        private void MnuLookUp_Click(object sender, RoutedEventArgs e)
        {
            string url = Settings.Default.ResultCodeUrl;
            try
            {
                using (Process p = new Process())
                {
                    p.StartInfo.FileName = url;
                    _ = p.Start();
                }
            }
            catch (Exception ex)
            {
                _ = MessageBox.Show(
                    $"Could not open browser\n{ex}",
                    "WUView",
                    MessageBoxButton.OK,
                    MessageBoxImage.Error);
            }
        }

        private void MnuViewExcl_Click(object sender, RoutedEventArgs e)
        {
            TextFileViewer.ViewTextFile(GetJsonFile());
        }

        private void MnuReadme_Click(object sender, RoutedEventArgs e)
        {
            TextFileViewer.ViewTextFile(Path.Combine(AppInfo.AppDirectory, "ReadMe.txt"));
        }

        private void DetailBG_Checked(object sender, RoutedEventArgs e)
        {
            var mi = (MenuItem)e.OriginalSource;
            switch (mi.Header)
            {
                case "Blue":
                    mnuGreen.IsChecked = false;
                    mnuGray.IsChecked = false;
                    mnuYellow.IsChecked = false;
                    Settings.Default.DetailsBackground = "#FFF0F8FF";
                    break;
                case "Gray":
                    mnuBlue.IsChecked = false;
                    mnuGreen.IsChecked = false;
                    mnuYellow.IsChecked = false;
                    Settings.Default.DetailsBackground = "#FFEAEAEA";
                    break;
                case "Green":
                    mnuBlue.IsChecked = false;
                    mnuGray.IsChecked = false;
                    mnuYellow.IsChecked = false;
                    Settings.Default.DetailsBackground = "#FFEAFDE1";
                    break;
                case "Yellow":
                    mnuBlue.IsChecked = false;
                    mnuGreen.IsChecked = false;
                    mnuGray.IsChecked = false;
                    Settings.Default.DetailsBackground = "#FFFFF8DC";
                    break;
            }
        }
        #endregion Menu Events

        #region Window Events
        private void Window_Closing(object sender, CancelEventArgs e)
        {
            log.Info("{0} is shutting down.", AppInfo.AppName);

            // Shut down NLog
            LogManager.Shutdown();

            // save the property settings
            Settings.Default.WindowLeft = Left;
            Settings.Default.WindowTop = Top;
            Settings.Default.WindowHeight = Height;
            Settings.Default.WindowWidth = Width;
            Settings.Default.Save();
        }

        private void GridSplitter_DragCompleted(object sender, System.Windows.Controls.Primitives.DragCompletedEventArgs e)
        {
            Settings.Default.DetailsHeight = deetsRow.Height.Value;
        }
        #endregion Window Events

        #region Mouse Events
        private void DataGrid_PreviewMouseWheel(object sender, MouseWheelEventArgs e)
        {
            if (Keyboard.Modifiers != ModifierKeys.Control)
                return;

            if (e.Delta > 0)
            {
                GridLarger();
            }
            else if (e.Delta < 0)
            {
                GridSmaller();
            }
        }

        private void GridSmaller_Click(object sender, RoutedEventArgs e)
        {
            GridSmaller();
        }

        private void GridLarger_Click(object sender, RoutedEventArgs e)
        {
            GridLarger();
        }

        private void GridReset_Click(object sender, RoutedEventArgs e)
        {
            GridSizeReset();
        }

        private void ViewTemp_Click(object sender, RoutedEventArgs e)
        {
            TextFileViewer.ViewTextFile(GetTempLogFile());
        }

        #endregion Mouse Events

        #region Mouse click on HResult
        private void HResult_PreviewMouseDown(object sender, MouseButtonEventArgs e)
        {
            if (hypHResult.Inlines.FirstInline is Run run)
            {
                Clipboard.SetText(run.Text);
                Debug.WriteLine($"Setting clipboard to {run.Text}");
            }
        }
        #endregion Mouse click on HResult

        #region Keyboard Events
        private void Window_KeyDown(object sender, KeyEventArgs e)
        {
            Debug.WriteLine(e.Key);
            if (e.Key == Key.NumPad0 && (Keyboard.Modifiers & ModifierKeys.Control) != 0)
            {
                GridSizeReset();
            }

            if (e.Key == Key.Add && (Keyboard.Modifiers & ModifierKeys.Control) != 0)
            {
                GridLarger();
            }

            if (e.Key == Key.Subtract && (Keyboard.Modifiers & ModifierKeys.Control) != 0)
            {
                GridSmaller();
            }

            if (e.Key == Key.C && (Keyboard.Modifiers & ModifierKeys.Control) != 0
                               && (Keyboard.Modifiers & ModifierKeys.Shift) != 0)
            {
                Copy2Clipboard();
            }

            if (e.Key == Key.E && (Keyboard.Modifiers & ModifierKeys.Control) != 0)
            {
                EditExcludes();
            }

            if (e.Key == Key.H && (Keyboard.Modifiers & ModifierKeys.Control) != 0)
            {
                mnuHideExcluded.IsChecked = !mnuHideExcluded.IsChecked;
            }

            if (e.Key == Key.S && (Keyboard.Modifiers & ModifierKeys.Control) != 0)
            {
                if ((Keyboard.Modifiers & ModifierKeys.Shift) != 0)
                {
                    SaveToCSV();
                }
                else
                {
                    SaveToFile();
                }
            }

            if (e.Key == Key.F1)
            {
                About about = new About
                {
                    Owner = Application.Current.MainWindow,
                    WindowStartupLocation = WindowStartupLocation.CenterOwner
                };
                _ = about.ShowDialog();
            }

            if (e.Key == Key.F5)
            {
                UpdateGrid();
            }
        }
        #endregion Keyboard Events

        #region Setting change
        private void SettingChanging(object sender, SettingChangingEventArgs e)
        {
            switch (e.SettingName)
            {
                case "ShadeAltRows":
                    {
                        if ((bool)e.NewValue)
                        {
                            AltRowShadingOn();
                        }
                        else
                        {
                            AltRowShadingOff();
                        }
                        break;
                    }
                case "ShowGridLines":
                    {
                        if ((bool)e.NewValue)
                        {
                            dataGrid.GridLinesVisibility = DataGridGridLinesVisibility.All;
                        }
                        else
                        {
                            dataGrid.GridLinesVisibility = DataGridGridLinesVisibility.None;
                        }
                        break;
                    }
                case "ShowDetails":
                    {
                        if ((bool)e.NewValue)
                        {
                            bottomGrid.Visibility = Visibility.Visible;
                            deetsRow.Height = new GridLength(Settings.Default.DetailsHeight);
                        }
                        else
                        {
                            bottomGrid.Visibility = Visibility.Collapsed;
                            deetsRow.Height = new GridLength(0);
                        }
                        break;
                    }
                case "VerboseLogging":
                    {
                        if ((bool)e.NewValue)
                        {
                            LogManager.Configuration.Variables["logLev"] = "Debug";
                        }
                        else
                        {
                            LogManager.Configuration.Variables["logLev"] = "Info";
                        }
                        LogManager.ReconfigExistingLoggers();
                        break;
                    }
            }
            if (IsLoaded)
            {
                log.Debug($"Setting change: {e.SettingName} - New Value: {e.NewValue}");
            }
        }
        #endregion Setting change

        #region Unhandled Exception Handler
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
        }
        #endregion Unhandled Exception Handler

        #region Window Title
        public void WindowTitleVersionAdmin()
        {
            // Set the windows title
            if (IsAdministrator())
            {
                Title = "Windows Update Viewer - " + AppInfo.TitleVersion + " - (Administrator)";
            }
            else
            {
                Title = "Windows Update Viewer - " + AppInfo.TitleVersion;
            }
        }

        public static bool IsAdministrator()
        {
            return new WindowsPrincipal(WindowsIdentity.GetCurrent()).IsInRole(WindowsBuiltInRole.Administrator);
        }
        #endregion Window Title

        #region Get the list of Windows updates
        private void GetListOfUpdates()
        {
            Stopwatch sw = new Stopwatch();
            sw.Start();
            UpdateSession updateSession = new UpdateSession();
            IUpdateSearcher updateSearcher = updateSession.CreateUpdateSearcher();
            int count = updateSearcher.GetTotalHistoryCount();
            log.Debug($"Read {count} Windows Update records in {sw.Elapsed.TotalMilliseconds:N2} milliseconds");
            sw.Restart();
            // Believe it or not, it's possible to not have any updates.
            if (count > 0)
            {
                Stopwatch gkbsw = new Stopwatch();
                Stopwatch updsw = new Stopwatch();
                foreach (IUpdateHistoryEntry x in updateSearcher.QueryHistory(0, count))
                {
                    gkbsw.Start();
                    string kbNum = GetKB(x.Title);
                    gkbsw.Stop();
                    updsw.Start();
                    WUpdate update = new WUpdate
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
        private string FindEventLogs(string kb)
        {
            if (kb == "n/a")
            {
                return string.Empty;
            }
            StringBuilder sbEventLog = new StringBuilder();
            foreach (var item in eventLogRecords)
            {
                if (item.Properties[0].Value.ToString().Contains(kb))
                //if (item.FormatDescription().Contains(kb))  This took forever
                {
                    string tc = string.Format($"{item.TimeCreated} - {item.FormatDescription()}  Event ID: {item.Id}.");
                    _ = sbEventLog.AppendLine(tc);
                }
            }
            return sbEventLog.ToString();
        }
        #endregion Match event logs to update items

        #region Remove excluded items and create list without excludes
        private void PopulateExcludedList()
        {
            Stopwatch esw = new Stopwatch();
            esw.Start();
            updatesWithoutExcludesList.Clear();
            foreach (WUpdate upd in updatesFullList)
            {
                bool skip = false;
                foreach (ExcludedItems exc in ExcludedItems.ExcludedStrings)
                {
                    if (upd.Title.Contains(exc.ExcludedString) && !skip)
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
        private void WhichList()
        {
            Stopwatch wsw = new Stopwatch();
            wsw.Start();
            if (Settings.Default.HideExcluded)
            {
                dataGrid.ItemsSource = updatesWithoutExcludesList;
            }
            else
            {
                dataGrid.ItemsSource = updatesFullList;
            }
            dataGrid.Items.Refresh();
            int cnt = dataGrid.Items.Count;
            tb1.Text = string.Format($"Displaying {cnt} of {updatesFullList.Count} updates");
            log.Info(tb1.Text);
            wsw.Stop();
            log.Debug($"Loading data grid took {wsw.Elapsed.TotalMilliseconds:N2} milliseconds");
        }
        #endregion Full list or list without excludes

        #region Get Event Log details
        private void GetEventLog()
        {
            Stopwatch swe = new Stopwatch();
            swe.Start();
            const string query = "*[System/Provider/@Name=\"Microsoft-Windows-Servicing\"]";
            EventLogQuery eventsQuery = new EventLogQuery("Setup", PathType.LogName, query);

            try
            {
                EventLogReader logReader = new EventLogReader(eventsQuery);
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

        #region Get KB Number from Title
        private static string GetKB(string title)
        {
            title = title.Replace("(", "").Replace(")", "");
            int pos = title.IndexOf("KB");
            if (pos > -1)
            {
                int endpos = title.IndexOf(" ", pos);
                if (endpos > -1)
                {
                    return title.Substring(pos, endpos - pos);
                }
                return title.Substring(pos);
            }
            return "n/a";
        }
        #endregion Get KB Number from Title

        #region Grid Size
        private void GridSmaller()
        {
            double curZoom = Settings.Default.GridZoom;
            if (curZoom > 0.5)
            {
                curZoom -= .05;
                Settings.Default.GridZoom = Math.Round(curZoom, 2);
            }
            dataGrid.LayoutTransform = new ScaleTransform(curZoom, curZoom);
            sc1.LayoutTransform = new ScaleTransform(curZoom, curZoom);
        }

        private void GridLarger()
        {
            double curZoom = Settings.Default.GridZoom;
            if (curZoom < 2.0)
            {
                curZoom += .05;
                Settings.Default.GridZoom = Math.Round(curZoom, 2);
            }

            dataGrid.LayoutTransform = new ScaleTransform(curZoom, curZoom);
            sc1.LayoutTransform = new ScaleTransform(curZoom, curZoom);
        }
        #endregion Grid Size

        #region Alternate row shading
        private void AltRowShadingOff()
        {
            dataGrid.AlternationCount = 0;
            dataGrid.RowBackground = new SolidColorBrush(Colors.White);
            dataGrid.AlternatingRowBackground = new SolidColorBrush(Colors.White);
            dataGrid.Items.Refresh();
        }

        private void AltRowShadingOn()
        {
            dataGrid.AlternationCount = 2;
            dataGrid.RowBackground = new SolidColorBrush(Colors.White);
            dataGrid.AlternatingRowBackground = new SolidColorBrush(Colors.WhiteSmoke);
            dataGrid.Items.Refresh();
        }
        #endregion Alternate row shading

        #region Update the grid
        private void UpdateGrid()
        {
            Mouse.OverrideCursor = Cursors.Wait;
            WhichList();
            dataGrid.Items.Refresh();
            Mouse.OverrideCursor = null;
        }
        #endregion Update the grid

        #region Get temp file name
        public static string GetTempLogFile()
        {
            // Ask NLog what the file name is
            var target = LogManager.Configuration.FindTargetByName("logFile") as FileTarget;
            var logEventInfo = new LogEventInfo { TimeStamp = DateTime.Now };
            return target.FileName.Render(logEventInfo);
        }
        #endregion

        #region Get the JSON file name
        private static string GetJsonFile()
        {
            return Path.Combine(AppInfo.AppDirectory, "WUViewExcludes.json");
        }
        #endregion Get the JSON file name

        #region Open support URL
        private void Hyperlink_RequestNavigate(object sender, System.Windows.Navigation.RequestNavigateEventArgs e)
        {
            log.Debug($"Opening {e.Uri.AbsoluteUri}");
            if (!string.IsNullOrWhiteSpace(e.Uri.AbsoluteUri))
            {
                _ = Process.Start(e.Uri.AbsoluteUri);
                e.Handled = true;
            }
        }
        #endregion Open support URL

        #region Read the Exclude file
        private void GetExcludes()
        {
            Stopwatch rxsw = new Stopwatch();
            rxsw.Start();
            string json = File.ReadAllText(GetJsonFile());
            ExcludedItems.ExcludedStrings = JsonConvert.DeserializeObject<List<ExcludedItems>>(json);
            rxsw.Stop();
            int xCount = ExcludedItems.ExcludedStrings.Count;
            string xRecs;
            if (xCount > 0)
            {
                foreach (var item in ExcludedItems.ExcludedStrings)
                {
                    log.Info($"Excluding updates containing: \"{item.ExcludedString}\"");
                }
            }
            xRecs = xCount == 1 ? "record" : "records";
            log.Debug($"Read {ExcludedItems.ExcludedStrings.Count} exclude {xRecs} from disk in {rxsw.Elapsed.TotalMilliseconds:N2} milliseconds");
        }
        #endregion Read the Exclude file

        #region Reset zoom
        private void GridSizeReset()
        {
            Settings.Default.GridZoom = 1.0;
            dataGrid.LayoutTransform = new ScaleTransform(1, 1);
            sc1.LayoutTransform = new ScaleTransform(1, 1);
        }
        #endregion Reset zoom

        #region Filter Any column
        private void TbxSearch_TextChanged(object sender, TextChangedEventArgs e)
        {
            string filter = tbxSearch.Text;

            // I really don't understand how this works
            ICollectionView cv = CollectionViewSource.GetDefaultView(dataGrid.ItemsSource);
            if (filter?.Length == 0)
            {
                cv.Filter = null;
            }
            else if (!filter.StartsWith("-"))
            {
                cv.Filter = o =>
                {
                    WUpdate wu = o as WUpdate;
                    return wu.Title.IndexOf(filter, StringComparison.OrdinalIgnoreCase) >= 0 ||
                           wu.ResultCode.IndexOf(filter, StringComparison.OrdinalIgnoreCase) >= 0 ||
                           wu.KBNum.IndexOf(filter, StringComparison.OrdinalIgnoreCase) >= 0;
                };
            }
            else
            {
                filter = filter.Remove(0, 1);
                cv.Filter = o =>
                {
                    WUpdate wu = o as WUpdate;
                    return wu.Title.IndexOf(filter, StringComparison.OrdinalIgnoreCase) == -1 &&
                           wu.ResultCode.IndexOf(filter, StringComparison.OrdinalIgnoreCase) == -1 &&
                           wu.KBNum.IndexOf(filter, StringComparison.OrdinalIgnoreCase) == -1;
                };
            }

            if (string.IsNullOrEmpty(tbxSearch.Text))
            {
                tb2.Text = string.Empty;
                btnSearch.IsEnabled = false;
            }
            else
            {
                if (dataGrid.Items.Count == 1)
                {
                    tb2.Text = string.Format($"Showing {dataGrid.Items.Count} filtered item");
                }
                else
                {
                    tb2.Text = string.Format($"Showing {dataGrid.Items.Count} filtered items");
                }
                btnSearch.IsEnabled = true;
            }
        }

        private void BtnSearch_Click(object sender, RoutedEventArgs e)
        {
            tbxSearch.Clear();
        }
        #endregion Filter Any column

        #region Copy to clipboard
        private void Copy2Clipboard()
        {
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
        }
        #endregion Copy to clipboard

        #region Save grid to CSV file
        private void SaveToCSV()
        {
            string fname = "WUView_" + DateTime.Now.Date.ToString("yyyy-MM-dd") + ".csv";
            SaveFileDialog dialog = new SaveFileDialog
            {
                Title = "Save Grid as CSV FIle",
                Filter = "CSV File|*.csv",
                InitialDirectory = Environment.GetFolderPath(Environment.SpecialFolder.Desktop),
                FileName = fname
            };
            var result = dialog.ShowDialog();
            if (result == true)
            {
                Copy2Clipboard();
                string gridData = (string)Clipboard.GetData(DataFormats.CommaSeparatedValue);
                File.WriteAllText(dialog.FileName, gridData, Encoding.UTF8);
            }
        }
        #endregion Save grid to CSV file

        #region Save details to a text file
        private void SaveToFile()
        {
            string fname = "WUView_" + DateTime.Now.Date.ToString("yyyy-MM-dd") + ".txt";
            SaveFileDialog dialog = new SaveFileDialog
            {
                Title = "Save Details as Text File",
                Filter = "Text File|*.txt",
                InitialDirectory = Environment.GetFolderPath(Environment.SpecialFolder.Desktop),
                FileName = fname
            };
            var result = dialog.ShowDialog();
            if (result == true)
            {
                StringBuilder sb = new StringBuilder();
                _ = sb.Append("Windows Update details for ").Append(Environment.MachineName)
                    .Append(" - ").AppendFormat("{0:G}", DateTime.Now).AppendLine();
                string uscore = new string('-', sb.Length - 2);
                _ = sb.Append(uscore).AppendLine("\r\n");

                List<WUpdate> listInUse = updatesFullList;
                if (mnuHideExcluded.IsChecked)
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

                    var lines = Regex.Split(FindEventLogs(listInUse[i].KBNum), "\r\n|\r|\n");
                    foreach (var line in lines)
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
    }
}
