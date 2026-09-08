using Asset.Domain.Events;
using MachineryManager.SharedKernel;
using MachineryManager.SharedKernel.Abstractions;

namespace Asset.Domain;

/// <summary>
/// Aggregate Root representing a catalog of shared specifications for a
/// class of Engines (e.g. "Volvo D13"). Individual Engine instances
/// (own aggregate, not modeled in this increment) reference exactly one
/// EngineModel and inherit its shared specifications; identity-specific
/// data (serial number, install history) lives on the Engine instance
/// itself, not here (chat, 2026-08-25).
/// Scoped per Holding — shared across every Organization under that
/// Holding; only Engine identity records themselves are per-Organization
/// (correction, chat, 2026-08-26 — supersedes the earlier per-Organization
/// scope note).
/// </summary>
public sealed class EngineModel : AggregateRoot<EngineModelId>
{
    /// <summary>The maximum allowed length for the engine model's name.</summary>
    public const int MaxNameLength = 200;

    /// <summary>The maximum allowed length for the manufacturer's name.</summary>
    public const int MaxManufacturerLength = 200;

    /// <summary>Gets the identifier of the Holding that owns this catalog entry.</summary>
    public Guid HoldingId { get; private set; }

    /// <summary>Gets the display name of the engine model (e.g. "D13").</summary>
    public string Name { get; private set; } = string.Empty;

    /// <summary>
    /// Gets the identifier of the manufacturer Company. The Company belongs to the same Holding as this Engine Model.
    /// </summary>
    public Guid CompanyId { get; private set; }

    /// <summary>
    /// Gets the fuel kind this engine consumes. Required (chat,
    /// 2026-09-02) — unlike the other technical specification fields,
    /// FuelKind has no "not specified" state; every Engine Model must
    /// declare one.
    /// </summary>
    public FuelKind FuelKind { get; private set; }

    /// <summary>Gets the number of cylinders, or <see langword="null"/> if not specified (chat, 2026-08-30).</summary>
    public int? CylinderCount { get; private set; }

    /// <summary>Gets the engine displacement (volume) value, or <see langword="null"/> if not specified.</summary>
    public decimal? EngineDisplacementValue { get; private set; }

    /// <summary>
    /// Gets the identifier of the Unit of Measurement for
    /// <see cref="EngineDisplacementValue"/>, or <see langword="null"/>
    /// if not specified. References the Configuration module's
    /// UnitOfMeasurement aggregate by plain Guid — cross-module, no
    /// strongly-typed reference or database-level FK (chat, 2026-08-30),
    /// mirroring how <see cref="HoldingId"/> references the Organization
    /// module.
    /// </summary>
    public Guid? EngineDisplacementUnitOfMeasurementId { get; private set; }

    /// <summary>Gets the engine power value, or <see langword="null"/> if not specified.</summary>
    public decimal? EnginePowerValue { get; private set; }

    /// <summary>
    /// Gets the identifier of the Unit of Measurement for
    /// <see cref="EnginePowerValue"/>, or <see langword="null"/> if not specified.
    /// </summary>
    public Guid? EnginePowerUnitOfMeasurementId { get; private set; }

    /// <summary>Gets the weight value, or <see langword="null"/> if not specified.</summary>
    public decimal? WeightValue { get; private set; }

    /// <summary>
    /// Gets the identifier of the Unit of Measurement for
    /// <see cref="WeightValue"/>, or <see langword="null"/> if not specified.
    /// </summary>
    public Guid? WeightUnitOfMeasurementId { get; private set; }

    // Reserved for ORM materialization only. Never used by application code.
    private EngineModel()
    {
    }

    private EngineModel(
        EngineModelId id,
        Guid holdingId,
        string name,
        Guid companyId,
        FuelKind fuelKind,
        int? cylinderCount,
        decimal? engineDisplacementValue,
        Guid? engineDisplacementUnitOfMeasurementId,
        decimal? enginePowerValue,
        Guid? enginePowerUnitOfMeasurementId,
        decimal? weightValue,
        Guid? weightUnitOfMeasurementId)
        : base(id)
    {
        HoldingId = holdingId;
        Name = name;
        CompanyId = companyId;
        FuelKind = fuelKind;
        CylinderCount = cylinderCount;
        EngineDisplacementValue = engineDisplacementValue;
        EngineDisplacementUnitOfMeasurementId = engineDisplacementUnitOfMeasurementId;
        EnginePowerValue = enginePowerValue;
        EnginePowerUnitOfMeasurementId = enginePowerUnitOfMeasurementId;
        WeightValue = weightValue;
        WeightUnitOfMeasurementId = weightUnitOfMeasurementId;
    }

