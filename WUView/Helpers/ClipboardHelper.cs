// Copyright (c) Tim Kennedy. All Rights Reserved. Licensed under the MIT License.

// Based on https://stackoverflow.com/a/30165665

namespace WUView.Helpers;

internal static class ClipboardHelper
{
    [DllImport("user32.dll")]
    private static extern bool OpenClipboard(IntPtr hWndNewOwner);

    [DllImport("user32.dll")]
    private static extern bool CloseClipboard();

    [DllImport("user32.dll")]
    private static extern bool SetClipboardData(uint uFormat, IntPtr data);

    private const uint CF_UNICODETEXT = 13;

    public static bool CopyTextToClipboard(string text)
    {
        if (!OpenClipboard(IntPtr.Zero))
        {
            return false;
        }

        IntPtr global = Marshal.StringToHGlobalUni(text);

        SetClipboardData(CF_UNICODETEXT, global);
        CloseClipboard();

        return true;
    }
}
