// Copyright (c) Tim Kennedy. All Rights Reserved. Licensed under the MIT License.

namespace WUView.Helpers;

/// <summary>
/// Methods for displaying snackbar messages
/// </summary>
public static class SnackbarMsg
{
    #region Clear message queue then queue a message (default duration)
    public static void ClearAndQueueMessage(string message)
    {
        (Application.Current.MainWindow as MainWindow)?.SnackBar1.MessageQueue.Clear();
        (Application.Current.MainWindow as MainWindow)?.SnackBar1.MessageQueue.Enqueue(message);
    }
    #endregion Clear message queue then queue a message (default duration)

    #region Clear message queue then queue a message and set duration
    public static void ClearAndQueueMessage(string message, int duration)
    {
        (Application.Current.MainWindow as MainWindow)?.SnackBar1.MessageQueue.Clear();
        (Application.Current.MainWindow as MainWindow)?.SnackBar1.MessageQueue.Enqueue(message,
            null,
            null,
            null,
            false,
            true,
            TimeSpan.FromMilliseconds(duration));
    }
    #endregion Clear message queue then queue a message and set duration

}
