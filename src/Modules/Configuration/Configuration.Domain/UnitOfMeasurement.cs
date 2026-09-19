using Configuration.Domain.Events;
using MachineryManagerEnterprise.SharedKernel;
using MachineryManagerEnterprise.SharedKernel.Abstractions;

namespace Configuration.Domain;

/// <summary>
/// Aggregate Root representing a selectable unit of measurement (e.g.
/// "اسب بخار" / "کیلووات"). Scope promoted from Organization to
/// Holding (chat, 2026-08-30) — same Holding as its CategoryId.
/// </summary>
public sealed class UnitOfMeasurement : AggregateRoot<UnitOfMeasurementId>
{
    /// <summary>Gets the MaxNameLength constant.</summary>
    public const int MaxNameLength = 50;
    /// <summary>Gets the HoldingId value.</summary>
    public Guid HoldingId { get; private set; }
    /// <summary>Gets the Name value.</summary>
    public string Name { get; private set; } = string.Empty;
    /// <summary>Gets the Kind value.</summary>
    public PhysicalQuantityKind Kind { get; private set; }

    private UnitOfMeasurement() { }

    private UnitOfMeasurement(UnitOfMeasurementId id, Guid holdingId, string name, PhysicalQuantityKind kind)
        : base(id)
    {
        HoldingId = holdingId;
        Name = name;
        Kind = kind;
    }

    /// <summary>Executes the Register operation.</summary>
    public static Result<UnitOfMeasurement> Register(
        Guid holdingId, string name, PhysicalQuantityKind kind, IDateTimeProvider dateTimeProvider)
    {
        if (string.IsNullOrWhiteSpace(name))
            return Result.Failure<UnitOfMeasurement>(UnitOfMeasurementErrors.NameRequired());

        if (name.Length > MaxNameLength)
            return Result.Failure<UnitOfMeasurement>(UnitOfMeasurementErrors.NameTooLong(MaxNameLength));

        if (!Enum.IsDefined(kind))
            return Result.Failure<UnitOfMeasurement>(UnitOfMeasurementErrors.InvalidKind());

        var unit = new UnitOfMeasurement(UnitOfMeasurementId.New(), holdingId, name.Trim(), kind);

        unit.RaiseDomainEvent(new UnitOfMeasurementRegistered(
            unit.Id, holdingId, unit.Name, kind, dateTimeProvider.UtcNow));

        return unit;
    }

    /// <summary>
    /// Updates this unit's name and physical quantity kind (chat,
    /// 2026-09-19). The owning Holding never changes. Changing the kind of a
    /// unit that other modules already reference can make those
    /// references fail their kind check the next time they are saved.
    /// </summary>
    public Result Update(string name, PhysicalQuantityKind kind)
    {
        if (string.IsNullOrWhiteSpace(name))
            return Result.Failure(UnitOfMeasurementErrors.NameRequired());

        if (name.Trim().Length > MaxNameLength)
            return Result.Failure(UnitOfMeasurementErrors.NameTooLong(MaxNameLength));

        if (!Enum.IsDefined(kind))
            return Result.Failure(UnitOfMeasurementErrors.InvalidKind());

        Name = name.Trim();
        Kind = kind;

        return Result.Success();
    }
}
