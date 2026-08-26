using MachineryManager.SharedKernel;

namespace Asset.Domain.Events;

/// <summary>Raised when an Engine Model is marked compatible with an Asset Model.</summary>
public sealed class EngineModelAssignedToAssetModel : IDomainEvent
{
    /// <summary>Gets the identifier of the asset model.</summary>
    public AssetModelId AssetModelId { get; }

    /// <summary>Gets the identifier of the engine model now marked compatible.</summary>
    public EngineModelId EngineModelId { get; }

    /// <summary>Gets the UTC timestamp when the event occurred.</summary>
    public DateTimeOffset OccurredOn { get; }

    /// <summary>Initializes a new instance of the <see cref="EngineModelAssignedToAssetModel"/> class.</summary>
    public EngineModelAssignedToAssetModel(AssetModelId assetModelId, EngineModelId engineModelId, DateTimeOffset occurredOn)
    {
        AssetModelId = assetModelId;
        EngineModelId = engineModelId;
        OccurredOn = occurredOn;
    }
}