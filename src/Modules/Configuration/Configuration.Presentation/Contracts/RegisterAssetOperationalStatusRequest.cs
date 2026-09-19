namespace MachineryManagerEnterprise.Configuration.Presentation.Contracts;

/// <summary>Represents a request to register an Asset operational status option within a Holding.</summary>
public sealed record RegisterAssetOperationalStatusRequest(Guid HoldingId, string Name);