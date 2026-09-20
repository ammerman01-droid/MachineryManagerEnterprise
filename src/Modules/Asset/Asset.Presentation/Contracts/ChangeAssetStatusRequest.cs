namespace MachineryManagerEnterprise.Asset.Presentation.Contracts;

/// <summary>Request body for changing an Asset's status.</summary>
/// <param name="Status">The target status by name: Active, Ready, OutOfService, or OutOfFleet.</param>
public sealed record ChangeAssetStatusRequest(string Status);