    /// <summary>Registers a new Engine Model.</summary>
    /// <param name="holdingId">The owning Holding.</param>
    /// <param name="name">The display name.</param>
    /// <param name="companyId">The manufacturer company.</param>
    /// <param name="fuelKind">The fuel kind this engine consumes (chat, 2026-09-02 — required).</param>
    /// <param name="dateTimeProvider">Provides the current UTC time for the raised domain event.</param>
    /// <param name="cylinderCount">Optional number of cylinders (chat, 2026-08-30).</param>
    /// <param name="engineDisplacementValue">Optional engine displacement value.</param>
    /// <param name="engineDisplacementUnitOfMeasurementId">Optional unit of measurement for the displacement value.</param>
    /// <param name="enginePowerValue">Optional engine power value.</param>
    /// <param name="enginePowerUnitOfMeasurementId">Optional unit of measurement for the power value.</param>
    /// <param name="weightValue">Optional weight value.</param>
    /// <param name="weightUnitOfMeasurementId">Optional unit of measurement for the weight value.</param>
    /// <returns>A <see cref="Result{EngineModel}"/> containing the new aggregate, or a validation error.</returns>
    public static Result<EngineModel> Register(
        Guid holdingId,
        string name,
        Guid companyId,
        FuelKind fuelKind,
        IDateTimeProvider dateTimeProvider,
        int? cylinderCount = null,
        decimal? engineDisplacementValue = null,
        Guid? engineDisplacementUnitOfMeasurementId = null,
        decimal? enginePowerValue = null,
        Guid? enginePowerUnitOfMeasurementId = null,
        decimal? weightValue = null,
        Guid? weightUnitOfMeasurementId = null)
    {
        if (string.IsNullOrWhiteSpace(name))
        {
            return Result.Failure<EngineModel>(EngineModelErrors.NameRequired());
        }

        if (name.Length > MaxNameLength)
        {
            return Result.Failure<EngineModel>(EngineModelErrors.NameTooLong(MaxNameLength));
        }

        if (!Enum.IsDefined(fuelKind))
        {
            return Result.Failure<EngineModel>(EngineModelErrors.InvalidFuelKind());
        }

        if (cylinderCount is <= 0)
        {
            return Result.Failure<EngineModel>(EngineModelErrors.InvalidCylinderCount());
        }

        var displacementCheck = ValidateValueUnitPair(
            "Engine displacement", engineDisplacementValue, engineDisplacementUnitOfMeasurementId);

        if (displacementCheck.IsFailure)
        {
            return Result.Failure<EngineModel>(displacementCheck.Error);
        }

        var powerCheck = ValidateValueUnitPair("Engine power", enginePowerValue, enginePowerUnitOfMeasurementId);

        if (powerCheck.IsFailure)
        {
            return Result.Failure<EngineModel>(powerCheck.Error);
        }

        var weightCheck = ValidateValueUnitPair("Weight", weightValue, weightUnitOfMeasurementId);

        if (weightCheck.IsFailure)
        {
            return Result.Failure<EngineModel>(weightCheck.Error);
        }

        var engineModel = new EngineModel(
            EngineModelId.New(),
            holdingId,
            name.Trim(),
            companyId,
            fuelKind,
            cylinderCount,
            engineDisplacementValue,
            engineDisplacementUnitOfMeasurementId,
            enginePowerValue,
            enginePowerUnitOfMeasurementId,
            weightValue,
            weightUnitOfMeasurementId);

        engineModel.RaiseDomainEvent(new EngineModelRegistered(
            engineModel.Id,
            holdingId,
            engineModel.Name,
            dateTimeProvider.UtcNow));

        return engineModel;
    }

