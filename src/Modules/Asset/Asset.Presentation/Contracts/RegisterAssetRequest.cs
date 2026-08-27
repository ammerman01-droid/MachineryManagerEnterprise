namespace MachineryManager.Asset.Presentation.Contracts;

/// <summary>Request body for registering a new Asset.</summary>
public sealed record RegisterAssetRequest(
    Guid OrganizationId,
    Guid AssetModelId,
    string Color,
    string? SerialNumber,
    string? LicensePlate,
    int? ManufactureYear);
