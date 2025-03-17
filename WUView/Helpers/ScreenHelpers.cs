// Copyright (c) Tim Kennedy. All Rights Reserved. Licensed under the MIT License.

namespace WUView.Helpers;

internal static class ScreenHelpers
{
    #region Reposition off-screen window back to the desktop
    /// <summary>
    /// Keep the window on the screen.
    /// </summary>
    public static void KeepWindowOnScreen(Window? window)
    {
        if (window is null || UserSettings.Setting!.StartCentered)
        {
            return;
        }

        // the SystemParameters properties work better for this method than Screen properties.
        if (window.Top < SystemParameters.VirtualScreenTop)
        {
            window.Top = SystemParameters.VirtualScreenTop;
        }

        if (window.Left < SystemParameters.VirtualScreenLeft)
        {
            window.Left = SystemParameters.VirtualScreenLeft;
        }

        if (window.Left + window.Width > SystemParameters.VirtualScreenLeft + SystemParameters.VirtualScreenWidth)
        {
            window.Left = SystemParameters.VirtualScreenWidth + SystemParameters.VirtualScreenLeft - window.Width;
        }

        if (window.Top + window.Height > SystemParameters.VirtualScreenTop + SystemParameters.VirtualScreenHeight)
        {
            window.Top = SystemParameters.WorkArea.Size.Height + SystemParameters.VirtualScreenTop - window.Height;
        }
    }
    #endregion Reposition off-screen window back to the desktop
}
