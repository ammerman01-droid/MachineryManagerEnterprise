using MachineryManager.SharedKernel;

namespace Asset.Domain.Events;

/// <summary>Raised when an Asset completes commissioning (Registered → Commissioned).</summary>
public sealed class AssetCommissioned : IDomainEvent
{
    /// <summary>Gets the identifier of the commissioned asset.</summary>
    public AssetId AssetId { get; }

    /// <summary>Gets the UTC timestamp when the event occurred.</summary>
    public DateTimeOffset OccurredOn { get; }

    /// <summary>Initializes a new instance of the <see cref="AssetCommissioned"/> class.</summary>
    /// <param name="assetId">The identifier of the commissioned asset.</param>
    /// <param name="occurredOn">The UTC timestamp when the event occurred.</param>
    public AssetCommissioned(AssetId assetId, DateTimeOffset occurredOn)
    {
        AssetId = assetId;
        OccurredOn = occurredOn;
    }
}