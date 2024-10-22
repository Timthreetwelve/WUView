// Copyright (c) Tim Kennedy. All Rights Reserved. Licensed under the MIT License.

namespace WUView.Helpers;

/// <summary>
/// Methods for reading and writing files
/// </summary>
public static partial class FileHelpers
{
    #region JSON options
    private static JsonSerializerOptions JsonOptions { get; } = new()
    {
        WriteIndented = true
    };
    #endregion JSON options

    #region Read the Exclude file
    /// <summary>
    ///  Read the JSON file containing the exclude items
    /// </summary>
    public static void GetExcludes()
    {
        Stopwatch rxStopWatch = Stopwatch.StartNew();
        string json = File.ReadAllText(GetExcludesFile());
        rxStopWatch.Stop();
        ExcludedItems.ExcludedStrings = JsonSerializer.Deserialize<ObservableCollection<ExcludedItems>>(json)!;
        int xCount = ExcludedItems.ExcludedStrings!.Count;
        string xRecs = xCount == 1 ? "record" : "records";
        _log.Debug($"Read {ExcludedItems.ExcludedStrings.Count} exclude {xRecs} from disk in {rxStopWatch.Elapsed.TotalMilliseconds:N2} milliseconds");
        if (xCount > 0)
        {
            foreach (ExcludedItems item in ExcludedItems.ExcludedStrings)
            {
                _log.Info($"{GetStringResource("MsgText_ExcludingContaining")} \"{item.ExcludedString}\"");
            }
        }
    }
    #endregion Read the Exclude file

    #region Save the Exclude file
    /// <summary>
    ///  Save the JSON file containing the exclude items
    /// </summary>
    public static async Task SaveExcludeFile()
    {
        string json = JsonSerializer.Serialize(ExcludedItems.ExcludedStrings, JsonOptions);
        await File.WriteAllTextAsync(GetExcludesFile(), json);
        _log.Info($"Saving {GetExcludesFile()}");

        foreach (ExcludedItems item in ExcludedItems.ExcludedStrings)
        {
            _log.Info($"Excluding updates containing: \"{item.ExcludedString}\"");
        }
    }
    #endregion Save the Exclude file

    #region Get the exclude file name
    /// <summary>
    /// Determine the full path for the exclude file. Create it if it doesn't exist.
    /// </summary>
    /// <returns>Full path to exclude file</returns>
    public static string GetExcludesFile()
    {
        string filePath = Path.Combine(AppInfo.AppDirectory, "WUViewExcludes.json");
        if (!File.Exists(filePath))
        {
            File.Create(filePath).Dispose();
            File.WriteAllText(filePath, "[{ \"ExcludedString\": \"Defender\"}]");
            _log.Debug($"New Exclude file created: {filePath}");
        }
        return filePath;
    }
    #endregion Get the exclude file name

    #region Save grid to CSV file
    /// <summary>
    ///  Save the contents of the DataGrid to a CSV file
    /// </summary>
    public static async Task SaveToCSV()
    {
        string filename = "WUView_" + DateTime.Now.Date.ToString("yyyy-MM-dd", CultureInfo.InvariantCulture) + ".csv";
        SaveFileDialog dialog = new()
        {
            Title = ResourceHelpers.GetStringResource("MenuItem_SaveCSV"),
            Filter = "CSV File|*.csv",
            InitialDirectory = Environment.GetFolderPath(Environment.SpecialFolder.Desktop),
            FileName = filename
        };
        bool? result = dialog.ShowDialog();
        if (result == true)
        {
            MainPage.Instance!.Copy2Clipboard();
            string gridData = (string)Clipboard.GetData(DataFormats.CommaSeparatedValue);
            await File.WriteAllTextAsync(dialog.FileName, gridData, Encoding.UTF8);
            _log.Debug($"Details written to {dialog.FileName}");
        }
    }
    #endregion Save grid to CSV file

