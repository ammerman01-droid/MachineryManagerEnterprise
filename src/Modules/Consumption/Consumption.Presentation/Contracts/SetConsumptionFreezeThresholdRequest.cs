namespace MachineryManagerEnterprise.Consumption.Presentation.Contracts;

/// <summary>Request body for setting an Organization's Consumption freeze threshold.</summary>
/// <param name="ThresholdDate">Reports dated on or before this date can no longer be edited or deleted.</param>
public sealed record SetConsumptionFreezeThresholdRequest(DateOnly ThresholdDate);
