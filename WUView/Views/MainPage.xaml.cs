// Copyright (c) Tim Kennedy. All Rights Reserved. Licensed under the MIT License.

namespace WUView.Views;

/// <summary>
/// MainPage is home to the data grid and details pane
/// </summary>
public partial class MainPage : UserControl
{
    #region MainPage Instance
    public static MainPage? Instance { get; private set; }
    #endregion MainPage Instance

    public MainPage()
    {
        InitializeComponent();
        Instance = this;

        SetDetailsHeight();
    }

    #region Set height of details pane
    /// <summary>
    /// Set the details pane height
    /// </summary>
    public void SetDetailsHeight()
    {
        DetailsRow.Height = !UserSettings.Setting!.ShowDetails
            ? new GridLength(1)
            : new GridLength(UserSettings.Setting.DetailsHeight);
    }
    #endregion Set height of details pane

    #region HResult click event
    /// <summary>
    /// Put HResult in clipboard and open Windows Update error code site
    /// </summary>
    private void HResult_PreviewMouseDown(object sender, MouseButtonEventArgs e)
    {
        if (HypHResult.Inlines.FirstInline is Run run)
        {
            try
            {
                if (ClipboardHelper.CopyTextToClipboard(run.Text))
                {
                    SnackbarMsg.ClearAndQueueMessage(string.Format(CultureInfo.InvariantCulture,
                        MsgTextHResultCopiedToClipboard, run.Text), 3000);
                }
            }
            catch (Exception ex)
            {
                _log.Error(ex, "Could not open clipboard for HResult");
            }
        }
        _log.Debug($"{GetStringResource("MsgText_Opening")} {AppConstUri.HResultCodeUrl}");
        Process p = new();
        p.StartInfo.FileName = AppConstUri.HResultCodeUrl.AbsoluteUri;
        p.StartInfo.UseShellExecute = true;
        p.Start();
        e.Handled = true;
    }
    #endregion HResult click event

    #region Result Code click event
    /// <summary>
    /// Open the result code page in the wiki
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    private void ResultCode_PreviewMouseDown(object sender, MouseButtonEventArgs e)
    {
        _log.Debug($"{GetStringResource("MsgText_Opening")} {AppConstUri.ResultCodeUrl}");
        Process p = new();
        p.StartInfo.FileName = AppConstUri.ResultCodeUrl.AbsoluteUri;
        p.StartInfo.UseShellExecute = true;
        p.Start();
        e.Handled = true;
    }
    #endregion Result Code click event

    #region URL click event
    /// <summary>
    /// Navigate to website
    /// </summary>
    private void Hyperlink_RequestNavigate(object sender, RequestNavigateEventArgs e)
    {
        _log.Debug($"{GetStringResource("MsgText_Opening")} {e.Uri.AbsoluteUri}");
        if (!string.IsNullOrWhiteSpace(e.Uri.AbsoluteUri))
        {
            Process p = new();
            p.StartInfo.FileName = e.Uri.AbsoluteUri;
            p.StartInfo.UseShellExecute = true;
            p.Start();
            e.Handled = true;
            SnackbarMsg.ClearAndQueueMessage($"{GetStringResource("MsgText_Opening")} {e.Uri.AbsoluteUri}", 2000);
        }
    }
    #endregion URL click event

    #region Filter textbox changed event
    /// <summary>
    /// Used by the "filter" textbox at the top
    /// </summary>
    private void TbxSearch_TextChanged(object sender, TextChangedEventArgs e)
    {
        FilterTheGrid();
    }
    #endregion Filter textbox changed event

    #region Filter the datagrid
    /// <summary>
    /// Filters the grid.
    /// </summary>
    /// <param name="exit">If true, will return if filter length is 0.</param>
    public void FilterTheGrid(bool exit = false)
    {
        string filter = TbxSearch.Text;

        ICollectionView cv = CollectionViewSource.GetDefaultView(DataGrid.ItemsSource);
        if (filter.Length == 0)
        {
            cv.Filter = null;
            if (exit)
            {
                return;
            }
        }
        else if (filter.StartsWith('!'))
        {
            filter = filter[1..].TrimStart(' ');
            cv.Filter = o =>
            {
                WUpdate? wu = o as WUpdate;
                return !wu!.Title!.Contains(filter, StringComparison.OrdinalIgnoreCase) &&
                       !wu.ResultCode!.Contains(filter, StringComparison.OrdinalIgnoreCase) &&
                       !wu.KBNum!.Contains(filter, StringComparison.OrdinalIgnoreCase);
            };
        }
        else
        {
            cv.Filter = o =>
            {
                WUpdate? wu = o as WUpdate;
                return wu!.Title!.Contains(filter, StringComparison.OrdinalIgnoreCase) ||
                       wu.ResultCode!.Contains(filter, StringComparison.OrdinalIgnoreCase) ||
                       wu.KBNum!.Contains(filter, StringComparison.OrdinalIgnoreCase);
            };
        }
        if (DataGrid.Items.Count == 1)
        {
            SnackbarMsg.ClearAndQueueMessage(GetStringResource("MsgText_FilterOneRowShown"), 2000);
        }
        else
        {
            SnackbarMsg.ClearAndQueueMessage(string.Format(CultureInfo.InvariantCulture,
                MsgTextFilterRowsShown, DataGrid.Items.Count), 2000);
        }
    }
    #endregion Filter the datagrid

