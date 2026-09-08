using MachineryManager.SharedKernel;

namespace Asset.Domain.Events;

/// <summary>Raised when an Asset is physically disposed of (final state, BR-004 — history is preserved).</summary>
public sealed class AssetDisposed : IDomainEvent
{
    /// <summary>Gets the identifier of the disposed asset.</summary>
    public AssetId AssetId { get; }

    /// <summary>Gets the UTC timestamp when the event occurred.</summary>
    public DateTimeOffset OccurredOn { get; }

    /// <summary>Initializes a new instance of the <see cref="AssetDisposed"/> class.</summary>
    /// <param name="assetId">The identifier of the disposed asset.</param>
    /// <param name="occurredOn">The UTC timestamp when the event occurred.</param>
    public AssetDisposed(AssetId assetId, DateTimeOffset occurredOn)
    {
        AssetId = assetId;
        OccurredOn = occurredOn;
    }
}