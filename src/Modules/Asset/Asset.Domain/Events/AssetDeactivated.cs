using MachineryManager.SharedKernel;

namespace Asset.Domain.Events;

/// <summary>Raised when an Asset is temporarily taken out of use (Operational → Inactive).</summary>
public sealed class AssetDeactivated : IDomainEvent
{
    /// <summary>Gets the identifier of the deactivated asset.</summary>
    public AssetId AssetId { get; }

    /// <summary>Gets the UTC timestamp when the event occurred.</summary>
    public DateTimeOffset OccurredOn { get; }

    /// <summary>Initializes a new instance of the <see cref="AssetDeactivated"/> class.</summary>
    /// <param name="assetId">The identifier of the deactivated asset.</param>
    /// <param name="occurredOn">The UTC timestamp when the event occurred.</param>
    public AssetDeactivated(AssetId assetId, DateTimeOffset occurredOn)
    {
        AssetId = assetId;
        OccurredOn = occurredOn;
    }
}