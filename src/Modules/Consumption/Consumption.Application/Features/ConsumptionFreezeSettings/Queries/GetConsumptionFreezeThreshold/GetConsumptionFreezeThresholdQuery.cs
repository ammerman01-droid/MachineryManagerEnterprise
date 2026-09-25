using MachineryManagerEnterprise.Consumption.Application.Features.ConsumptionFreezeSettings.Dtos;
using MachineryManagerEnterprise.SharedKernel;
using MediatR;

namespace MachineryManagerEnterprise.Consumption.Application.Features.ConsumptionFreezeSettings.Queries.GetConsumptionFreezeThreshold;

/// <summary>
/// Retrieves the Consumption freeze threshold currently set for an
/// Organization. Succeeds with a <see langword="null"/> value (not a
/// NotFound error) when no threshold has ever been set — the absence of
/// a freeze is a normal, valid state, not an error.
/// </summary>
public sealed record GetConsumptionFreezeThresholdQuery(Guid OrganizationId) : IRequest<Result<ConsumptionFreezeSettingDto?>>;
