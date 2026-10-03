using MachineryManagerEnterprise.SharedKernel;
using MachineryManagerEnterprise.SharedKernel.Abstractions;
using Usage.Domain.Events;

namespace Usage.Domain;

/// <summary>
/// Aggregate Root holding the full append-only usage timeline for one
/// owner (an Asset or a Tracked Component) and one
/// <see cref="MeterReadingUnit"/> — one Ledger per (Owner, Unit). This
/// is the heart of the Meter/Counter module (chat, 2026-09-29 redesign
/// to per-shift granularity): shift-sequential entry, automatic
/// gap-filling of missed mandatory shifts (BR-044), rejection of
/// unreported increases across a gap (BR-044a), corrections with
/// downstream-chain cascading recalculation (BR-051 — see
/// <see cref="CorrectEntryReading"/>), backward-only deletion
/// (BR-049), explicit counter rebasing on a device change, the freeze
/// boundary (BR-052/BR-053), and a per-entry Project snapshot for
/// historical reporting accuracy across Project transfers (BR-054 —
/// see <see cref="UsageEntry.ProjectId"/>). Deliberately independent
/// from <see cref="MeterDevice"/> (BR-010): Operational Usage is never
/// read directly off a device's current value (BR-015). This
/// aggregate never resolves the Work Calendar itself — the Application
/// layer resolves it and passes in any <see cref="MandatoryShift"/>s
/// that must be gap-filled (Modular Monolith boundary).
/// </summary>
public sealed class UsageLedger : AggregateRoot<UsageLedgerId>
{
    private readonly List<UsageEntry> _entries = [];

    /// <summary>Gets the kind of this ledger's owner (Asset or Component).</summary>
    public UsageOwnerType OwnerType { get; private set; }

    /// <summary>Gets the identifier of this ledger's owner.</summary>
    public Guid OwnerId { get; private set; }

    /// <summary>
    /// Gets the identifier of the Organization this ledger belongs to
    /// — denormalized from the owner at <see cref="Create"/> time
    /// (chat, 2026-09-12) so every subsequent operation can resolve a
    /// permission scope without a fresh cross-module lookup each time.
    /// </summary>
    public Guid OrganizationId { get; private set; }

    /// <summary>Gets the unit this ledger tracks (Hour, Kilometer, or Mile), matching the owning device's <see cref="MeterDevice.Unit"/>.</summary>
    public MeterReadingUnit Unit { get; private set; }

    /// <summary>
    /// Gets the freeze boundary (inclusive), or <see langword="null"/>
    /// if nothing has been frozen yet. Entries dated on or before this
    /// date can no longer be registered, corrected, or deleted by
    /// project-level users (BR-052/BR-053). Scoped to this single
    /// ledger (chat, 2026-09-12) — freezing an entire Project's worth
    /// of Assets is an Application Service concern that calls
    /// <see cref="FreezeUpTo"/> once per affected ledger.
    /// </summary>
    public DateOnly? FrozenUpToDate { get; private set; }

    /// <summary>Gets the full timeline, ordered by <see cref="UsageEntry.Sequence"/> ascending — the sole ordering key.</summary>
    public IReadOnlyCollection<UsageEntry> Entries => _entries.AsReadOnly();

    // Reserved for ORM materialization only. Never used by application code.
    private UsageLedger()
    {
    }

    private UsageLedger(UsageLedgerId id, UsageOwnerType ownerType, Guid ownerId, Guid organizationId, MeterReadingUnit unit)
        : base(id)
    {
        OwnerType = ownerType;
        OwnerId = ownerId;
        OrganizationId = organizationId;
        Unit = unit;
    }

    /// <summary>Creates a new, empty Usage Ledger for an owner and unit. No Domain Event is raised — the ledger becomes observable once its first entry is registered.</summary>
    public static Result<UsageLedger> Create(UsageOwnerType ownerType, Guid ownerId, Guid organizationId, MeterReadingUnit unit)
    {
        if (ownerId == Guid.Empty)
        {
            return Result.Failure<UsageLedger>(UsageLedgerErrors.OwnerRequired());
        }

        if (organizationId == Guid.Empty)
        {
            return Result.Failure<UsageLedger>(UsageLedgerErrors.OrganizationRequired());
        }

        return new UsageLedger(UsageLedgerId.New(), ownerType, ownerId, organizationId, unit);
    }

