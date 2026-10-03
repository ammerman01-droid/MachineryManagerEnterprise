namespace MachineryManagerEnterprise.Usage.Presentation.Contracts;

/// <summary>HTTP request body for mounting a registered Meter Device on an owner.</summary>
/// <param name="OwnerType">The owner's kind, by name: Asset or Component.</param>
/// <param name="OwnerId">The owner's identifier.</param>
public sealed record InstallMeterDeviceRequest(string OwnerType, Guid OwnerId);
