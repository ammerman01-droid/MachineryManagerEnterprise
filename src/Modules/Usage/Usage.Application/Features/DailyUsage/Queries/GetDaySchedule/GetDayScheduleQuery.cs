using MachineryManagerEnterprise.SharedKernel;
using MediatR;

namespace MachineryManagerEnterprise.Usage.Application.Features.DailyUsage.Queries.GetDaySchedule;

/// <summary>
/// Query to resolve the Work Calendar day schedule for the Project a
/// Meter Device's current owner belongs to (chat, 2026-09-29) — backs
/// the shift-picker control in <c>RegisterShiftReadingDialog</c>. Kept
/// device-addressed, not ledger-addressed, so the Presentation layer
/// can offer it before any Usage Ledger exists yet (the device's very
/// first reading).
/// </summary>
public sealed record GetDayScheduleQuery(Guid MeterDeviceId, DateOnly Date) : IRequest<Result<DayScheduleDto>>;

/// <summary>A Work Calendar day's resolved schedule, for display in the shift picker.</summary>
/// <param name="IsWorkingDay">Whether the resolved date is a working day.</param>
/// <param name="Shifts">The day's shifts, ordered by <see cref="ShiftDto.Index"/>.</param>
public sealed record DayScheduleDto(bool IsWorkingDay, IReadOnlyList<ShiftDto> Shifts);

/// <summary>One shift within a resolved day schedule.</summary>
/// <param name="Index">The shift's stable index within its day.</param>
/// <param name="StartTime">The shift's scheduled start time.</param>
/// <param name="EndTime">The shift's scheduled end time.</param>
public sealed record ShiftDto(int Index, TimeOnly StartTime, TimeOnly EndTime);
