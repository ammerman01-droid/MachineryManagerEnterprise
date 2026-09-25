using MachineryManagerEnterprise.SharedKernel;

namespace Consumption.Domain;

/// <summary>
/// Child Entity of <see cref="LubricantOverflowReport"/>: records that one
/// Personnel performed cleanup/handling work for the report, and for how
/// long. Recorded once per person for the whole report — even when the
/// report has several <see cref="LubricantOverflowLine"/>s — because the
/// same person's time is not split per line (chat, 2026-09-16).
/// <see cref="StartTime"/> and <see cref="Duration"/> are captured
/// separately (rather than only a duration) so that later reporting can
/// determine how much useful work a person performed within any given
/// time range, across many reports.
/// </summary>
public sealed class LubricantOverflowReportPersonnelEntry
{
    /// <summary>Gets the identifier of this entry, unique within its owning report.</summary>
    public Guid Id { get; private set; }

    /// <summary>Gets the identifier of the Personnel (Personnel module) who performed the work.</summary>
    public Guid PersonnelId { get; private set; }

    /// <summary>Gets the clock time the work started.</summary>
    public TimeOnly StartTime { get; private set; }

    /// <summary>Gets how long the work took.</summary>
    public TimeSpan Duration { get; private set; }

    private LubricantOverflowReportPersonnelEntry()
    {
    }

    private LubricantOverflowReportPersonnelEntry(Guid id, Guid personnelId, TimeOnly startTime, TimeSpan duration)
    {
        Id = id;
        PersonnelId = personnelId;
        StartTime = startTime;
        Duration = duration;
    }

    internal static Result<LubricantOverflowReportPersonnelEntry> Create(Guid personnelId, TimeOnly startTime, TimeSpan duration)
    {
        if (personnelId == Guid.Empty)
        {
            return Result.Failure<LubricantOverflowReportPersonnelEntry>(LubricantOverflowReportErrors.PersonnelRequired());
        }

        if (duration <= TimeSpan.Zero || duration > TimeSpan.FromHours(24))
        {
            return Result.Failure<LubricantOverflowReportPersonnelEntry>(LubricantOverflowReportErrors.DurationInvalid());
        }

        return new LubricantOverflowReportPersonnelEntry(Guid.NewGuid(), personnelId, startTime, duration);
    }
}
