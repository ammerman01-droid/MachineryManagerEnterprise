using MachineryManagerEnterprise.SharedKernel;
using MediatR;

namespace MachineryManagerEnterprise.Usage.Application.Features.DailyUsage.Queries.GetLastReading;

/// <summary>
/// Query for the most recent entry on the Usage Ledger of a Meter Device's current owner (chat,
/// 2026-10-12) — backs the "previous reading" and "operational usage" preview fields in the
/// register-usage dialog.
/// </summary>
/// <param name="MeterDeviceId">The device whose owner's latest ledger entry is wanted.</param>
public sealed record GetLastReadingQuery(Guid MeterDeviceId) : IRequest<Result<LastReadingDto>>;

/// <summary>The latest recorded reading for a device's owner, or all-<see langword="null"/> values when none exists yet.</summary>
/// <param name="Unit">The device's reading unit, by name (Hour, Kilometer, or Mile).</param>
/// <param name="RawReadingValue">The raw value of the ledger's last entry, or <see langword="null"/> when nothing has been recorded.</param>
/// <param name="EntryDate">The date of the ledger's last entry.</param>
/// <param name="ShiftIndex">The shift index of the last entry, or <see langword="null"/> when it is a counter rebase.</param>
/// <param name="SourceMeterDeviceId">The device that produced the last entry — a different device than the one being registered on means a counter rebase is required first.</param>
public sealed record LastReadingDto(
    string Unit,
    decimal? RawReadingValue,
    DateOnly? EntryDate,
    int? ShiftIndex,
    Guid? SourceMeterDeviceId);
