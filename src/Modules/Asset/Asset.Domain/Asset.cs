using Asset.Domain.Events;
using MachineryManagerEnterprise.SharedKernel;
using MachineryManagerEnterprise.SharedKernel.Abstractions;

namespace Asset.Domain;

/// <summary>
/// Aggregate Root representing a single physical Asset (BR-001–BR-004) —
/// e.g. one specific truck or crusher. Shared specifications come from
/// its <see cref="AssetModelId"/>; this aggregate holds only the
/// instance-specific identity (identification code, name, serial
/// number, chassis/body number, VIN, license plate, manufacture year,
/// color) and current lifecycle state.
/// Owned by exactly one Organization (BR-003) — note this differs from
/// AssetModel/EngineModel, which are Holding-scoped (chat, 2026-08-27).
/// </summary>
public sealed class Asset : AggregateRoot<AssetId>
{
    /// <summary>The maximum allowed length for the identification code field.</summary>
    public const int MaxCodeLength = 20;

    /// <summary>The maximum allowed length for the name field.</summary>
    public const int MaxNameLength = 60;

    /// <summary>The maximum allowed length for the serial number field.</summary>
    public const int MaxSerialNumberLength = 100;

    /// <summary>The maximum allowed length for the license plate field.</summary>
    public const int MaxLicensePlateLength = 50;

    /// <summary>The maximum allowed length for the chassis number, body number, and VIN fields.</summary>
    public const int MaxChassisBodyVinLength = 30;

    /// <summary>Gets the identifier of the owning Organization (BR-003).</summary>
    public Guid OrganizationId { get; private set; }

    /// <summary>
    /// Gets the Asset's identification code — required, and unique
    /// within its owning Organization (chat, 2026-08-28).
    /// </summary>
    public string Code { get; private set; } = string.Empty;

    /// <summary>Gets the display name of the Asset (required, chat, 2026-08-28).</summary>
    public string Name { get; private set; } = string.Empty;

    /// <summary>Gets the identifier of this Asset's shared specification catalog entry.</summary>
    public AssetModelId AssetModelId { get; private set; } = null!;

    /// <summary>
    /// Gets the identifier of this Asset's Color, a reference into the
    /// Configuration module (chat, 2026-08-30). Stored as a plain Guid,
    /// not a typed value object, because Asset.Domain must not depend
    /// on Configuration.Domain directly (Modular Monolith boundary).
    /// </summary>
    public Guid ColorId { get; private set; }

    /// <summary>
    /// Gets the identifier of the Project this Asset is currently
    /// assigned to, if any — a reference into the Organization module
    /// (chat, 2026-09-14). Stored as a plain Guid, not a typed value
    /// object, mirroring <see cref="ColorId"/>: Asset.Domain must not
    /// depend on Organization.Domain directly. Per Project's own design
    /// note, a Project does not own the Asset — this only records the
    /// Asset's current operational assignment, which the caller may
    /// change over time (not yet supported in this increment: Asset has
    /// no "Reassign" mutator, matching its current no-edit-after-create
    /// design for every other identity field).
    /// </summary>
    public Guid ProjectId { get; private set; }

    /// <summary>
    /// Gets the identifier of this Asset's current operational status
    /// (e.g. "در حال کار", "آماده به کار" — chat, 2026-09-18). Required.
    /// Reference into the Configuration module's user-managed
    /// AssetOperationalStatus catalog; plain Guid, mirroring
    /// <see cref="ColorId"/>.
    /// </summary>
    public Guid OperationalStatusId { get; private set; }

    /// <summary>Gets the serial number, if recorded.</summary>
    public string? SerialNumber { get; private set; }

    /// <summary>Gets the chassis number, if recorded (chat, 2026-08-28).</summary>
    public string? ChassisNumber { get; private set; }

    /// <summary>Gets the body number, if recorded (chat, 2026-08-28).</summary>
    public string? BodyNumber { get; private set; }

    /// <summary>Gets the Vehicle Identification Number, if recorded (chat, 2026-08-28).</summary>
    public string? Vin { get; private set; }

    /// <summary>Gets the license plate, if recorded.</summary>
    public string? LicensePlate { get; private set; }

