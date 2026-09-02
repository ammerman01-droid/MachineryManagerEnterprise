using Configuration.Domain.Events;
using MachineryManager.SharedKernel;
using MachineryManager.SharedKernel.Abstractions;

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
}