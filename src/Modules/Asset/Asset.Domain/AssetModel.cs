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

    /// <summary>Gets the length value, or <see langword="null"/> if not specified (chat, 2026-09-04).</summary>
    public decimal? LengthValue { get; private set; }

    /// <summary>
    /// Gets the identifier of the Unit of Measurement for
    /// <see cref="LengthValue"/>, or <see langword="null"/> if not
    /// specified. References the Configuration module's
    /// UnitOfMeasurement aggregate by plain Guid — cross-module, no
    /// strongly-typed reference or database-level FK, mirroring how
    /// <see cref="HoldingId"/> references the Organization module.
    /// </summary>
    public Guid? LengthUnitOfMeasurementId { get; private set; }

    /// <summary>Gets the width value, or <see langword="null"/> if not specified.</summary>
    public decimal? WidthValue { get; private set; }

    /// <summary>Gets the identifier of the Unit of Measurement for <see cref="WidthValue"/>, or <see langword="null"/> if not specified.</summary>
    public Guid? WidthUnitOfMeasurementId { get; private set; }

    /// <summary>Gets the height value, or <see langword="null"/> if not specified.</summary>
    public decimal? HeightValue { get; private set; }

    /// <summary>Gets the identifier of the Unit of Measurement for <see cref="HeightValue"/>, or <see langword="null"/> if not specified.</summary>
    public Guid? HeightUnitOfMeasurementId { get; private set; }

    /// <summary>Gets the weight value, or <see langword="null"/> if not specified.</summary>
    public decimal? WeightValue { get; private set; }

    /// <summary>Gets the identifier of the Unit of Measurement for <see cref="WeightValue"/>, or <see langword="null"/> if not specified.</summary>
    public Guid? WeightUnitOfMeasurementId { get; private set; }

    /// <summary>
    /// Gets the volume-based working capacity value (e.g. bucket
    /// capacity in m³/liters), or <see langword="null"/> if not
    /// specified (chat, 2026-09-04: distinct from
    /// <see cref="WorkingCapacityWeightValue"/> — an Asset Model may
    /// have either, both, or neither, depending on the kind of
    /// equipment).
    /// </summary>
    public decimal? WorkingCapacityVolumeValue { get; private set; }

    /// <summary>Gets the identifier of the Unit of Measurement for <see cref="WorkingCapacityVolumeValue"/>, or <see langword="null"/> if not specified.</summary>
    public Guid? WorkingCapacityVolumeUnitOfMeasurementId { get; private set; }

    /// <summary>
    /// Gets the weight-based working capacity value (e.g. crane lifting
    /// capacity in kg/tons), or <see langword="null"/> if not specified.
    /// </summary>
    public decimal? WorkingCapacityWeightValue { get; private set; }

    /// <summary>Gets the identifier of the Unit of Measurement for <see cref="WorkingCapacityWeightValue"/>, or <see langword="null"/> if not specified.</summary>
    public Guid? WorkingCapacityWeightUnitOfMeasurementId { get; private set; }

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

    private AssetModel(
        AssetModelId id,
        Guid holdingId,
        string name,
        Guid companyId,
        decimal? lengthValue,
        Guid? lengthUnitOfMeasurementId,
        decimal? widthValue,
        Guid? widthUnitOfMeasurementId,
        decimal? heightValue,
        Guid? heightUnitOfMeasurementId,
        decimal? weightValue,
        Guid? weightUnitOfMeasurementId,
        decimal? workingCapacityVolumeValue,
        Guid? workingCapacityVolumeUnitOfMeasurementId,
        decimal? workingCapacityWeightValue,
        Guid? workingCapacityWeightUnitOfMeasurementId)
        : base(id)
    {
        HoldingId = holdingId;
        Name = name;
        CompanyId = companyId;
        LengthValue = lengthValue;
        LengthUnitOfMeasurementId = lengthUnitOfMeasurementId;
        WidthValue = widthValue;
        WidthUnitOfMeasurementId = widthUnitOfMeasurementId;
        HeightValue = heightValue;
        HeightUnitOfMeasurementId = heightUnitOfMeasurementId;
        WeightValue = weightValue;
        WeightUnitOfMeasurementId = weightUnitOfMeasurementId;
        WorkingCapacityVolumeValue = workingCapacityVolumeValue;
        WorkingCapacityVolumeUnitOfMeasurementId = workingCapacityVolumeUnitOfMeasurementId;
        WorkingCapacityWeightValue = workingCapacityWeightValue;
        WorkingCapacityWeightUnitOfMeasurementId = workingCapacityWeightUnitOfMeasurementId;
    }

    /// <summary>Registers a new Asset Model.</summary>
    /// <param name="holdingId">The owning Holding.</param>
    /// <param name="name">The display name.</param>
    /// <param name="companyId">The manufacturer.</param>
    /// <param name="dateTimeProvider">Provides the current UTC time for the raised domain event.</param>
    /// <param name="lengthValue">Optional length value.</param>
    /// <param name="lengthUnitOfMeasurementId">Optional unit of measurement for the length value.</param>
    /// <param name="widthValue">Optional width value.</param>
    /// <param name="widthUnitOfMeasurementId">Optional unit of measurement for the width value.</param>
    /// <param name="heightValue">Optional height value.</param>
    /// <param name="heightUnitOfMeasurementId">Optional unit of measurement for the height value.</param>
    /// <param name="weightValue">Optional weight value.</param>
    /// <param name="weightUnitOfMeasurementId">Optional unit of measurement for the weight value.</param>
    /// <param name="workingCapacityVolumeValue">Optional volume-based working capacity value.</param>
    /// <param name="workingCapacityVolumeUnitOfMeasurementId">Optional unit of measurement for the volume-based working capacity value.</param>
    /// <param name="workingCapacityWeightValue">Optional weight-based working capacity value.</param>
    /// <param name="workingCapacityWeightUnitOfMeasurementId">Optional unit of measurement for the weight-based working capacity value.</param>
    /// <returns>A <see cref="Result{AssetModel}"/> containing the new aggregate, or a validation error.</returns>
    public static Result<AssetModel> Register(
        Guid holdingId,
        string name,
        Guid companyId,
        IDateTimeProvider dateTimeProvider,
        decimal? lengthValue = null,
        Guid? lengthUnitOfMeasurementId = null,
        decimal? widthValue = null,
        Guid? widthUnitOfMeasurementId = null,
        decimal? heightValue = null,
        Guid? heightUnitOfMeasurementId = null,
        decimal? weightValue = null,
        Guid? weightUnitOfMeasurementId = null,
        decimal? workingCapacityVolumeValue = null,
        Guid? workingCapacityVolumeUnitOfMeasurementId = null,
        decimal? workingCapacityWeightValue = null,
        Guid? workingCapacityWeightUnitOfMeasurementId = null)
    {
        if (string.IsNullOrWhiteSpace(name))
        {
            return Result.Failure<AssetModel>(AssetModelErrors.NameRequired());
        }

        if (name.Length > MaxNameLength)
        {
            return Result.Failure<AssetModel>(AssetModelErrors.NameTooLong(MaxNameLength));
        }

        var specificationsCheck = ValidateSpecifications(
            lengthValue, lengthUnitOfMeasurementId,
            widthValue, widthUnitOfMeasurementId,
            heightValue, heightUnitOfMeasurementId,
            weightValue, weightUnitOfMeasurementId,
            workingCapacityVolumeValue, workingCapacityVolumeUnitOfMeasurementId,
            workingCapacityWeightValue, workingCapacityWeightUnitOfMeasurementId);

        if (specificationsCheck.IsFailure)
        {
            return Result.Failure<AssetModel>(specificationsCheck.Error);
        }

        var assetModel = new AssetModel(
            AssetModelId.New(),
            holdingId,
            name.Trim(),
            companyId,
            lengthValue,
            lengthUnitOfMeasurementId,
            widthValue,
            widthUnitOfMeasurementId,
            heightValue,
            heightUnitOfMeasurementId,
            weightValue,
            weightUnitOfMeasurementId,
            workingCapacityVolumeValue,
            workingCapacityVolumeUnitOfMeasurementId,
            workingCapacityWeightValue,
            workingCapacityWeightUnitOfMeasurementId);

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

    /// <summary>
    /// Updates this Asset Model's technical specifications — the same
    /// fields accepted by <see cref="Register"/> apart from Holding and
    /// Name (chat, 2026-09-04, mirroring the equivalent
    /// EngineModel.UpdateSpecifications built 2026-09-01).
    /// </summary>
    /// <param name="companyId">The manufacturer company.</param>
    /// <param name="lengthValue">Optional length value.</param>
    /// <param name="lengthUnitOfMeasurementId">Optional unit of measurement for the length value.</param>
    /// <param name="widthValue">Optional width value.</param>
    /// <param name="widthUnitOfMeasurementId">Optional unit of measurement for the width value.</param>
    /// <param name="heightValue">Optional height value.</param>
    /// <param name="heightUnitOfMeasurementId">Optional unit of measurement for the height value.</param>
    /// <param name="weightValue">Optional weight value.</param>
    /// <param name="weightUnitOfMeasurementId">Optional unit of measurement for the weight value.</param>
    /// <param name="workingCapacityVolumeValue">Optional volume-based working capacity value.</param>
    /// <param name="workingCapacityVolumeUnitOfMeasurementId">Optional unit of measurement for the volume-based working capacity value.</param>
    /// <param name="workingCapacityWeightValue">Optional weight-based working capacity value.</param>
    /// <param name="workingCapacityWeightUnitOfMeasurementId">Optional unit of measurement for the weight-based working capacity value.</param>
    /// <returns>A <see cref="Result"/> indicating success or a validation error.</returns>
    public Result UpdateSpecifications(
        Guid companyId,
        decimal? lengthValue,
        Guid? lengthUnitOfMeasurementId,
        decimal? widthValue,
        Guid? widthUnitOfMeasurementId,
        decimal? heightValue,
        Guid? heightUnitOfMeasurementId,
        decimal? weightValue,
        Guid? weightUnitOfMeasurementId,
        decimal? workingCapacityVolumeValue,
        Guid? workingCapacityVolumeUnitOfMeasurementId,
        decimal? workingCapacityWeightValue,
        Guid? workingCapacityWeightUnitOfMeasurementId)
    {
        var specificationsCheck = ValidateSpecifications(
            lengthValue, lengthUnitOfMeasurementId,
            widthValue, widthUnitOfMeasurementId,
            heightValue, heightUnitOfMeasurementId,
            weightValue, weightUnitOfMeasurementId,
            workingCapacityVolumeValue, workingCapacityVolumeUnitOfMeasurementId,
            workingCapacityWeightValue, workingCapacityWeightUnitOfMeasurementId);

        if (specificationsCheck.IsFailure)
        {
            return specificationsCheck;
        }

        CompanyId = companyId;
        LengthValue = lengthValue;
        LengthUnitOfMeasurementId = lengthUnitOfMeasurementId;
        WidthValue = widthValue;
        WidthUnitOfMeasurementId = widthUnitOfMeasurementId;
        HeightValue = heightValue;
        HeightUnitOfMeasurementId = heightUnitOfMeasurementId;
        WeightValue = weightValue;
        WeightUnitOfMeasurementId = weightUnitOfMeasurementId;
        WorkingCapacityVolumeValue = workingCapacityVolumeValue;
        WorkingCapacityVolumeUnitOfMeasurementId = workingCapacityVolumeUnitOfMeasurementId;
        WorkingCapacityWeightValue = workingCapacityWeightValue;
        WorkingCapacityWeightUnitOfMeasurementId = workingCapacityWeightUnitOfMeasurementId;

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

    /// <summary>
    /// Validates every value/unit pair together (chat, 2026-09-04): each
    /// of Length, Width, Height, Weight, WorkingCapacityVolume, and
    /// WorkingCapacityWeight is independently nullable, but a present
    /// value always requires its unit and vice versa.
    /// </summary>
    private static Result ValidateSpecifications(
        decimal? lengthValue, Guid? lengthUnitOfMeasurementId,
        decimal? widthValue, Guid? widthUnitOfMeasurementId,
        decimal? heightValue, Guid? heightUnitOfMeasurementId,
        decimal? weightValue, Guid? weightUnitOfMeasurementId,
        decimal? workingCapacityVolumeValue, Guid? workingCapacityVolumeUnitOfMeasurementId,
        decimal? workingCapacityWeightValue, Guid? workingCapacityWeightUnitOfMeasurementId)
    {
        var lengthCheck = ValidateValueUnitPair("Length", lengthValue, lengthUnitOfMeasurementId);

        if (lengthCheck.IsFailure)
        {
            return lengthCheck;
        }

        var widthCheck = ValidateValueUnitPair("Width", widthValue, widthUnitOfMeasurementId);

        if (widthCheck.IsFailure)
        {
            return widthCheck;
        }

        var heightCheck = ValidateValueUnitPair("Height", heightValue, heightUnitOfMeasurementId);

        if (heightCheck.IsFailure)
        {
            return heightCheck;
        }

        var weightCheck = ValidateValueUnitPair("Weight", weightValue, weightUnitOfMeasurementId);

        if (weightCheck.IsFailure)
        {
            return weightCheck;
        }

        var capacityVolumeCheck = ValidateValueUnitPair(
            "Working capacity (volume)", workingCapacityVolumeValue, workingCapacityVolumeUnitOfMeasurementId);

        if (capacityVolumeCheck.IsFailure)
        {
            return capacityVolumeCheck;
        }

        var capacityWeightCheck = ValidateValueUnitPair(
            "Working capacity (weight)", workingCapacityWeightValue, workingCapacityWeightUnitOfMeasurementId);

        if (capacityWeightCheck.IsFailure)
        {
            return capacityWeightCheck;
        }

        return Result.Success();
    }

    /// <summary>
    /// Validates that a technical specification's value and its unit of
    /// measurement are either both present or both absent, and that a
    /// present value is positive.
    /// </summary>
    private static Result ValidateValueUnitPair(string fieldName, decimal? value, Guid? unitOfMeasurementId)
    {
        if (value.HasValue != unitOfMeasurementId.HasValue)
        {
            return Result.Failure(AssetModelErrors.SpecificationValueUnitMismatch(fieldName));
        }

        if (value is <= 0)
        {
            return Result.Failure(AssetModelErrors.InvalidSpecificationValue(fieldName));
        }

        return Result.Success();
    }
}