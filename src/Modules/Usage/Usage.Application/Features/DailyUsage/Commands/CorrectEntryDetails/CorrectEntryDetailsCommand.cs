using MachineryManagerEnterprise.SharedKernel;
using MediatR;

namespace MachineryManagerEnterprise.Usage.Application.Features.DailyUsage.Commands.CorrectEntryDetails;

/// <summary>
/// Command to correct a shift reading's operator attribution and/or
/// shift times, without touching its reading or Operational Usage —
/// no recalculation, no cascade (chat, 2026-09-29).
/// </summary>
public sealed record CorrectEntryDetailsCommand(
    global::Usage.Domain.UsageOwnerType OwnerType,
    Guid OwnerId,
    global::MachineryManagerEnterprise.SharedKernel.MeterReadingUnit Unit,
    DateOnly EntryDate,
    int ShiftIndex,
    Guid? OperatorId,
    TimeOnly ShiftStartTime,
    TimeOnly ShiftEndTime) : IRequest<Result>;
