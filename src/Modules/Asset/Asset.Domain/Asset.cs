using Asset.Domain.Events;
using MachineryManager.SharedKernel;
using MachineryManager.SharedKernel.Abstractions;

namespace Asset.Domain;

/// <summary>
/// Aggregate Root representing a single physical Asset (BR-001–BR-004) —
/// e.g. one specific truck or crusher. Shared specifications come from
/// its <see cref="AssetModelId"/>; this aggregate holds only the
/// instance-specific identity (serial number, license plate,
/// manufacture year, color) and current lifecycle state.
/// Owned by exactly one Organization (BR-003) — note this differs from
/// AssetModel/EngineModel, which are Holding-scoped (chat, 2026-08-27).
/// </summary>
public sealed class Asset : AggregateRoot<AssetId>
{
    /// <summary>The maximum allowed length for the color field.</summary>
    public const int MaxColorLength = 50;

    /// <summary>The maximum allowed length for the serial number field.</summary>
    public const int MaxSerialNumberLength = 100;

    /// <summary>The maximum allowed length for the license plate field.</summary>
    public const int MaxLicensePlateLength = 50;

    /// <summary>Gets the identifier of the owning Organization (BR-003).</summary>
    public Guid OrganizationId { get; private set; }

    /// <summary>Gets the identifier of this Asset's shared specification catalog entry.</summary>
    public AssetModelId AssetModelId { get; private set; } = null!;

    /// <summary>Gets the serial number, if recorded.</summary>
    public string? SerialNumber { get; private set; }

    /// <summary>Gets the license plate, if recorded.</summary>
    public string? LicensePlate { get; private set; }

    /// <summary>Gets the manufacture year, if recorded.</summary>
    public int? ManufactureYear { get; private set; }

    /// <summary>Gets the body color.</summary>
    public string Color { get; private set; } = string.Empty;

    /// <summary>Gets the current lifecycle status.</summary>
    public AssetStatus Status { get; private set; }

    // Reserved for ORM materialization only. Never used by application code.
    private Asset()
    {
    }

    private Asset(
        AssetId id,
        Guid organizationId,
        AssetModelId assetModelId,
        string color,
        string? serialNumber,
        string? licensePlate,
        int? manufactureYear)
        : base(id)
    {
        OrganizationId = organizationId;
        AssetModelId = assetModelId;
        Color = color;
        SerialNumber = serialNumber;
        LicensePlate = licensePlate;
        ManufactureYear = manufactureYear;
        Status = AssetStatus.Registered;
    }

    /// <summary>
    /// Registers a new Asset. Single-step registration (chat, 2026-08-27):
    /// identity and model are captured together and the Asset starts
    /// directly at <see cref="AssetStatus.Registered"/> (the Draft state
    /// is reserved for a future two-step flow).
    /// </summary>
    /// <param name="organizationId">The owning Organization.</param>
    /// <param name="assetModelId">The shared specification catalog entry.</param>
    /// <param name="color">The body color (required).</param>
    /// <param name="serialNumber">The serial number (optional).</param>
    /// <param name="licensePlate">The license plate (optional).</param>
    /// <param name="manufactureYear">The manufacture year (optional).</param>
    /// <param name="dateTimeProvider">Provides the current UTC time for the raised domain event.</param>
    /// <returns>A <see cref="Result{Asset}"/> containing the new aggregate, or a validation error.</returns>
    public static Result<global::Asset.Domain.Asset> Register(
        Guid organizationId,
        AssetModelId assetModelId,
        string color,
        string? serialNumber,
        string? licensePlate,
        int? manufactureYear,
        IDateTimeProvider dateTimeProvider)
    {
        if (string.IsNullOrWhiteSpace(color))
        {
            return Result.Failure<global::Asset.Domain.Asset>(AssetErrors.ColorRequired());
        }

        if (color.Length > MaxColorLength)
        {
            return Result.Failure<global::Asset.Domain.Asset>(
                Error.Validation("Asset.ColorTooLong", $"Asset color shall not exceed {MaxColorLength} characters."));
        }

        var asset = new global::Asset.Domain.Asset(
            AssetId.New(),
            organizationId,
            assetModelId,
            color.Trim(),
            string.IsNullOrWhiteSpace(serialNumber) ? null : serialNumber.Trim(),
            string.IsNullOrWhiteSpace(licensePlate) ? null : licensePlate.Trim(),
            manufactureYear);

        asset.RaiseDomainEvent(new AssetRegistered(asset.Id, organizationId, assetModelId, dateTimeProvider.UtcNow));

        return asset;
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
    /// the first case and <see cref="AssetReactivated"/> for the second,
    /// so the audit trail distinguishes initial activation from later
    /// reactivations (chat, 2026-08-27).
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
}