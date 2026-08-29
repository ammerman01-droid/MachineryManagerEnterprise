namespace MachineryManager.Asset.Presentation.Contracts;

/// <summary>Request body for registering a new Asset.</summary>
public sealed record RegisterAssetRequest(
    Guid OrganizationId,
    string Code,
    Guid AssetModelId,
    string Color,
    string? SerialNumber,
    string? LicensePlate,
    int? ManufactureYear);