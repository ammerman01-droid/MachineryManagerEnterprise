using MachineryManager.SharedKernel;

namespace Asset.Domain.Events;

/// <summary>Raised when an Asset is placed into operation (Commissioned → Operational, or Inactive → Operational).</summary>
public sealed class AssetActivated : IDomainEvent
{
    /// <summary>Gets the identifier of the activated asset.</summary>
    public AssetId AssetId { get; }

    /// <summary>Gets the UTC timestamp when the event occurred.</summary>
    public DateTimeOffset OccurredOn { get; }

    /// <summary>Initializes a new instance of the <see cref="AssetActivated"/> class.</summary>
    /// <param name="assetId">The identifier of the activated asset.</param>
    /// <param name="occurredOn">The UTC timestamp when the event occurred.</param>
    public AssetActivated(AssetId assetId, DateTimeOffset occurredOn)
    {
        AssetId = assetId;
        OccurredOn = occurredOn;
    }
}