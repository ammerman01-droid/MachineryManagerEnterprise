using MachineryManagerEnterprise.SharedKernel;
using MediatR;

namespace MachineryManagerEnterprise.Usage.Application.Features.DailyUsage.Commands.RebaseCounter;

/// <summary>
/// Command to explicitly rebase a Usage Ledger's counter — the device
/// was replaced or its physical counter reset (chat, 2026-09-29). The
/// replacement device must already be installed on the same owner
/// (<see cref="global::Usage.Domain.MeterDevice.Install"/>) before
/// this command is issued. <paramref name="TargetDate"/> must be
/// strictly after the Ledger's last recorded entry and no later than
/// today — enforced in the Presentation layer's date picker and
/// re-validated here by the aggregate.
/// </summary>
public sealed record RebaseCounterCommand(
    global::Usage.Domain.UsageOwnerType OwnerType,
    Guid OwnerId,
    global::MachineryManagerEnterprise.SharedKernel.MeterReadingUnit Unit,
    Guid NewMeterDeviceId,
    decimal NewRawReadingValue,
    DateOnly TargetDate) : IRequest<Result>;