    /// <summary>Gets the manufacture year, if recorded.</summary>
    public int? ManufactureYear { get; private set; }

    /// <summary>
    /// Gets the unit in which this Asset's counter/odometer is read
    /// (hours, kilometers, or miles), if recorded (chat, 2026-09-19).
    /// A fixed <see cref="global::MachineryManagerEnterprise.SharedKernel.MeterReadingUnit"/>
    /// enum — no longer a reference into the Configuration module's
    /// UnitOfMeasurement table.
    /// </summary>
    public MeterReadingUnit? MeterReadingUnit { get; private set; }

    /// <summary>
    /// Gets the primary fuel this Asset consumes, if recorded (chat,
    /// 2026-09-14). Distinct from <see cref="EngineModel.FuelKind"/>:
    /// that is the catalog default for the engine class, while this is
    /// the physical Asset's actual real-world configuration (e.g. after
    /// a bi-fuel conversion). Optional because not every Asset has an
    /// engine (e.g. a crusher).
    /// </summary>
    public FuelKind? PrimaryFuelKind { get; private set; }

    /// <summary>
    /// Gets the unit in which <see cref="PrimaryFuelKind"/>'s
    /// consumption is counted (e.g. liters, kilograms), or
    /// <see langword="null"/> if not specified (chat, 2026-09-19).
    /// Set together with <see cref="PrimaryFuelKind"/> — one implies
    /// the other.
    /// </summary>
    public FuelUnit? PrimaryFuelUnit { get; private set; }

    /// <summary>
    /// Gets the secondary fuel this Asset consumes, if it is a bi-fuel
    /// Asset (e.g. gasoline + LPG — chat, 2026-09-14). Only meaningful
    /// alongside a <see cref="PrimaryFuelKind"/>, and must differ from
    /// it.
    /// </summary>
    public FuelKind? SecondaryFuelKind { get; private set; }

    /// <summary>
    /// Gets the unit in which <see cref="SecondaryFuelKind"/>'s
    /// consumption is counted, or <see langword="null"/> if not
    /// specified (chat, 2026-09-19). Set together with
    /// <see cref="SecondaryFuelKind"/> — one implies the other.
    /// </summary>
    public FuelUnit? SecondaryFuelUnit { get; private set; }

    /// <summary>Gets the current lifecycle status.</summary>
    public AssetStatus Status { get; private set; }

    // Reserved for ORM materialization only. Never used by application code.
    private Asset()
    {
    }

    private Asset(
    AssetId id,
    Guid organizationId,
    string code,
    string name,
    AssetModelId assetModelId,
    Guid colorId,
    Guid projectId,
    string? serialNumber,
    string? chassisNumber,
    string? bodyNumber,
    string? vin,
    string? licensePlate,
    int? manufactureYear,
    MeterReadingUnit? meterReadingUnit,
    FuelKind? primaryFuelKind,
    FuelUnit? primaryFuelUnit,
    FuelKind? secondaryFuelKind,
    FuelUnit? secondaryFuelUnit)
        : base(id)
    {
        OrganizationId = organizationId;
        Code = code;
        Name = name;
        AssetModelId = assetModelId;
        ColorId = colorId;
        SerialNumber = serialNumber;
        ChassisNumber = chassisNumber;
        BodyNumber = bodyNumber;
        Vin = vin;
        LicensePlate = licensePlate;
        ManufactureYear = manufactureYear;
        ProjectId = projectId;
        MeterReadingUnit = meterReadingUnit;
        PrimaryFuelKind = primaryFuelKind;
        PrimaryFuelUnit = primaryFuelUnit;
        SecondaryFuelKind = secondaryFuelKind;
        SecondaryFuelUnit = secondaryFuelUnit;
        Status = AssetStatus.Registered;
    }

