// Copyright (c) Tim Kennedy. All Rights Reserved. Licensed under the MIT License.

namespace WUView.Helpers;

/// <summary>
/// Methods for reading and writing files
/// </summary>
public static class FileHelpers
{
    #region Read the Exclude file
    /// <summary>
    ///  Read the JSON file containing the exclude items
    /// </summary>
    public static void GetExcludes()
    {
        Stopwatch rxsw = Stopwatch.StartNew();
        string json = File.ReadAllText(GetExcludesFile());
        rxsw.Stop();
        ExcludedItems.ExcludedStrings = JsonSerializer.Deserialize<ObservableCollection<ExcludedItems>>(json);
        int xCount = ExcludedItems.ExcludedStrings.Count;
        string xRecs = xCount == 1 ? "record" : "records";
        _log.Debug($"Read {ExcludedItems.ExcludedStrings.Count} exclude {xRecs} from disk in {rxsw.Elapsed.TotalMilliseconds:N2} milliseconds");
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
    public static async void SaveExcludeFile()
    {
        JsonSerializerOptions opts = new()
        {
            WriteIndented = true
        };
        string json = JsonSerializer.Serialize(ExcludedItems.ExcludedStrings, opts);
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
    ///  Save the contents of the datagrid to a CSV file
    /// </summary>
    public static async void SaveToCSV()
    {
        string fname = "WUView_" + DateTime.Now.Date.ToString("yyyy-MM-dd") + ".csv";
        SaveFileDialog dialog = new()
        {
            Title = ResourceHelpers.GetStringResource("MenuItem_SaveCSV"),
            Filter = "CSV File|*.csv",
            InitialDirectory = Environment.GetFolderPath(Environment.SpecialFolder.Desktop),
            FileName = fname
        };
        bool? result = dialog.ShowDialog();
        if (result == true)
        {
            MainPage.Instance.Copy2Clipboard(false);
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
    public static async void SaveToFile()
    {
        string fname = "WUView_" + DateTime.Now.Date.ToString("yyyy-MM-dd") + ".txt";
        SaveFileDialog dialog = new()
        {
            Title = ResourceHelpers.GetStringResource("MenuItem_SaveTXT"),
            Filter = "Text File|*.txt",
            InitialDirectory = Environment.GetFolderPath(Environment.SpecialFolder.Desktop),
            FileName = fname
        };
        bool? result = dialog.ShowDialog();
        if (result == true)
        {
            StringBuilder sb = new();
            _ = sb.Append(GetStringResource("Details_HeadingUpdate"))
                .Append(' ')
                .Append(Environment.MachineName)
                .Append(" - ")
                .AppendFormat("{0:G}", DateTime.Now)
                .AppendLine();
            string uscore = new('-', sb.Length - 2);
            _ = sb.Append(uscore).AppendLine("\r\n");

            List<WUpdate> listInUse = MainViewModel.UpdatesFullList.ToList();
            if (UserSettings.Setting.HideExcluded)
            {
                listInUse = MainViewModel.UpdatesWithoutExcludedItems.ToList();
            }

            for (int i = 0; i < listInUse.Count; i++)
            {
                _ = sb.Append(GetStringResource("Details_Title")).Append(' ').AppendLine(listInUse[i].Title)
                    .Append(GetStringResource("Details_Date")).Append(' ').AppendLine(listInUse[i].Date.ToString())
                    .Append(GetStringResource("Details_KBNum")).Append(' ').AppendLine(listInUse[i].KBNum)
                    .Append(GetStringResource("Details_Operation")).Append(' ').AppendLine(listInUse[i].Operation.Replace("uo", ""))
                    .Append(GetStringResource("Details_ResultCode")).Append(' ').AppendLine(listInUse[i].ResultCode.Replace("orc", ""))
                    .Append(GetStringResource("Details_HResult")).Append(' ').AppendFormat($"0x{int.Parse(listInUse[i].HResult):X8}").AppendLine()
                    .Append(GetStringResource("Details_UpdateID")).Append(' ').AppendLine(listInUse[i].UpdateID)
                    .Append(GetStringResource("Details_SupportURL")).Append(' ').AppendLine(listInUse[i].SupportURL)
                    .Append(GetStringResource("Details_Description")).Append(' ').AppendLine(listInUse[i].Description);

                foreach (string line in Regex.Split(MainViewModel.FindEventLogs(listInUse[i].KBNum), "\r\n|\r|\n"))
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
    #endregion Save details to a text file
}