    #region Clear column sort
    /// <summary>
    ///  Clears any sorts that may have been applied to columns in the DataGrid
    /// </summary>
    internal void ClearColumnSort()
    {
        foreach (DataGridColumn column in DataGrid.Columns)
        {
            column.SortDirection = null;
        }
        DataGrid.Items.SortDescriptions.Clear();

        SnackbarMsg.ClearAndQueueMessage(GetStringResource("MsgText_ColumnSortCleared"));
    }
    #endregion Clear column sort

    #region Copy to clipboard
    /// <summary>
    /// Copies the present contents of the DataGrid to the clipboard
    /// </summary>
    public void Copy2Clipboard(bool msg = false)
    {
        // Preserve the selected row
        int selIndex = DataGrid.SelectedIndex;

        // Clear the clipboard
        Clipboard.Clear();

        // Include the header row
        DataGrid.ClipboardCopyMode = DataGridClipboardCopyMode.IncludeHeader;

        // Temporarily set selection mode to all rows
        DataGrid.SelectionMode = DataGridSelectionMode.Extended;

        // Select all the cells
        DataGrid.SelectAllCells();

        // Execute the copy
        ApplicationCommands.Copy.Execute(null, DataGrid);

        // Unselect the cells
        DataGrid.UnselectAllCells();

        // Set selection mode back to one row
        DataGrid.SelectionMode = DataGridSelectionMode.Single;

        // re-select the previous row
        DataGrid.SelectedIndex = selIndex;

        if (msg)
        {
            SnackbarMsg.ClearAndQueueMessage(GetStringResource("MsgText_CopiedToClipboard"), 2000);
        }
    }
    #endregion Copy to clipboard

    #region Update the grid
    /// <summary>
    /// Update the DataGrid after changes have occurred
    /// </summary>
    public void UpdateGrid()
    {
        Mouse.OverrideCursor = Cursors.Wait;
        DataGrid.Items.Refresh();
        Mouse.OverrideCursor = null;
    }
    #endregion Update the grid

    #region GridSplitter drag event
    /// <summary>
    /// Fires when grid splitter drag has completed
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    private void DetailsSplitter_DragCompleted(object sender, DragCompletedEventArgs e)
    {
        UserSettings.Setting!.DetailsHeight = DetailsRow.Height.Value;
    }
    #endregion GridSplitter drag event

    #region Refresh everything
    /// <summary>
    /// Handles the Click event of the Refresh control.
    /// </summary>
    /// <param name="sender">The source of the event.</param>
    /// <param name="e">The <see cref="RoutedEventArgs"/> instance containing the event data.</param>
    private void Refresh_Click(object sender, RoutedEventArgs e)
    {
        RefreshAll();
    }

    /// <summary>
    /// Refreshes all update and event log data.
    /// </summary>
    public static void RefreshAll()
    {
        _log.Debug("Refresh in progress");
        MainViewModel.ClearLists();
        MainViewModel.GatherInfo();
        Instance!.UpdateGrid();
        SnackbarMsg.ClearAndQueueMessage(GetStringResource("MsgText_ListRefreshed"), 2000);
    }
    #endregion Refresh everything

    #region Unloaded event
    /// <summary>
    /// Nullify any DataGrid filter on the unloaded event
    /// </summary>
    /// <param name="sender">The source of the event.</param>
    /// <param name="e">The <see cref="RoutedEventArgs"/> instance containing the event data.</param>
    private void UserControl_Unloaded(object sender, RoutedEventArgs e)
    {
        DataGrid.Items.Filter = null;
    }
    #endregion Unloaded event

    #region Loaded event
    /// <summary>
    /// Set the order of the DataGrid columns
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    private void UserControl_Loaded(object sender, RoutedEventArgs e)
    {
        SetColumnOrder(DataGrid);
    }
    #endregion Loaded event

    #region DataGrid column reorder
    /// <summary>
    /// Column reorder event
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    private void ColumnReorderEvent(object sender, DataGridColumnEventArgs e)
    {
        SaveColumnOrder(sender as DataGrid);
    }
    #endregion DataGrid column reorder

    #region Save order of columns in the DataGrid
    /// <summary>
    /// Save DataGrid column order
    /// </summary>
    /// <param name="grid">The DataGrid</param>
    private static void SaveColumnOrder(DataGrid? grid)
    {
        UserSettings.Setting!.ColumnKB = grid!.Columns[0].DisplayIndex;
        UserSettings.Setting.ColumnDate = grid.Columns[1].DisplayIndex;
        UserSettings.Setting.ColumnTitle = grid.Columns[2].DisplayIndex;
        UserSettings.Setting.ColumnResult = grid.Columns[3].DisplayIndex;
    }
    #endregion Save order of columns in the DataGrid

    #region Set order of columns in the DataGrid
    /// <summary>
    /// Set DataGrid column order
    /// </summary>
    /// <param name="grid">The DataGrid</param>
    private static void SetColumnOrder(DataGrid grid)
    {
        try
        {
            Dictionary<int, int> columns = new()
            {
                { UserSettings.Setting!.ColumnKB, 0 },
                { UserSettings.Setting.ColumnDate, 1 },
                { UserSettings.Setting.ColumnTitle, 2 },
                { UserSettings.Setting.ColumnResult, 3 }
            };

            foreach (KeyValuePair<int, int> pair in columns.OrderBy(k => k.Key))
            {
                grid.Columns[pair.Value].DisplayIndex = pair.Key;
            }
        }
        catch (Exception ex)
        {
            _log.Error(ex, "Error setting data grid column order.");
            for (int i = 0; i < grid.Columns.Count; i++)
            {
                grid.Columns[i].DisplayIndex = i;
            }
            _log.Error(ex, "Data grid column order has been reset.");
        }
    }
    #endregion Set order of columns in the DataGrid
}