    /// <summary>
    /// Registers a new Asset. Single-step registration (chat, 2026-08-27):
    /// identity and model are captured together and the Asset starts
    /// directly at <see cref="AssetStatus.Registered"/> (the Draft state
    /// is reserved for a future two-step flow). Uniqueness of
    /// <paramref name="code"/> within the Organization, existence of the
    /// referenced AssetModel/Color/Project and Organization/Holding
    /// consistency are all enforced by the caller (Application layer)
    /// before this method is invoked —
    /// the aggregate itself cannot check other aggregates.
    /// </summary>
    public static Result<global::Asset.Domain.Asset> Register(
    Guid organizationId,
    string code,
    string name,
    AssetModelId assetModelId,
    Guid colorId,
    Guid projectId,
    string? serialNumber,
    string? chassisNumber,
    string? bodyNumber,
    string? vin,
    string? licensePlate,
    int? manufactureYear,
    IDateTimeProvider dateTimeProvider,
    MeterReadingUnit? meterReadingUnit = null,
    FuelKind? primaryFuelKind = null,
    FuelUnit? primaryFuelUnit = null,
    FuelKind? secondaryFuelKind = null,
    FuelUnit? secondaryFuelUnit = null)
    {
        if (string.IsNullOrWhiteSpace(code))
        {
            return Result.Failure<global::Asset.Domain.Asset>(AssetErrors.CodeRequired());
        }

        if (code.Length > MaxCodeLength)
        {
            return Result.Failure<global::Asset.Domain.Asset>(AssetErrors.CodeTooLong(MaxCodeLength));
        }

        if (string.IsNullOrWhiteSpace(name))
        {
            return Result.Failure<global::Asset.Domain.Asset>(AssetErrors.NameRequired());
        }

        if (name.Length > MaxNameLength)
        {
            return Result.Failure<global::Asset.Domain.Asset>(AssetErrors.NameTooLong(MaxNameLength));
        }

        if (chassisNumber is { Length: > 0 } && chassisNumber.Length > MaxChassisBodyVinLength)
        {
            return Result.Failure<global::Asset.Domain.Asset>(
                AssetErrors.FieldTooLong(nameof(ChassisNumber), MaxChassisBodyVinLength));
        }

        if (bodyNumber is { Length: > 0 } && bodyNumber.Length > MaxChassisBodyVinLength)
        {
            return Result.Failure<global::Asset.Domain.Asset>(
                AssetErrors.FieldTooLong(nameof(BodyNumber), MaxChassisBodyVinLength));
        }

        if (vin is { Length: > 0 } && vin.Length > MaxChassisBodyVinLength)
        {
            return Result.Failure<global::Asset.Domain.Asset>(
                AssetErrors.FieldTooLong(nameof(Vin), MaxChassisBodyVinLength));
        }

        if (projectId == Guid.Empty)
        {
            return Result.Failure<global::Asset.Domain.Asset>(AssetErrors.ProjectRequired());
        }

        var fuelConfiguration = ValidateFuelConfiguration(
            primaryFuelKind, primaryFuelUnit, secondaryFuelKind, secondaryFuelUnit);

        if (fuelConfiguration.IsFailure)
        {
            return Result.Failure<global::Asset.Domain.Asset>(fuelConfiguration.Error);
        }

        var asset = new global::Asset.Domain.Asset(
            AssetId.New(), organizationId, code.Trim(), name.Trim(), assetModelId, colorId, projectId,
            string.IsNullOrWhiteSpace(serialNumber) ? null : serialNumber.Trim(),
            string.IsNullOrWhiteSpace(chassisNumber) ? null : chassisNumber.Trim(),
            string.IsNullOrWhiteSpace(bodyNumber) ? null : bodyNumber.Trim(),
            string.IsNullOrWhiteSpace(vin) ? null : vin.Trim(),
            string.IsNullOrWhiteSpace(licensePlate) ? null : licensePlate.Trim(),
            manufactureYear, meterReadingUnit, primaryFuelKind, primaryFuelUnit,
            secondaryFuelKind, secondaryFuelUnit);

        asset.RaiseDomainEvent(new AssetRegistered(asset.Id, organizationId, assetModelId, dateTimeProvider.UtcNow));
        return asset;
    }