    /// <summary>
    /// Registers a reading for one Work Calendar shift.
    /// <paramref name="entryDate"/>/<paramref name="shiftIndex"/> must
    /// come strictly after the Ledger's last recorded entry, except for
    /// the very first entry on an empty ledger, which may be any shift
    /// (it establishes the baseline, no usage counted yet). When
    /// <paramref name="missedMandatoryShifts"/> is non-empty (the
    /// Application layer resolved mandatory shifts between the last
    /// entry and this one that were never reported), those shifts are
    /// auto-filled with zero usage (BR-044) — but only if
    /// <paramref name="rawReadingValue"/> is unchanged from the last
    /// entry; otherwise the gap is rejected outright (BR-044a) and the
    /// missing shifts must be entered manually, or the counter rebased.
    /// A reading from a different device than the Ledger's current one
    /// is always rejected — use <see cref="RebaseCounter"/> instead.
    /// </summary>
    /// <param name="entryDate">The calendar date of the shift being reported.</param>
    /// <param name="shiftIndex">The Work Calendar shift index being reported, resolved by the Application layer.</param>
    /// <param name="shiftStartTime">The shift's scheduled start time, resolved by the Application layer.</param>
    /// <param name="shiftEndTime">The shift's scheduled end time, resolved by the Application layer.</param>
    /// <param name="rawReadingValue">The raw value shown on the device at the end of this shift.</param>
    /// <param name="sourceMeterDeviceId">The device that produced <paramref name="rawReadingValue"/> (BR-012); must match the Ledger's current device.</param>
    /// <param name="operatorId">The operator this shift is attributed to, when supplied.</param>
    /// <param name="projectId">The Project the owner belongs to right now, resolved by the Application layer and snapshotted onto this entry and any gap-fill entries (BR-054).</param>
    /// <param name="missedMandatoryShifts">Mandatory shifts between the last recorded entry and this one that were never reported, resolved by the Application layer from the Work Calendar, in chronological order.</param>
    /// <param name="dateTimeProvider">Supplies the current UTC time for raised events.</param>
    public Result RegisterShiftReading(
        DateOnly entryDate,
        int shiftIndex,
        TimeOnly shiftStartTime,
        TimeOnly shiftEndTime,
        decimal rawReadingValue,
        MeterDeviceId sourceMeterDeviceId,
        Guid? operatorId,
        Guid projectId,
        IReadOnlyList<MandatoryShift> missedMandatoryShifts,
        IDateTimeProvider dateTimeProvider)
    {
        if (FrozenUpToDate is { } frozenDate && entryDate <= frozenDate)
        {
            return Result.Failure(UsageLedgerErrors.EntryFrozen(entryDate, frozenDate));
        }

        if (rawReadingValue < 0)
        {
            return Result.Failure(MeterDeviceErrors.NegativeReadingValue());
        }

        var lastEntry = _entries.Count > 0 ? _entries[^1] : null;
        decimal rawDelta;

        if (lastEntry is null)
        {
            // First entry ever recorded on this ledger: establishes the baseline, no usage counted yet (chat, 2026-09-12).
            rawDelta = 0m;
        }
        else
        {
            if ((entryDate, shiftIndex).CompareTo((lastEntry.EntryDate, lastEntry.ShiftIndex ?? int.MinValue)) <= 0)
            {
                return Result.Failure(UsageLedgerErrors.NonSequentialShift(lastEntry.EntryDate, lastEntry.ShiftIndex, entryDate, shiftIndex));
            }

            if (sourceMeterDeviceId != lastEntry.SourceMeterDeviceId)
            {
                return Result.Failure(UsageLedgerErrors.DifferentSourceDeviceRequiresRebase());
            }

            if (missedMandatoryShifts.Count > 0)
            {
                if (rawReadingValue != lastEntry.RawReadingValue)
                {
                    var first = missedMandatoryShifts[0];
                    var last = missedMandatoryShifts[^1];
                    return Result.Failure(UsageLedgerErrors.GapRequiresManualEntry(first.Date, first.ShiftIndex, last.Date, last.ShiftIndex));
                }

                rawDelta = 0m;
            }
            else
            {
                rawDelta = rawReadingValue - lastEntry.RawReadingValue;
                if (rawDelta < 0)
                {
                    return Result.Failure(UsageLedgerErrors.RawValueDecreased(lastEntry.RawReadingValue, rawReadingValue));
                }
            }
        }

        var sequence = _entries.Count;
        var gapFillBaselineRaw = lastEntry?.RawReadingValue ?? rawReadingValue;

        foreach (var missed in missedMandatoryShifts)
        {
            var gapFillEntry = new UsageEntry(
                UsageEntryId.New(),
                sequence++,
                UsageEntryKind.ShiftReading,
                UsageEntryOrigin.AutoFilled,
                missed.Date,
                missed.ShiftIndex,
                missed.StartTime,
                missed.EndTime,
                gapFillBaselineRaw,
                sourceMeterDeviceId,
                operationalUsageAmount: 0m,
                operatorId: null,
                projectId: projectId);

            _entries.Add(gapFillEntry);

            RaiseDomainEvent(new UsageEntryRegistered(
                Id, OwnerType, OwnerId, Unit, missed.Date, missed.ShiftIndex, UsageEntryOrigin.AutoFilled, 0m, dateTimeProvider.UtcNow));
        }

        _entries.Add(new UsageEntry(
            UsageEntryId.New(),
            sequence,
            UsageEntryKind.ShiftReading,
            UsageEntryOrigin.Manual,
            entryDate,
            shiftIndex,
            shiftStartTime,
            shiftEndTime,
            rawReadingValue,
            sourceMeterDeviceId,
            rawDelta,
            operatorId,
            projectId));

        RaiseDomainEvent(new UsageEntryRegistered(
            Id, OwnerType, OwnerId, Unit, entryDate, shiftIndex, UsageEntryOrigin.Manual, rawDelta, dateTimeProvider.UtcNow));

        return Result.Success();
    }

