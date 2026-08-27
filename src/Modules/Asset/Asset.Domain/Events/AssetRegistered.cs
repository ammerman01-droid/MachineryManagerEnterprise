using MachineryManager.SharedKernel;

namespace Asset.Domain.Events;

/// <summary>Raised when a new Asset is registered.</summary>
public sealed class AssetRegistered : IDomainEvent
{
    /// <summary>Gets the identifier of the registered asset.</summary>
    public AssetId AssetId { get; }

    /// <summary>Gets the identifier of the owning Organization (BR-003).</summary>
    public Guid OrganizationId { get; }

    /// <summary>Gets the identifier of the asset's shared specification catalog entry.</summary>
    public AssetModelId AssetModelId { get; }

    /// <summary>Gets the UTC timestamp when the event occurred.</summary>
    public DateTimeOffset OccurredOn { get; }

    /// <summary>Initializes a new instance of the <see cref="AssetRegistered"/> class.</summary>
    /// <param name="assetId">The identifier of the registered asset.</param>
    /// <param name="organizationId">The identifier of the owning Organization.</param>
    /// <param name="assetModelId">The identifier of the asset's shared specification catalog entry.</param>
    /// <param name="occurredOn">The UTC timestamp when the event occurred.</param>
    public AssetRegistered(AssetId assetId, Guid organizationId, AssetModelId assetModelId, DateTimeOffset occurredOn)
    {
        AssetId = assetId;
        OrganizationId = organizationId;
        AssetModelId = assetModelId;
        OccurredOn = occurredOn;
    }
}