// Copyright (c) Tim Kennedy. All Rights Reserved. Licensed under the MIT License.

namespace WUView.Converters;

/// <summary>
/// Updates strings to the desired format.
/// </summary>
/// <seealso cref="System.Windows.Data.IValueConverter" />
class ResultsConverter : IValueConverter
{
    public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
    {
        if (value == null || parameter == null)
        {
            return string.Empty;
        }
        else
        {
            switch (parameter)
            {
                case "HResult":
                    string hr = (string)value;
                    return string.Format($"0x{int.Parse(hr):X8}");

                //case "ResultCode":
                //    string rc = (string)value;
                //    return rc.Replace("orc", "");

                case "Operation":
                    string op = (string)value;
                    return op.Replace("uo", "");
            }
        }
        return value.ToString();
    }

    public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
    {
        throw new NotImplementedException();
    }
}