    /// <summary>
    /// Explicitly rebases this Ledger's counter — the device was
    /// replaced or its physical counter reset (chat, 2026-09-29).
    /// <paramref name="targetDate"/> must be strictly after the last
    /// recorded entry's date and no later than today. Any
    /// <paramref name="missedMandatoryShifts"/> between the last entry
    /// and <paramref name="targetDate"/> — resolved by the Application
    /// layer from the Work Calendar — are unconditionally auto-filled
    /// with zero usage, mirroring the old device's last raw value
    /// (approved default, chat 2026-09-29). The rebase itself never
    /// contributes Operational Usage.
    /// </summary>
    /// <param name="newRawReadingValue">The new baseline raw value shown on the (possibly new) device.</param>
    /// <param name="newSourceMeterDeviceId">The device the Ledger reads from going forward.</param>
    /// <param name="targetDate">The date to record the rebase on; must be after the last entry and not in the future.</param>
    /// <param name="projectId">The Project the owner belongs to right now, snapshotted onto the rebase entry and any gap-fill entries (BR-054).</param>
    /// <param name="missedMandatoryShifts">Mandatory shifts between the last recorded entry and <paramref name="targetDate"/> that were never reported, resolved by the Application layer, in chronological order.</param>
    /// <param name="today">Today's date, supplied by the Application layer, used to reject a future <paramref name="targetDate"/>.</param>
    /// <param name="dateTimeProvider">Supplies the current UTC time for raised events.</param>
    public Result RebaseCounter(
        decimal newRawReadingValue,
        MeterDeviceId newSourceMeterDeviceId,
        DateOnly targetDate,
        Guid projectId,
        IReadOnlyList<MandatoryShift> missedMandatoryShifts,
        DateOnly today,
        IDateTimeProvider dateTimeProvider)
    {
        if (FrozenUpToDate is { } frozenDate && targetDate <= frozenDate)
        {
            return Result.Failure(UsageLedgerErrors.EntryFrozen(targetDate, frozenDate));
        }

        if (newRawReadingValue < 0)
        {
            return Result.Failure(MeterDeviceErrors.NegativeReadingValue());
        }

        if (targetDate > today)
        {
            return Result.Failure(UsageLedgerErrors.RebaseDateInFuture(today, targetDate));
        }

        var lastEntry = _entries.Count > 0 ? _entries[^1] : null;
        var sequence = _entries.Count;

        if (lastEntry is not null)
        {
            if (targetDate <= lastEntry.EntryDate)
            {
                return Result.Failure(UsageLedgerErrors.RebaseDateNotAfterLastEntry(lastEntry.EntryDate, targetDate));
            }

            foreach (var missed in missedMandatoryShifts)
            {
                var gapFillEntry = new UsageEntry(
                    UsageEntryId.New(),
                    sequence++,
                    UsageEntryKind.ShiftReading,
                    UsageEntryOrigin.AutoFilled,
                    missed.Date,
                    missed.ShiftIndex,
                    missed.StartTime,
                    missed.EndTime,
                    lastEntry.RawReadingValue,
                    lastEntry.SourceMeterDeviceId,
                    operationalUsageAmount: 0m,
                    operatorId: null,
                    projectId: projectId);

                _entries.Add(gapFillEntry);

                RaiseDomainEvent(new UsageEntryRegistered(
                    Id, OwnerType, OwnerId, Unit, missed.Date, missed.ShiftIndex, UsageEntryOrigin.AutoFilled, 0m, dateTimeProvider.UtcNow));
            }
        }

        _entries.Add(new UsageEntry(
            UsageEntryId.New(),
            sequence,
            UsageEntryKind.Rebase,
            UsageEntryOrigin.Manual,
            targetDate,
            shiftIndex: null,
            shiftStartTime: null,
            shiftEndTime: null,
            newRawReadingValue,
            newSourceMeterDeviceId,
            operationalUsageAmount: 0m,
            operatorId: null,
            projectId: projectId));

        RaiseDomainEvent(new CounterRebased(Id, targetDate, newSourceMeterDeviceId, newRawReadingValue, dateTimeProvider.UtcNow));

        return Result.Success();
    }

