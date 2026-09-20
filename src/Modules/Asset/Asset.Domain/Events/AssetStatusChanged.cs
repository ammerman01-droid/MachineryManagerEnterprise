using MachineryManagerEnterprise.SharedKernel;

namespace Asset.Domain.Events;

/// <summary>
/// Raised whenever an Asset's status changes (chat, 2026-09-20). Replaces
/// the former per-transition events (AssetCommissioned, AssetActivated,
/// AssetReactivated, AssetDeactivated, AssetRetired, AssetDisposed).
/// </summary>
public sealed class AssetStatusChanged : IDomainEvent
{
    /// <summary>Gets the identifier of the asset whose status changed.</summary>
    public AssetId AssetId { get; }

    /// <summary>Gets the status the asset had before the change.</summary>
    public AssetStatus PreviousStatus { get; }

    /// <summary>Gets the status the asset has after the change.</summary>
    public AssetStatus NewStatus { get; }

    /// <summary>Gets the UTC timestamp when the event occurred.</summary>
    public DateTimeOffset OccurredOn { get; }

    /// <summary>Initializes a new instance of the <see cref="AssetStatusChanged"/> class.</summary>
    /// <param name="assetId">The identifier of the asset whose status changed.</param>
    /// <param name="previousStatus">The status before the change.</param>
    /// <param name="newStatus">The status after the change.</param>
    /// <param name="occurredOn">The UTC timestamp when the event occurred.</param>
    public AssetStatusChanged(AssetId assetId, AssetStatus previousStatus, AssetStatus newStatus, DateTimeOffset occurredOn)
    {
        AssetId = assetId;
        PreviousStatus = previousStatus;
        NewStatus = newStatus;
        OccurredOn = occurredOn;
    }
}
