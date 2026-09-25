namespace MachineryManagerEnterprise.Consumption.Application.Features.ConsumptionFreezeSettings.Dtos;

/// <summary>Represents the ConsumptionFreezeSettingDto data contract.</summary>
public sealed record ConsumptionFreezeSettingDto(Guid OrganizationId, DateOnly ThresholdDate);