    /// <summary>
    /// Corrects an already-registered shift reading's raw value
    /// (BR-051), as long as it is not yet frozen (BR-052/BR-053). The
    /// device that produced the reading, the Project it was
    /// snapshotted under (BR-054), the operator, and the shift times
    /// cannot be changed by this operation — see
    /// <see cref="CorrectEntryDetails"/> for those.
    /// <para>
    /// Any run of auto-filled shifts (BR-044) immediately after the
    /// corrected entry mirrors the corrected raw value forward — they
    /// stay at zero usage. The first manually-entered shift after that
    /// run has its Operational Usage recalculated once, since it is the
    /// only entry whose own delta is computed from this chain's raw
    /// value. Propagation stops at that entry, or immediately at a
    /// <see cref="UsageEntryKind.Rebase"/> entry (which never derives
    /// its own numbers from a previous entry).
    /// </para>
    /// </summary>
    /// <param name="entryDate">The date of the shift reading being corrected.</param>
    /// <param name="shiftIndex">The shift index of the reading being corrected.</param>
    /// <param name="newRawReadingValue">The corrected raw value.</param>
    /// <param name="dateTimeProvider">Supplies the current UTC time for raised events.</param>
    public Result CorrectEntryReading(
        DateOnly entryDate,
        int shiftIndex,
        decimal newRawReadingValue,
        IDateTimeProvider dateTimeProvider)
    {
        if (FrozenUpToDate is { } frozenDate && entryDate <= frozenDate)
        {
            return Result.Failure(UsageLedgerErrors.EntryFrozen(entryDate, frozenDate));
        }

        if (newRawReadingValue < 0)
        {
            return Result.Failure(MeterDeviceErrors.NegativeReadingValue());
        }

        var index = _entries.FindIndex(e => e.Kind == UsageEntryKind.ShiftReading && e.EntryDate == entryDate && e.ShiftIndex == shiftIndex);
        if (index < 0)
        {
            return Result.Failure(UsageLedgerErrors.EntryNotFound(entryDate, shiftIndex));
        }

        var entry = _entries[index];
        var previousEntry = index > 0 ? _entries[index - 1] : null;

        var rawDelta = previousEntry is null || previousEntry.SourceMeterDeviceId != entry.SourceMeterDeviceId
            ? 0m
            : newRawReadingValue - previousEntry.RawReadingValue;

        if (rawDelta < 0)
        {
            return Result.Failure(UsageLedgerErrors.RawValueDecreased(previousEntry!.RawReadingValue, newRawReadingValue));
        }

        // Phase 1 — walk the dependency chain and validate every step, WITHOUT mutating anything yet.
        var mirroredGapFills = new List<UsageEntry>();
        UsageEntry? recalculatedRealEntry = null;
        var recalculatedOperationalUsageAmount = 0m;

        var carriedRawValue = newRawReadingValue;
        var cursor = index + 1;

        while (cursor < _entries.Count)
        {
            var candidate = _entries[cursor];

            if (candidate.Kind == UsageEntryKind.Rebase)
            {
                // A rebase never derives its own numbers from a previous entry — propagation stops here.
                break;
            }

            if (candidate.Origin == UsageEntryOrigin.AutoFilled)
            {
                // An auto-filled shift mirrors the previous real value by construction (BR-044): keep it in sync, stay at zero, keep walking.
                mirroredGapFills.Add(candidate);
                cursor++;
                continue;
            }

            // First manually-entered shift after the (possibly empty) run of auto-fills: recompute once, then propagation provably stops (see method summary).
            var candidateRawDelta = candidate.SourceMeterDeviceId != entry.SourceMeterDeviceId
                ? 0m
                : candidate.RawReadingValue - carriedRawValue;

            if (candidateRawDelta < 0)
            {
                return Result.Failure(UsageLedgerErrors.RawValueDecreased(carriedRawValue, candidate.RawReadingValue));
            }

            recalculatedRealEntry = candidate;
            recalculatedOperationalUsageAmount = candidateRawDelta;

            break;
        }

        // Phase 2 — every step validated; apply all changes together.
        entry.ApplyReadingCorrection(newRawReadingValue, rawDelta);

        foreach (var gapFill in mirroredGapFills)
        {
            gapFill.ApplyReadingCorrection(carriedRawValue, 0m);
        }

        recalculatedRealEntry?.ApplyCascadedRecalculation(recalculatedOperationalUsageAmount);

        RaiseDomainEvent(new UsageEntryAdjusted(Id, OwnerType, OwnerId, entryDate, shiftIndex, rawDelta, dateTimeProvider.UtcNow));

        if (recalculatedRealEntry is not null)
        {
            RaiseDomainEvent(new UsageEntryAdjusted(
                Id, OwnerType, OwnerId, recalculatedRealEntry.EntryDate, recalculatedRealEntry.ShiftIndex!.Value, recalculatedOperationalUsageAmount, dateTimeProvider.UtcNow));
        }

        return Result.Success();
    }

