using MachineryManagerEnterprise.SharedKernel;
using MediatR;

namespace MachineryManagerEnterprise.Consumption.Application.Features.ConsumptionFreezeSettings.Commands.SetConsumptionFreezeThreshold;

/// <summary>
/// Sets (or moves) the Organization's Consumption freeze threshold.
/// Reports (currently: Lubricant Overflow Reports) with a date on or
/// before this threshold can no longer be edited or deleted. Restricted
/// to an Organization Administrator.
/// </summary>
public sealed record SetConsumptionFreezeThresholdCommand(Guid OrganizationId, DateOnly ThresholdDate) : IRequest<Result>;
