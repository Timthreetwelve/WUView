// Copyright (c) Tim Kennedy. All Rights Reserved. Licensed under the MIT License.

namespace WUView.Views;

/// <summary>
/// MainPage is home to the data grid and details pane
/// </summary>
public partial class MainPage : UserControl
{
    #region NLog Instance
    private static readonly Logger _log = LogManager.GetLogger("logTemp");
    #endregion NLog Instance

    #region MainPage Instance
    public static MainPage Instance { get; set; }
    #endregion MainPage Instance

    public MainPage()
    {
        InitializeComponent();
        Instance = this;
    }

    #region HResult click event
    /// <summary>
    /// Put HResult in clipboard and open Windows Update error code site
    /// </summary>
    private void HResult_PreviewMouseDown(object sender, MouseButtonEventArgs e)
    {
        if (hypHResult.Inlines.FirstInline is Run run)
        {
            Clipboard.SetText(run.Text);
        }
        _log.Debug($"Opening {AppConstUri.ResultCodeUrl}");
        Process p = new();
        p.StartInfo.FileName = AppConstUri.ResultCodeUrl.AbsoluteUri;
        p.StartInfo.UseShellExecute = true;
        p.Start();
        e.Handled = true;
        SnackbarMsg.ClearAndQueueMessage($"Opening {AppConstUri.ResultCodeUrl}", 2000);
    }
    #endregion HResult click event

    #region URL click event
    /// <summary>
    /// Navigate to website
    /// </summary>
    private void Hyperlink_RequestNavigate(object sender, RequestNavigateEventArgs e)
    {
        _log.Debug($"Opening {e.Uri.AbsoluteUri}");
        if (!string.IsNullOrWhiteSpace(e.Uri.AbsoluteUri))
        {
            Process p = new();
            p.StartInfo.FileName = e.Uri.AbsoluteUri;
            p.StartInfo.UseShellExecute = true;
            p.Start();
            e.Handled = true;
            SnackbarMsg.ClearAndQueueMessage($"Opening {e.Uri.AbsoluteUri}", 2000);
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
    public void FilterTheGrid()
    {
        string filter = tbxSearch.Text;

        ICollectionView cv = CollectionViewSource.GetDefaultView(dataGrid.ItemsSource);
        if (filter?.Length == 0)
        {
            cv.Filter = null;
            SnackbarMsg.ClearAndQueueMessage("Filter removed", 2000);
        }
        else
        {
            if (filter?.StartsWith("!") == true)
            {
                filter = filter[1..].TrimStart(' ');
                cv.Filter = o =>
                {
                    WUpdate wu = o as WUpdate;
                    return !wu.Title.Contains(filter, StringComparison.OrdinalIgnoreCase) &&
                           !wu.ResultCode.Contains(filter, StringComparison.OrdinalIgnoreCase) &&
                           !wu.KBNum.Contains(filter, StringComparison.OrdinalIgnoreCase);
                };
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
            }

            if (dataGrid.Items.Count == 1)
            {
                SnackbarMsg.ClearAndQueueMessage("1 row shown", 2000);
            }
            else
            {
                SnackbarMsg.ClearAndQueueMessage($"{dataGrid.Items.Count} rows shown", 2000);
            }
        }
    }
    #endregion Filter the datagrid

    #region Clear column sort
    /// <summary>
    ///  Clears any sorts that may have been applied to columns in the datagrid
    /// </summary>
    internal void ClearColumnSort()
    {
        foreach (DataGridColumn column in dataGrid.Columns)
        {
            column.SortDirection = null;
        }
        dataGrid.Items.SortDescriptions.Clear();

        SnackbarMsg.ClearAndQueueMessage("Column sort cleared");
    }
    #endregion Clear column sort

    #region Copy to clipboard
    /// <summary>
    /// Copies the present contents of the datagrid to the clipboard
    /// </summary>
    public void Copy2Clipboard(bool msg = false)
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

        if (msg)
        {
            SnackbarMsg.ClearAndQueueMessage("Copied to clipboard", 1000);
        }
    }
    #endregion Copy to clipboard

    #region Update the grid
    /// <summary>
    /// Update the datagrid after changes have occurred
    /// </summary>
    public void UpdateGrid()
    {
        Mouse.OverrideCursor = Cursors.Wait;
        dataGrid.Items.Refresh();
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
        UserSettings.Setting.DetailsHeight = DetailsRow.Height.Value;
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
        Instance.UpdateGrid();
        SnackbarMsg.ClearAndQueueMessage("List Refreshed", 2000);
    }
    #endregion Refresh everything
}