    /// <summary>
    /// Corrects a shift reading's operator attribution and/or shift
    /// times without touching its reading or Operational Usage — no
    /// recalculation, no cascade. Not yet frozen (BR-052/BR-053).
    /// </summary>
    /// <param name="entryDate">The date of the shift reading being corrected.</param>
    /// <param name="shiftIndex">The shift index of the reading being corrected.</param>
    /// <param name="operatorId">The corrected operator attribution, or <see langword="null"/> to clear it.</param>
    /// <param name="shiftStartTime">The corrected shift start time.</param>
    /// <param name="shiftEndTime">The corrected shift end time.</param>
    public Result CorrectEntryDetails(
        DateOnly entryDate,
        int shiftIndex,
        Guid? operatorId,
        TimeOnly shiftStartTime,
        TimeOnly shiftEndTime)
    {
        if (FrozenUpToDate is { } frozenDate && entryDate <= frozenDate)
        {
            return Result.Failure(UsageLedgerErrors.EntryFrozen(entryDate, frozenDate));
        }

        var entry = _entries.Find(e => e.Kind == UsageEntryKind.ShiftReading && e.EntryDate == entryDate && e.ShiftIndex == shiftIndex);
        if (entry is null)
        {
            return Result.Failure(UsageLedgerErrors.EntryNotFound(entryDate, shiftIndex));
        }

        entry.ApplyDetailsCorrection(operatorId, shiftStartTime, shiftEndTime);

        return Result.Success();
    }