    /// <summary>Renames this Engine Model.</summary>
    /// <param name="name">The new name.</param>
    /// <returns>A <see cref="Result"/> indicating success or a validation error.</returns>
    public Result Rename(string name)
    {
        if (string.IsNullOrWhiteSpace(name))
        {
            return Result.Failure(EngineModelErrors.NameRequired());
        }

        if (name.Length > MaxNameLength)
        {
            return Result.Failure(EngineModelErrors.NameTooLong(MaxNameLength));
        }

        Name = name.Trim();

        return Result.Success();
    }

    /// <summary>
    /// Updates this Engine Model's technical specifications — the same
    /// fields accepted by <see cref="Register"/> apart from Holding and
    /// Name (chat, 2026-09-01: the Edit page previously exposed only
    /// Name; this closes that gap. Chat, 2026-09-02: FuelKind added,
    /// also required here).
    /// </summary>
    /// <param name="companyId">The manufacturer company.</param>
    /// <param name="fuelKind">The fuel kind this engine consumes.</param>
    /// <param name="cylinderCount">Optional number of cylinders.</param>
    /// <param name="engineDisplacementValue">Optional engine displacement value.</param>
    /// <param name="engineDisplacementUnitOfMeasurementId">Optional unit of measurement for the displacement value.</param>
    /// <param name="enginePowerValue">Optional engine power value.</param>
    /// <param name="enginePowerUnitOfMeasurementId">Optional unit of measurement for the power value.</param>
    /// <param name="weightValue">Optional weight value.</param>
    /// <param name="weightUnitOfMeasurementId">Optional unit of measurement for the weight value.</param>
    /// <returns>A <see cref="Result"/> indicating success or a validation error.</returns>
    public Result UpdateSpecifications(
        Guid companyId,
        FuelKind fuelKind,
        int? cylinderCount,
        decimal? engineDisplacementValue,
        Guid? engineDisplacementUnitOfMeasurementId,
        decimal? enginePowerValue,
        Guid? enginePowerUnitOfMeasurementId,
        decimal? weightValue,
        Guid? weightUnitOfMeasurementId)
    {
        if (!Enum.IsDefined(fuelKind))
        {
            return Result.Failure(EngineModelErrors.InvalidFuelKind());
        }

        if (cylinderCount is <= 0)
        {
            return Result.Failure(EngineModelErrors.InvalidCylinderCount());
        }

        var displacementCheck = ValidateValueUnitPair(
            "Engine displacement", engineDisplacementValue, engineDisplacementUnitOfMeasurementId);

        if (displacementCheck.IsFailure)
        {
            return displacementCheck;
        }

        var powerCheck = ValidateValueUnitPair("Engine power", enginePowerValue, enginePowerUnitOfMeasurementId);

        if (powerCheck.IsFailure)
        {
            return powerCheck;
        }

        var weightCheck = ValidateValueUnitPair("Weight", weightValue, weightUnitOfMeasurementId);

        if (weightCheck.IsFailure)
        {
            return weightCheck;
        }

        CompanyId = companyId;
        FuelKind = fuelKind;
        CylinderCount = cylinderCount;
        EngineDisplacementValue = engineDisplacementValue;
        EngineDisplacementUnitOfMeasurementId = engineDisplacementUnitOfMeasurementId;
        EnginePowerValue = enginePowerValue;
        EnginePowerUnitOfMeasurementId = enginePowerUnitOfMeasurementId;
        WeightValue = weightValue;
        WeightUnitOfMeasurementId = weightUnitOfMeasurementId;

        return Result.Success();
    }

    /// <summary>
    /// Validates that a technical specification's value and its unit of
    /// measurement are either both present or both absent, and that a
    /// present value is positive (chat, 2026-08-30).
    /// </summary>
    private static Result ValidateValueUnitPair(string fieldName, decimal? value, Guid? unitOfMeasurementId)
    {
        if (value.HasValue != unitOfMeasurementId.HasValue)
        {
            return Result.Failure(EngineModelErrors.SpecificationValueUnitMismatch(fieldName));
        }

        if (value is <= 0)
        {
            return Result.Failure(EngineModelErrors.InvalidSpecificationValue(fieldName));
        }

        return Result.Success();
    }
}