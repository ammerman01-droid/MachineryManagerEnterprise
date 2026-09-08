using MachineryManager.SharedKernel;

namespace Asset.Domain.Events;

/// <summary>Raised when an Asset is permanently withdrawn from use.</summary>
public sealed class AssetRetired : IDomainEvent
{
    /// <summary>Gets the identifier of the retired asset.</summary>
    public AssetId AssetId { get; }

    /// <summary>Gets the UTC timestamp when the event occurred.</summary>
    public DateTimeOffset OccurredOn { get; }

    /// <summary>Initializes a new instance of the <see cref="AssetRetired"/> class.</summary>
    /// <param name="assetId">The identifier of the retired asset.</param>
    /// <param name="occurredOn">The UTC timestamp when the event occurred.</param>
    public AssetRetired(AssetId assetId, DateTimeOffset occurredOn)
    {
        AssetId = assetId;
        OccurredOn = occurredOn;
    }
}