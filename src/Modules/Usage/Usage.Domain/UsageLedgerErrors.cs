using MachineryManagerEnterprise.SharedKernel;

namespace Usage.Domain;

/// <summary>Business Errors for the <see cref="UsageLedger"/> aggregate.</summary>
public static class UsageLedgerErrors
{
    /// <summary>Creates an error indicating no owner (Asset or Component) was supplied.</summary>
    public static Error OwnerRequired() => Error.Validation(
        "UsageLedger.OwnerRequired",
        "An owner (Asset or Component) is required for a Usage Ledger.");

    /// <summary>Creates an error indicating no Organization reference was supplied.</summary>
    public static Error OrganizationRequired() => Error.Validation(
        "UsageLedger.OrganizationRequired",
        "An Organization is required for a Usage Ledger.");

    /// <summary>Creates an error indicating the target date falls on or before the freeze boundary (BR-052/BR-053).</summary>
    public static Error EntryFrozen(DateOnly entryDate, DateOnly frozenUpToDate) => Error.Conflict(
        "UsageLedger.EntryFrozen",
        $"Usage entry for {entryDate:yyyy-MM-dd} is frozen (frozen up to and including {frozenUpToDate:yyyy-MM-dd}) and can no longer be registered, corrected, or deleted.");

    /// <summary>
    /// Creates an error indicating the shift being registered does not
    /// come strictly after the Ledger's last recorded entry — either it
    /// duplicates an already-registered shift, or it is out of order.
    /// Shift order and existence against the Work Calendar is resolved
    /// and validated by the Application layer before this aggregate is
    /// ever called (this aggregate cannot look the calendar up itself).
    /// </summary>
    public static Error NonSequentialShift(DateOnly lastEntryDate, int? lastShiftIndex, DateOnly attemptedDate, int attemptedShiftIndex) => Error.Validation(
        "UsageLedger.NonSequentialShift",
        $"Shift {attemptedShiftIndex} on {attemptedDate:yyyy-MM-dd} does not come after the last recorded entry ({attemptedDate:yyyy-MM-dd} shift {(lastShiftIndex?.ToString() ?? "-")} on {lastEntryDate:yyyy-MM-dd}).");

    /// <summary>
    /// Creates an error indicating a gap of missed mandatory shifts was
    /// found with a raw value that changed across the gap — per
    /// BR-044a this is rejected outright; the missing shifts shall be
    /// entered manually, one by one, or the counter rebased instead.
    /// </summary>
    public static Error GapRequiresManualEntry(DateOnly fromDate, int fromShiftIndex, DateOnly toDate, int toShiftIndex) => Error.Validation(
        "UsageLedger.GapRequiresManualEntry",
        $"Shifts from {fromDate:yyyy-MM-dd} (shift {fromShiftIndex}) to {toDate:yyyy-MM-dd} (shift {toShiftIndex}) were not reported and the value has changed; they shall be entered manually, one shift at a time, or the counter rebased.");

    /// <summary>
    /// Creates an error indicating a reading was submitted for a
    /// different source device than the Ledger's current device,
    /// without going through <see cref="UsageLedger.RebaseCounter"/>
    /// (chat, 2026-09-29) — the device change must be explicit.
    /// </summary>
    public static Error DifferentSourceDeviceRequiresRebase() => Error.Conflict(
        "UsageLedger.DifferentSourceDeviceRequiresRebase",
        "This reading comes from a different device than the Ledger's current one; use the counter rebase operation instead.");

    /// <summary>Creates an error indicating a rebase target date is not strictly after the last recorded entry's date.</summary>
    public static Error RebaseDateNotAfterLastEntry(DateOnly lastEntryDate, DateOnly attemptedTargetDate) => Error.Validation(
        "UsageLedger.RebaseDateNotAfterLastEntry",
        $"The rebase date {attemptedTargetDate:yyyy-MM-dd} must be after the last recorded entry date {lastEntryDate:yyyy-MM-dd}.");

    /// <summary>Creates an error indicating a rebase target date is in the future.</summary>
    public static Error RebaseDateInFuture(DateOnly today, DateOnly attemptedTargetDate) => Error.Validation(
        "UsageLedger.RebaseDateInFuture",
        $"The rebase date {attemptedTargetDate:yyyy-MM-dd} cannot be after today ({today:yyyy-MM-dd}).");

    /// <summary>
    /// Creates an error indicating a day's raw reading is lower than
    /// the raw value it is being compared against (a rollback, on the
    /// same device) — rejected per BR-018/BR-046.
    /// </summary>
    public static Error RawValueDecreased(decimal previousValue, decimal attemptedValue) => Error.Validation(
        "UsageLedger.RawValueDecreased",
        $"Raw value {attemptedValue} is lower than the previously recorded value {previousValue} for the same device.");

    /// <summary>
    /// Creates an error indicating a correction would move a later entry's reading below zero —
    /// corrections shift every later reading by the same amount to keep their usage unchanged
    /// (chat, 2026-10-14), and a counter cannot read a negative value.
    /// </summary>
    public static Error CorrectionMakesLaterReadingNegative() => Error.Validation(
        "UsageLedger.CorrectionMakesLaterReadingNegative",
        "This correction would move a later reading below zero.");

    /// <summary>Creates an error indicating only the most recently registered entry may be deleted (BR-049 — backward-only deletion).</summary>
    public static Error OnlyLatestEntryDeletable() => Error.Conflict(
        "UsageLedger.OnlyLatestEntryDeletable",
        "Only the most recently registered usage entry may be deleted; delete later entries first.");

    /// <summary>Creates an error indicating the ledger has no entries to delete.</summary>
    public static Error NoEntriesToDelete() => Error.Conflict(
        "UsageLedger.NoEntriesToDelete",
        "This Usage Ledger has no entries to delete.");

    /// <summary>Creates an error indicating no shift-reading entry exists for the given date and shift.</summary>
    public static Error EntryNotFound(DateOnly entryDate, int shiftIndex) => Error.NotFound(
        "UsageLedger.EntryNotFound",
        $"No usage entry exists for shift {shiftIndex} on {entryDate:yyyy-MM-dd}.");

    /// <summary>Creates an error indicating the freeze boundary was moved backward, which is not allowed.</summary>
    public static Error FreezeBoundaryCannotMoveBackward() => Error.Conflict(
        "UsageLedger.FreezeBoundaryCannotMoveBackward",
        "The freeze boundary can only move forward in time.");

    /// <summary>
    /// Creates an error indicating a day's total Operational Usage would exceed the daily limit
    /// (chat, 2026-10-13) — e.g. more than 24 hours or 1000 km registered for one day, which is
    /// almost certainly a typing mistake.
    /// </summary>
    public static Error DailyCapExceeded(DateOnly date, decimal dayTotal, decimal dailyCap) => Error.Validation(
        "UsageLedger.DailyCapExceeded",
        $"Operational usage for {date:yyyy-MM-dd} would total {dayTotal}, exceeding the daily limit of {dailyCap}.");

    /// <summary>Creates an error indicating the current user is not authorized to perform the requested action.</summary>
    public static Error NotAuthorized() => Error.Failure(
        "UsageLedger.NotAuthorized",
        "You do not have permission to perform this action.");
}
