// Copyright (c) Tim Kennedy. All Rights Reserved. Licensed under the MIT License.

using static Vanara.PInvoke.User32;

namespace WUView.Helpers;

internal static class ClipboardHelper
{
    #region Copy text to clipboard
    private const uint _const_CF_UNICODETEXT = 13;

    /// <summary>
    /// Copies text to clipboard using Vanara PInvoke instead of DllImport
    /// </summary>
    /// <param name="text">Text to be placed in the Windows clipboard</param>
    public static bool CopyTextToClipboard(string text)
    {
        if (!OpenClipboard(IntPtr.Zero) || text.Length < 1)
        {
            return false;
        }

        IntPtr global = Marshal.StringToHGlobalUni(text);

        _ = SetClipboardData(_const_CF_UNICODETEXT, global);
        _ = CloseClipboard();

        return true;
    }
    #endregion Copy text to clipboard
}
