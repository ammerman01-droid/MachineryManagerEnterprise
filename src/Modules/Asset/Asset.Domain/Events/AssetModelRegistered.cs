using MachineryManager.SharedKernel;

namespace Asset.Domain.Events;

/// <summary>Raised when a new Asset Model is registered.</summary>
public sealed class AssetModelRegistered : IDomainEvent
{
    /// <summary>Gets the identifier of the registered asset model.</summary>
    public AssetModelId AssetModelId { get; }

    /// <summary>Gets the identifier of the owning Holding.</summary>
    public Guid HoldingId { get; }

    /// <summary>Gets the name of the asset model.</summary>
    public string Name { get; }

    /// <summary>Gets the UTC timestamp when the event occurred.</summary>
    public DateTimeOffset OccurredOn { get; }

    /// <summary>Initializes a new instance of the <see cref="AssetModelRegistered"/> class.</summary>
    public AssetModelRegistered(AssetModelId assetModelId, Guid holdingId, string name, DateTimeOffset occurredOn)
    {
        AssetModelId = assetModelId;
        HoldingId = holdingId;
        Name = name;
        OccurredOn = occurredOn;
    }
}