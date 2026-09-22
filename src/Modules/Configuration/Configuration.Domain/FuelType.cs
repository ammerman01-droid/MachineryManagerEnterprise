using Configuration.Domain.Events;
using MachineryManagerEnterprise.SharedKernel;
using MachineryManagerEnterprise.SharedKernel.Abstractions;

namespace Configuration.Domain;

/// <summary>
/// Aggregate Root representing a specific, priced fuel type (e.g.
/// "بنزین سوپر"), classified under a fixed <see cref="FuelKind"/>
/// (e.g. Gasoline). Scoped per-Holding (chat, 2026-09-02) — mirrors
/// UnitCategory's shape exactly (freely-named Name + fixed Kind), with
/// an added Price field.
/// </summary>
public sealed class FuelType : AggregateRoot<FuelTypeId>
{
    /// <summary>The maximum allowed length for the fuel type's name.</summary>
    public const int MaxNameLength = 50;

    /// <summary>Gets the identifier of the owning Holding.</summary>
    public Guid HoldingId { get; private set; }

    /// <summary>Gets the display name of the fuel type (e.g. "بنزین سوپر"), freely chosen by the Holding.</summary>
    public string Name { get; private set; } = string.Empty;

    /// <summary>Gets the current price, as a non-negative whole number (e.g. Toman/Rial).</summary>
    public long Price { get; private set; }

    /// <summary>Gets the fixed fuel-kind classification of this fuel type.</summary>
    public FuelKind Kind { get; private set; }

    /// <summary>
    /// Gets whether this Fuel Type is currently active. A deactivated
    /// (soft-deleted) Fuel Type is kept in the database for historical
    /// and audit purposes, but is excluded from selection lists by
    /// default (chat, 2026-09-19).
    /// </summary>
    public bool IsActive { get; private set; } = true;

    // Reserved for ORM materialization only. Never used by application code.
    private FuelType()
    {
    }

    private FuelType(FuelTypeId id, Guid holdingId, string name, long price, FuelKind kind)
        : base(id)
    {
        HoldingId = holdingId;
        Name = name;
        Price = price;
        Kind = kind;
    }

    /// <summary>Registers a new Fuel Type within a Holding.</summary>
    /// <param name="holdingId">The owning Holding.</param>
    /// <param name="name">The display name (required, max 50 characters).</param>
    /// <param name="price">The price, as a whole number greater than zero.</param>
    /// <param name="kind">The fixed fuel-kind classification for this fuel type.</param>
    /// <param name="dateTimeProvider">Provides the current UTC time for the raised domain event.</param>
    /// <returns>A <see cref="Result{FuelType}"/> containing the new aggregate, or a validation error.</returns>
    public static Result<FuelType> Register(
        Guid holdingId, string name, long price, FuelKind kind, IDateTimeProvider dateTimeProvider)
    {
        if (string.IsNullOrWhiteSpace(name))
        {
            return Result.Failure<FuelType>(FuelTypeErrors.NameRequired());
        }

        if (name.Length > MaxNameLength)
        {
            return Result.Failure<FuelType>(FuelTypeErrors.NameTooLong(MaxNameLength));
        }

        if (price <= 0)
        {
            return Result.Failure<FuelType>(FuelTypeErrors.PriceMustBePositive());
        }

        if (!Enum.IsDefined(kind))
        {
            return Result.Failure<FuelType>(FuelTypeErrors.InvalidKind());
        }

        var fuelType = new FuelType(FuelTypeId.New(), holdingId, name.Trim(), price, kind);

        fuelType.RaiseDomainEvent(
            new FuelTypeRegistered(fuelType.Id, holdingId, fuelType.Name, price, kind, dateTimeProvider.UtcNow));

        return fuelType;
    }

    /// <summary>
    /// Updates the Fuel Type's display name, price, and kind.
    /// </summary>
    /// <param name="name">The new display name (required, max 50 characters).</param>
    /// <param name="price">The new price, as a whole number greater than zero.</param>
    /// <param name="kind">The new fixed fuel-kind classification.</param>
    /// <param name="dateTimeProvider">Provides the current UTC time.</param>
    /// <returns>A successful <see cref="Result"/>, or a validation failure.</returns>
    public Result Update(string name, long price, FuelKind kind, IDateTimeProvider dateTimeProvider)
    {
        if (string.IsNullOrWhiteSpace(name))
        {
            return Result.Failure(FuelTypeErrors.NameRequired());
        }

        if (name.Length > MaxNameLength)
        {
            return Result.Failure(FuelTypeErrors.NameTooLong(MaxNameLength));
        }

        if (price <= 0)
        {
            return Result.Failure(FuelTypeErrors.PriceMustBePositive());
        }

        if (!Enum.IsDefined(kind))
        {
            return Result.Failure(FuelTypeErrors.InvalidKind());
        }

        var trimmed = name.Trim();

        if (trimmed == Name && price == Price && kind == Kind)
        {
            return Result.Success();
        }

        Name = trimmed;
        Price = price;
        Kind = kind;

        RaiseDomainEvent(new FuelTypeUpdated(Id, HoldingId, Name, Price, Kind, dateTimeProvider.UtcNow));

        return Result.Success();
    }

    /// <summary>
    /// Deactivates the Fuel Type (soft delete). A deactivated Fuel Type
    /// is excluded from selection lists but remains in the database.
    /// </summary>
    /// <param name="dateTimeProvider">Provides the current UTC time.</param>
    /// <returns>A successful <see cref="Result"/>, or a failure if the Fuel Type is already inactive.</returns>
    public Result Deactivate(IDateTimeProvider dateTimeProvider)
    {
        if (!IsActive)
        {
            return Result.Failure(FuelTypeErrors.AlreadyInactive());
        }

        IsActive = false;

        RaiseDomainEvent(new FuelTypeDeactivated(Id, HoldingId, dateTimeProvider.UtcNow));

        return Result.Success();
    }

    /// <summary>
    /// Reactivates a previously deactivated Fuel Type.
    /// </summary>
    /// <param name="dateTimeProvider">Provides the current UTC time.</param>
    /// <returns>A successful <see cref="Result"/>, or a failure if the Fuel Type is already active.</returns>
    public Result Activate(IDateTimeProvider dateTimeProvider)
    {
        if (IsActive)
        {
            return Result.Failure(FuelTypeErrors.AlreadyActive());
        }

        IsActive = true;

        RaiseDomainEvent(new FuelTypeActivated(Id, HoldingId, dateTimeProvider.UtcNow));

        return Result.Success();
    }
}
