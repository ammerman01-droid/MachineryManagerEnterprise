namespace MachineryManager.Asset.Presentation.Contracts;

/// <summary>Request body for registering a new Asset.</summary>
public sealed record RegisterAssetRequest(
    Guid OrganizationId,
    string Code,
    string Name,
    Guid AssetModelId,
    Guid ColorId,
    string? SerialNumber,
    string? ChassisNumber,
    string? BodyNumber,
    string? Vin,
    string? LicensePlate,
    int? ManufactureYear);