    /// <summary>
    /// Validates the Primary/Secondary fuel kind-and-unit pairing
    /// (chat, 2026-09-14): each Kind must be accompanied by its own
    /// <see cref="FuelUnit"/> (both or neither), a Secondary fuel cannot
    /// be set without a Primary one, and the two Kinds — when both
    /// present — must differ (a bi-fuel Asset, e.g. gasoline + LPG,
    /// never declares the same fuel twice).
    /// </summary>
    private static Result ValidateFuelConfiguration(
        FuelKind? primaryFuelKind,
        FuelUnit? primaryFuelUnit,
        FuelKind? secondaryFuelKind,
        FuelUnit? secondaryFuelUnit)
    {
        if (primaryFuelKind.HasValue != primaryFuelUnit.HasValue)
        {
            return Result.Failure(AssetErrors.FuelSpecificationMismatch(nameof(PrimaryFuelKind)));
        }

        if (secondaryFuelKind.HasValue != secondaryFuelUnit.HasValue)
        {
            return Result.Failure(AssetErrors.FuelSpecificationMismatch(nameof(SecondaryFuelKind)));
        }

        if (secondaryFuelKind.HasValue && !primaryFuelKind.HasValue)
        {
            return Result.Failure(AssetErrors.SecondaryFuelRequiresPrimary());
        }

        if (primaryFuelKind.HasValue && secondaryFuelKind.HasValue &&
            primaryFuelKind.Value == secondaryFuelKind.Value)
        {
            return Result.Failure(AssetErrors.DuplicateFuelKind());
        }

        return Result.Success();
    }

    /// <summary>Completes commissioning (Registered → Commissioned).</summary>
    public Result Commission(IDateTimeProvider dateTimeProvider)
    {
        if (Status != AssetStatus.Registered)
        {
            return Result.Failure(AssetErrors.InvalidTransition(Status, AssetStatus.Commissioned));
        }

        Status = AssetStatus.Commissioned;
        RaiseDomainEvent(new AssetCommissioned(Id, dateTimeProvider.UtcNow));

        return Result.Success();
    }

    /// <summary>
    /// Places the Asset into operation (Commissioned → Operational, or
    /// Inactive → Operational). Raises <see cref="AssetActivated"/> for
    /// the first case and <see cref="AssetReactivated"/> for the second.
    /// </summary>
    public Result Activate(IDateTimeProvider dateTimeProvider)
    {
        if (Status is not (AssetStatus.Commissioned or AssetStatus.Inactive))
        {
            return Result.Failure(AssetErrors.InvalidTransition(Status, AssetStatus.Operational));
        }

        var wasInactive = Status == AssetStatus.Inactive;

        Status = AssetStatus.Operational;

        if (wasInactive)
        {
            RaiseDomainEvent(new AssetReactivated(Id, dateTimeProvider.UtcNow));
        }
        else
        {
            RaiseDomainEvent(new AssetActivated(Id, dateTimeProvider.UtcNow));
        }

        return Result.Success();
    }

    /// <summary>Temporarily takes the Asset out of use (Operational → Inactive).</summary>
    public Result Deactivate(IDateTimeProvider dateTimeProvider)
    {
        if (Status != AssetStatus.Operational)
        {
            return Result.Failure(AssetErrors.InvalidTransition(Status, AssetStatus.Inactive));
        }

        Status = AssetStatus.Inactive;
        RaiseDomainEvent(new AssetDeactivated(Id, dateTimeProvider.UtcNow));

        return Result.Success();
    }

    /// <summary>Permanently withdraws the Asset from use (Operational or Inactive → Retired).</summary>
    public Result Retire(IDateTimeProvider dateTimeProvider)
    {
        if (Status is not (AssetStatus.Operational or AssetStatus.Inactive))
        {
            return Result.Failure(AssetErrors.InvalidTransition(Status, AssetStatus.Retired));
        }

        Status = AssetStatus.Retired;
        RaiseDomainEvent(new AssetRetired(Id, dateTimeProvider.UtcNow));

        return Result.Success();
    }

    /// <summary>
    /// Marks the Asset as physically disposed of (final state; BR-004 —
    /// history is preserved, never overwritten).
    /// </summary>
    public Result Dispose(IDateTimeProvider dateTimeProvider)
    {
        if (Status != AssetStatus.Retired)
        {
            return Result.Failure(AssetErrors.InvalidTransition(Status, AssetStatus.Disposed));
        }

        Status = AssetStatus.Disposed;
        RaiseDomainEvent(new AssetDisposed(Id, dateTimeProvider.UtcNow));

        return Result.Success();
    }