    /// <summary>
    /// Deletes the most recently registered entry — shift reading or
    /// rebase (backward-only: an earlier entry cannot be deleted while
    /// a later one still exists), as long as it is not yet frozen
    /// (BR-052/BR-053).
    /// </summary>
    /// <param name="entryDate">The date of the entry expected to be the most recent one.</param>
    /// <param name="shiftIndex">The shift index expected on the most recent entry, or <see langword="null"/> if it is expected to be a rebase.</param>
    /// <param name="dateTimeProvider">Supplies the current UTC time for raised events.</param>
    public Result DeleteLatestEntry(DateOnly entryDate, int? shiftIndex, IDateTimeProvider dateTimeProvider)
    {
        if (FrozenUpToDate is { } frozenDate && entryDate <= frozenDate)
        {
            return Result.Failure(UsageLedgerErrors.EntryFrozen(entryDate, frozenDate));
        }

        if (_entries.Count == 0)
        {
            return Result.Failure(UsageLedgerErrors.NoEntriesToDelete());
        }

        var latest = _entries[^1];
        if (latest.EntryDate != entryDate || latest.ShiftIndex != shiftIndex)
        {
            return Result.Failure(UsageLedgerErrors.OnlyLatestEntryDeletable());
        }

        _entries.RemoveAt(_entries.Count - 1);

        RaiseDomainEvent(new UsageEntryDeleted(Id, entryDate, latest.OperationalUsageAmount, dateTimeProvider.UtcNow));

        return Result.Success();
    }

    /// <summary>
    /// Moves the freeze boundary forward (BR-052). Once frozen, entries
    /// dated on or before <paramref name="frozenUpToDate"/> become
    /// immutable to project-level users (BR-053) — the boundary can
    /// never move backward.
    /// </summary>
    public Result FreezeUpTo(DateOnly frozenUpToDate, IDateTimeProvider dateTimeProvider)
    {
        if (FrozenUpToDate is { } currentBoundary && frozenUpToDate < currentBoundary)
        {
            return Result.Failure(UsageLedgerErrors.FreezeBoundaryCannotMoveBackward());
        }

        FrozenUpToDate = frozenUpToDate;

        RaiseDomainEvent(new UsageFrozen(Id, frozenUpToDate, dateTimeProvider.UtcNow));

        return Result.Success();
    }

    /// <summary>
    /// Re-establishes ascending <see cref="UsageEntry.Sequence"/> order on the
    /// in-memory timeline. EF Core does not guarantee the materialization
    /// order of an owned collection, yet every cursor-based operation of this
    /// aggregate (<see cref="RegisterShiftReading"/>, <see cref="CorrectEntryReading"/>,
    /// <see cref="DeleteLatestEntry"/>, ...) relies on <c>_entries</c> being
    /// sorted by <c>Sequence</c>. The persistence layer calls this immediately
    /// after loading the aggregate (chat, 2026-09-29); it has no other effect
    /// and raises no domain event.
    /// </summary>
    public void EnsureEntriesOrderedBySequence()
    {
        _entries.Sort(static (left, right) => left.Sequence.CompareTo(right.Sequence));
    }
}