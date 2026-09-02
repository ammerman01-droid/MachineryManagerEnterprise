using Asset.Domain.Events;
using MachineryManager.SharedKernel;
using MachineryManager.SharedKernel.Abstractions;

namespace Asset.Domain;

/// <summary>
/// Aggregate Root representing a catalog of shared specifications for a
/// class of Assets (e.g. "Volvo FH16 6x4"). Individual Asset instances
/// reference exactly one AssetModel (BR-002) and inherit its shared
/// specifications; identity-specific data (serial number, license
/// plate, manufacture year, color, ...) lives on the Asset itself, not
/// here (chat, 2026-08-25).
/// Scoped per Holding — shared across every Organization under that
/// Holding; only Asset identity records themselves are per-Organization
/// (correction, chat, 2026-08-26 — supersedes the earlier per-Organization
/// scope note).
/// </summary>
public sealed class AssetModel : AggregateRoot<AssetModelId>
{
    /// <summary>The maximum allowed length for the asset model's name.</summary>
    public const int MaxNameLength = 200;

    /// <summary>The maximum allowed length for the manufacturer's name.</summary>
    public const int MaxManufacturerLength = 200;

    private readonly List<Guid> _compatibleEngineModelIds = [];

/// <summary>Gets the identifier of the Holding that owns this catalog entry.</summary>
    public Guid HoldingId { get; private set; }

    /// <summary>Gets the display name of the asset model (e.g. "FH16 6x4").</summary>
    public string Name { get; private set; } = string.Empty;

    /// <summary>Gets the identifier of the manufacturer Company.</summary>
    public Guid CompanyId { get; private set; }

    /// <summary>
    /// Gets the Engine Models that are compatible with this Asset Model
    /// (i.e. may be installed on an Asset of this model).
    /// </summary>
    public IReadOnlyCollection<EngineModelId> CompatibleEngineModelIds =>
        _compatibleEngineModelIds.Select(EngineModelId.From).ToList().AsReadOnly();
// Reserved for ORM materialization only. Never used by application code.
    private AssetModel()
    {
    }

    private AssetModel(AssetModelId id, Guid holdingId, string name, Guid companyId)
        : base(id)
    {
        HoldingId = holdingId;
        Name = name;
        CompanyId = companyId;
    }

    /// <summary>Registers a new Asset Model.</summary>
    /// <param name="holdingId">The owning Holding.</param>
    /// <param name="name">The display name.</param>
    /// <param name="companyId">The manufacturer.</param>
    /// <param name="dateTimeProvider">Provides the current UTC time for the raised domain event.</param>
    /// <returns>A <see cref="Result{AssetModel}"/> containing the new aggregate, or a validation error.</returns>
    public static Result<AssetModel> Register(
        Guid holdingId,
        string name,
        Guid companyId,
        IDateTimeProvider dateTimeProvider)
    {
        if (string.IsNullOrWhiteSpace(name))
        {
            return Result.Failure<AssetModel>(AssetModelErrors.NameRequired());
        }

        if (name.Length > MaxNameLength)
        {
            return Result.Failure<AssetModel>(AssetModelErrors.NameTooLong(MaxNameLength));
        }

        var assetModel = new AssetModel(AssetModelId.New(), holdingId, name.Trim(), companyId);

        assetModel.RaiseDomainEvent(new AssetModelRegistered(
            assetModel.Id,
            holdingId,
            assetModel.Name,
            dateTimeProvider.UtcNow));

        return assetModel;
    }

    /// <summary>Renames this Asset Model.</summary>
    /// <param name="name">The new name.</param>
    /// <returns>A <see cref="Result"/> indicating success or a validation error.</returns>
    public Result Rename(string name)
    {
        if (string.IsNullOrWhiteSpace(name))
        {
            return Result.Failure(AssetModelErrors.NameRequired());
        }

        if (name.Length > MaxNameLength)
        {
            return Result.Failure(AssetModelErrors.NameTooLong(MaxNameLength));
        }

        Name = name.Trim();

        return Result.Success();
    }

    /// <summary>Marks the given Engine Model as compatible with this Asset Model.</summary>
    /// <param name="engineModelId">The engine model to mark compatible.</param>
    /// <param name="dateTimeProvider">Provides the current UTC time for the raised domain event.</param>
    /// <returns>A <see cref="Result"/> indicating success or a conflict if already compatible.</returns>
    public Result AssignCompatibleEngineModel(EngineModelId engineModelId, IDateTimeProvider dateTimeProvider)
    {
        if (_compatibleEngineModelIds.Contains(engineModelId.Value))
        {
            return Result.Failure(AssetModelErrors.EngineModelAlreadyCompatible());
        }

        _compatibleEngineModelIds.Add(engineModelId.Value);

        RaiseDomainEvent(new EngineModelAssignedToAssetModel(Id, engineModelId, dateTimeProvider.UtcNow));

        return Result.Success();
    }

    /// <summary>Removes a previously assigned compatibility with the given Engine Model.</summary>
    /// <param name="engineModelId">The engine model to remove.</param>
    /// <returns>A <see cref="Result"/> indicating success or a conflict if not currently compatible.</returns>
    public Result RemoveCompatibleEngineModel(EngineModelId engineModelId)
    {
        if (!_compatibleEngineModelIds.Remove(engineModelId.Value))
        {
            return Result.Failure(AssetModelErrors.EngineModelNotCompatible());
        }

        return Result.Success();
    }
}