    /// <summary>
    /// Updates this Asset's mutable details (chat, 2026-09-16). The
    /// owning Organization remains permanent (BR-003) and is not
    /// editable here. The identification Code became editable (chat,
    /// 2026-09-19 — this relaxes the earlier BR-001 "permanent code"
    /// rule; the Asset's <see cref="AssetId"/> stays its permanent
    /// identity), as is every other identity/specification field,
    /// including the AssetModel, Color, and Project assignment.
    /// Uniqueness of the new code within the Organization, and
    /// existence and cross-module consistency of
    /// AssetModelId/ColorId/ProjectId, are enforced by the caller
    /// (Application layer), exactly as for <see cref="Register"/>.
    /// </summary>
    public Result UpdateDetails(
        string code,
        string name,
        AssetModelId assetModelId,
        Guid colorId,
        Guid projectId,
        string? serialNumber,
        string? chassisNumber,
        string? bodyNumber,
        string? vin,
        string? licensePlate,
        int? manufactureYear,
        MeterReadingUnit? meterReadingUnit,
        FuelKind? primaryFuelKind,
        FuelUnit? primaryFuelUnit,
        FuelKind? secondaryFuelKind,
        FuelUnit? secondaryFuelUnit)
    {
        if (string.IsNullOrWhiteSpace(code))
        {
            return Result.Failure(AssetErrors.CodeRequired());
        }

        var trimmedCode = code.Trim();

        if (trimmedCode.Length > MaxCodeLength)
        {
            return Result.Failure(AssetErrors.CodeTooLong(MaxCodeLength));
        }

        if (string.IsNullOrWhiteSpace(name))
        {
            return Result.Failure(AssetErrors.NameRequired());
        }

        if (name.Length > MaxNameLength)
        {
            return Result.Failure(AssetErrors.NameTooLong(MaxNameLength));
        }

        if (chassisNumber is { Length: > 0 } && chassisNumber.Length > MaxChassisBodyVinLength)
        {
            return Result.Failure(AssetErrors.FieldTooLong(nameof(ChassisNumber), MaxChassisBodyVinLength));
        }

        if (bodyNumber is { Length: > 0 } && bodyNumber.Length > MaxChassisBodyVinLength)
        {
            return Result.Failure(AssetErrors.FieldTooLong(nameof(BodyNumber), MaxChassisBodyVinLength));
        }

        if (vin is { Length: > 0 } && vin.Length > MaxChassisBodyVinLength)
        {
            return Result.Failure(AssetErrors.FieldTooLong(nameof(Vin), MaxChassisBodyVinLength));
        }

        if (projectId == Guid.Empty)
        {
            return Result.Failure(AssetErrors.ProjectRequired());
        }

        var fuelConfiguration = ValidateFuelConfiguration(
            primaryFuelKind, primaryFuelUnit, secondaryFuelKind, secondaryFuelUnit);

        if (fuelConfiguration.IsFailure)
        {
            return fuelConfiguration;
        }

        Code = trimmedCode;
        Name = name.Trim();
        AssetModelId = assetModelId;
        ColorId = colorId;
        ProjectId = projectId;
        SerialNumber = string.IsNullOrWhiteSpace(serialNumber) ? null : serialNumber.Trim();
        ChassisNumber = string.IsNullOrWhiteSpace(chassisNumber) ? null : chassisNumber.Trim();
        BodyNumber = string.IsNullOrWhiteSpace(bodyNumber) ? null : bodyNumber.Trim();
        Vin = string.IsNullOrWhiteSpace(vin) ? null : vin.Trim();
        LicensePlate = string.IsNullOrWhiteSpace(licensePlate) ? null : licensePlate.Trim();
        ManufactureYear = manufactureYear;
        MeterReadingUnit = meterReadingUnit;
        PrimaryFuelKind = primaryFuelKind;
        PrimaryFuelUnit = primaryFuelUnit;
        SecondaryFuelKind = secondaryFuelKind;
        SecondaryFuelUnit = secondaryFuelUnit;

        return Result.Success();
    }
}
