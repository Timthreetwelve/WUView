// Copyright (c) Tim Kennedy. All Rights Reserved. Licensed under the MIT License.

namespace WUView.Helpers;

internal static class ResourceHelpers
{
    /// <summary>
    /// Gets the string resource for the key.
    /// </summary>
    /// <param name="key">The key.</param>
    /// <returns>String</returns>
    public static string GetStringResource(string key)
    {
        try
        {
            return Application.Current.TryFindResource(key).ToString();
        }
        catch (Exception ex)
        {
            _log.Error(ex, $"Resource not found: {key}");
            return $"Resource not found: {key}";
        }
    }
}