    #region Save details to a text file
    /// <summary>
    /// Saves details to a text file.
    /// </summary>
    public static async Task SaveToFile()
    {
        string filename = "WUView_" + DateTime.Now.Date.ToString("yyyy-MM-dd", CultureInfo.InvariantCulture) + ".txt";
        SaveFileDialog dialog = new()
        {
            Title = GetStringResource("MenuItem_SaveTXT"),
            Filter = "Text File|*.txt",
            InitialDirectory = Environment.GetFolderPath(Environment.SpecialFolder.Desktop),
            FileName = filename
        };
        bool? result = dialog.ShowDialog();
        if (result == true)
        {
            StringBuilder sb = new();
            _ = sb.Append(GetStringResource("Details_HeadingUpdate"))
                .Append(' ')
                .Append(Environment.MachineName)
                .Append(" - ")
                .AppendFormat(CultureInfo.InvariantCulture, "{0:G}", DateTime.Now)
                .AppendLine();
            string underscore = new('-', sb.Length - 2);
            _ = sb.Append(underscore).AppendLine("\r\n");

            List<WUpdate> listInUse = [.. MainViewModel.UpdatesFullList];
            if (UserSettings.Setting!.HideExcluded)
            {
                listInUse = [.. MainViewModel.UpdatesWithoutExcludedItems];
            }

            for (int i = 0; i < listInUse.Count; i++)
            {
                _ = sb.Append(GetStringResource("Details_Title"))
                      .Append(' ')
                      .AppendLine(listInUse[i].Title)
                      .Append(GetStringResource("Details_Date"))
                      .Append(' ')
                      .AppendLine(listInUse[i].Date.ToString(CultureInfo.InvariantCulture))
                      .Append(GetStringResource("Details_KBNum"))
                      .Append(' ')
                      .AppendLine(listInUse[i].KBNum)
                      .Append(GetStringResource("Details_Operation"))
                      .Append(' ')
                      .AppendLine(listInUse[i].Operation!.Replace("uo", ""))
                      .Append(GetStringResource("Details_ResultCode"))
                      .Append(' ')
                      .AppendLine(listInUse[i].ResultCode!.Replace("orc", ""))
                      .Append(GetStringResource("Details_HResult"))
                      .Append(' ')
                      .AppendFormat(CultureInfo.InvariantCulture, $"0x{int.Parse(listInUse[i].HResult!, CultureInfo.InvariantCulture):X8}")
                      .AppendLine()
                      .Append(GetStringResource("Details_UpdateID"))
                      .Append(' ')
                      .AppendLine(listInUse[i].UpdateID)
                      .Append(GetStringResource("Details_SupportURL"))
                      .Append(' ')
                      .AppendLine(listInUse[i].SupportURL)
                      .Append(GetStringResource("Details_Description"))
                      .Append(' ')
                      .AppendLine(listInUse[i].Description);

                foreach (string line in RemoveCarriageReturnLineFeed().Split(MainViewModel.FindEventLogs(listInUse[i].KBNum!)))
                {
                    if (!string.IsNullOrWhiteSpace(line))
                    {
                        _ = sb.Append(GetStringResource("Details_HeadingEventLog")).Append(' ').AppendLine(line);
                    }
                }
                _ = sb.AppendLine("\r\n");
            }
            await File.WriteAllTextAsync(dialog.FileName, sb.ToString());
            _ = sb.Clear();
            _log.Debug($"Details written to {dialog.FileName}");
        }
    }
    [GeneratedRegex("\r\n|\r|\n")]
    private static partial Regex RemoveCarriageReturnLineFeed();
    #endregion Save details to a text file

    #region Save updates as JSON
    /// <summary>
    /// Save entire history as JSON file
    /// </summary>
    public static async Task SaveAsJson()
    {
        string filename = "WUView_Export_" + DateTime.Now.Date.ToString("yyyy-MM-dd", CultureInfo.InvariantCulture) + ".json";
        SaveFileDialog dialog = new()
        {
            Title = GetStringResource("MenuItem_SaveJSON"),
            Filter = "JSON File|*.json",
            InitialDirectory = Environment.GetFolderPath(Environment.SpecialFolder.Desktop),
            FileName = filename
        };
        bool? result = dialog.ShowDialog();
        if (result == true)
        {
            string json = JsonSerializer.Serialize(MainViewModel.UpdatesFullList, JsonOptions);
            await File.WriteAllTextAsync(dialog.FileName, json);
            _log.Debug($"History exported to {dialog.FileName}");
        }
    }
    #endregion Save updates as JSON
}
