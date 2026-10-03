using MachineryManagerEnterprise.SharedKernel;

namespace Usage.Domain;

/// <summary>
/// A single row on a <see cref="UsageLedger"/>'s append-only timeline
/// (chat, 2026-09-29). Either a <see cref="UsageEntryKind.ShiftReading"/>
/// tied to a specific Work Calendar shift (<see cref="ShiftIndex"/>,
/// <see cref="ShiftStartTime"/>, <see cref="ShiftEndTime"/>), or a
/// <see cref="UsageEntryKind.Rebase"/> that re-anchors the counter
/// baseline without contributing Operational Usage. Ordered on the
/// Ledger strictly by <see cref="Sequence"/>, which is the sole
/// ordering key — <see cref="EntryDate"/> is data, not sort order,
/// because a Rebase can share a date with a shift reading.
/// </summary>
public sealed class UsageEntry : Entity<UsageEntryId>
{
    /// <summary>Gets this entry's position in the Ledger's append-only timeline.</summary>
    public int Sequence { get; private set; }

    /// <summary>Gets whether this is a normal shift reading or a counter rebase.</summary>
    public UsageEntryKind Kind { get; private set; }

    /// <summary>Gets whether this entry was entered by a user or auto-generated to fill a missed mandatory shift (BR-044).</summary>
    public UsageEntryOrigin Origin { get; private set; }

    /// <summary>Gets the calendar date this entry belongs to.</summary>
    public DateOnly EntryDate { get; private set; }

    /// <summary>Gets the Work Calendar shift index this entry reports on, or <see langword="null"/> for a <see cref="UsageEntryKind.Rebase"/> entry.</summary>
    public int? ShiftIndex { get; private set; }

    /// <summary>Gets the shift's scheduled start time, or <see langword="null"/> for a <see cref="UsageEntryKind.Rebase"/> entry.</summary>
    public TimeOnly? ShiftStartTime { get; private set; }

    /// <summary>Gets the shift's scheduled end time, or <see langword="null"/> for a <see cref="UsageEntryKind.Rebase"/> entry.</summary>
    public TimeOnly? ShiftEndTime { get; private set; }

    /// <summary>Gets the raw value shown on the device for this entry.</summary>
    public decimal RawReadingValue { get; private set; }

    /// <summary>Gets the device that produced <see cref="RawReadingValue"/> (BR-012).</summary>
    public MeterDeviceId SourceMeterDeviceId { get; private set; } = null!;

    /// <summary>Gets the Operational Usage contributed by this entry; always zero for a <see cref="UsageEntryKind.Rebase"/> entry or the Ledger's very first entry.</summary>
    public decimal OperationalUsageAmount { get; private set; }

    /// <summary>Gets the operator this shift is attributed to, when supplied.</summary>
    public Guid? OperatorId { get; private set; }

    /// <summary>Gets the Project the owner belonged to at the time this entry was registered, snapshotted for historical reporting accuracy (BR-054).</summary>
    public Guid ProjectId { get; private set; }

    // Reserved for ORM materialization only. Never used by application code.
    private UsageEntry()
    {
    }

    internal UsageEntry(
        UsageEntryId id,
        int sequence,
        UsageEntryKind kind,
        UsageEntryOrigin origin,
        DateOnly entryDate,
        int? shiftIndex,
        TimeOnly? shiftStartTime,
        TimeOnly? shiftEndTime,
        decimal rawReadingValue,
        MeterDeviceId sourceMeterDeviceId,
        decimal operationalUsageAmount,
        Guid? operatorId,
        Guid projectId)
        : base(id)
    {
        Sequence = sequence;
        Kind = kind;
        Origin = origin;
        EntryDate = entryDate;
        ShiftIndex = shiftIndex;
        ShiftStartTime = shiftStartTime;
        ShiftEndTime = shiftEndTime;
        RawReadingValue = rawReadingValue;
        SourceMeterDeviceId = sourceMeterDeviceId;
        OperationalUsageAmount = operationalUsageAmount;
        OperatorId = operatorId;
        ProjectId = projectId;
    }

    /// <summary>Applies a corrected raw value and its recalculated Operational Usage to this entry (used by <see cref="UsageLedger.CorrectEntryReading"/>).</summary>
    internal void ApplyReadingCorrection(decimal rawReadingValue, decimal operationalUsageAmount)
    {
        RawReadingValue = rawReadingValue;
        OperationalUsageAmount = operationalUsageAmount;
    }

    /// <summary>
    /// Applies a cascaded recalculation of Operational Usage to the
    /// next entry after one whose reading was corrected, without
    /// changing its own recorded raw value (BR-051).
    /// </summary>
    internal void ApplyCascadedRecalculation(decimal operationalUsageAmount)
    {
        OperationalUsageAmount = operationalUsageAmount;
    }

    /// <summary>Applies a correction to this entry's operator attribution and shift times, without touching its reading or Operational Usage (used by <see cref="UsageLedger.CorrectEntryDetails"/>).</summary>
    internal void ApplyDetailsCorrection(Guid? operatorId, TimeOnly? shiftStartTime, TimeOnly? shiftEndTime)
    {
        OperatorId = operatorId;
        ShiftStartTime = shiftStartTime;
        ShiftEndTime = shiftEndTime;
    }
}
