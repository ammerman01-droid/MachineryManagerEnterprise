using MachineryManagerEnterprise.SharedKernel;
using MachineryManagerEnterprise.SharedKernel.Abstractions;

namespace MachineryManagerEnterprise.Usage.Application.Common;

/// <summary>
/// Resolves the mandatory Work Calendar shifts that fall strictly
/// between a Usage Ledger's last recorded entry and a target
/// date/shift (chat, 2026-09-29) — the gap-fill input
/// <c>UsageLedger.RegisterShiftReading</c> and
/// <c>UsageLedger.RebaseCounter</c> need but cannot resolve
/// themselves (Usage.Domain must not depend on WorkCalendar). Shared
/// by both handlers so the traversal logic (remaining shifts on the
/// last entry's own day, every shift on each full day in between, and
/// — for a shift registration only — the shifts on the target day
/// before the target shift) is written once.
/// </summary>
public static class MandatoryShiftResolver
{
    /// <summary>
    /// Resolves the mandatory shifts strictly between
    /// <paramref name="lastEntry"/> and <paramref name="toDate"/>.
    /// </summary>
    /// <param name="workCalendarLookupService">Cross-module, read-only lookup into the WorkCalendar module.</param>
    /// <param name="projectId">The Project whose Work Calendar to resolve.</param>
    /// <param name="lastEntry">The Ledger's last recorded entry, or <see langword="null"/> for an empty Ledger (no gap to fill).</param>
    /// <param name="toDate">The date of the entry being registered.</param>
    /// <param name="toShiftIndexExclusive">
    /// For a shift-reading registration, the target shift's index — shifts on
    /// <paramref name="toDate"/> before this index are included. Pass
    /// <see langword="null"/> for a counter rebase, which is not tied
    /// to a shift and never consumes shifts on its own target date.
    /// </param>
    /// <param name="cancellationToken">Token to cancel the asynchronous operation.</param>
    public static async Task<Result<IReadOnlyList<global::Usage.Domain.MandatoryShift>>> ResolveAsync(
        IWorkCalendarLookupService workCalendarLookupService,
        Guid projectId,
        global::Usage.Domain.UsageEntry? lastEntry,
        DateOnly toDate,
        int? toShiftIndexExclusive,
        CancellationToken cancellationToken)
    {
        var missed = new List<global::Usage.Domain.MandatoryShift>();

        if (lastEntry is null)
        {
            // First entry ever recorded on this Ledger: establishes the baseline, nothing to gap-fill.
            return missed;
        }

        var fromDate = lastEntry.EntryDate;

        // Remaining shifts on the last entry's own day, after its shift index — only meaningful when
        // the last entry was itself a shift reading (a Rebase entry is not tied to a shift index, so
        // any shifts remaining on its day are left alone — a known simplification, chat 2026-09-29).
        if (lastEntry.Kind == global::Usage.Domain.UsageEntryKind.ShiftReading && lastEntry.ShiftIndex is { } lastShiftIndex)
        {
            var lastDaySchedule = await workCalendarLookupService.ResolveDayScheduleAsync(projectId, fromDate, cancellationToken);

            if (lastDaySchedule is null)
            {
                return Result.Failure<IReadOnlyList<global::Usage.Domain.MandatoryShift>>(CalendarNotResolved(fromDate));
            }

            if (lastDaySchedule.IsWorkingDay)
            {
                var upperBound = fromDate == toDate ? toShiftIndexExclusive : null;

                missed.AddRange(lastDaySchedule.Shifts
                    .Where(s => s.Index > lastShiftIndex && (upperBound is null || s.Index < upperBound))
                    .OrderBy(s => s.Index)
                    .Select(s => new global::Usage.Domain.MandatoryShift(fromDate, s.Index, s.StartTime, s.EndTime)));
            }
        }

        // Every shift on each full day strictly between the last entry's day and the target day.
        for (var day = fromDate.AddDays(1); day < toDate; day = day.AddDays(1))
        {
            var daySchedule = await workCalendarLookupService.ResolveDayScheduleAsync(projectId, day, cancellationToken);

            if (daySchedule is null)
            {
                return Result.Failure<IReadOnlyList<global::Usage.Domain.MandatoryShift>>(CalendarNotResolved(day));
            }

            if (daySchedule.IsWorkingDay)
            {
                missed.AddRange(daySchedule.Shifts
                    .OrderBy(s => s.Index)
                    .Select(s => new global::Usage.Domain.MandatoryShift(day, s.Index, s.StartTime, s.EndTime)));
            }
        }

        // Shifts on the target day itself, before the target shift — only for a shift-reading
        // registration (toShiftIndexExclusive is non-null), and only when the target day differs from
        // the last entry's day (the same-day case was already handled by the first block above).
        if (toShiftIndexExclusive is { } exclusiveIndex && toDate > fromDate)
        {
            var toDaySchedule = await workCalendarLookupService.ResolveDayScheduleAsync(projectId, toDate, cancellationToken);

            if (toDaySchedule is null)
            {
                return Result.Failure<IReadOnlyList<global::Usage.Domain.MandatoryShift>>(CalendarNotResolved(toDate));
            }

            if (toDaySchedule.IsWorkingDay)
            {
                missed.AddRange(toDaySchedule.Shifts
                    .Where(s => s.Index < exclusiveIndex)
                    .OrderBy(s => s.Index)
                    .Select(s => new global::Usage.Domain.MandatoryShift(toDate, s.Index, s.StartTime, s.EndTime)));
            }
        }

        return missed;
    }

    /// <summary>Creates the Application-level error for a date the Work Calendar could not resolve at all (as opposed to a resolved non-working day, which is not an error).</summary>
    public static Error CalendarNotResolved(DateOnly date) => Error.Validation(
        "Usage.CalendarNotResolved",
        $"No Work Calendar schedule could be resolved for {date:yyyy-MM-dd}; usage cannot be registered for this date.");

    /// <summary>Creates the Application-level error for a shift index that is not part of the resolved schedule for a date.</summary>
    public static Error ShiftNotScheduled(DateOnly date, int shiftIndex) => Error.Validation(
        "Usage.ShiftNotScheduled",
        $"Shift {shiftIndex} is not scheduled for {date:yyyy-MM-dd}.");
}
