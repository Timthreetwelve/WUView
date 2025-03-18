// Copyright (c) Tim Kennedy. All Rights Reserved. Licensed under the MIT License.

namespace WUView.Models;

/// <summary>
/// Class to hold Setup event log information.
/// </summary>
internal sealed class WuEventRecord
{
    /// <summary>
    /// The Knowledge Base (KB) number.
    /// </summary>
    public string? KayBee { get; init; }

    /// <summary>
    /// The time the event record was created.
    /// </summary>
    public DateTime TimeCreated { get; init; }

    /// <summary>
    /// The event ID number.
    /// </summary>
    public int EventId { get; init; }

    /// <summary>
    /// The description text.
    /// </summary>
    public string? Description { get; init; }

    /// <summary>
    /// The record ID number.
    /// </summary>
    public long? RecordId { get; init; }
}
