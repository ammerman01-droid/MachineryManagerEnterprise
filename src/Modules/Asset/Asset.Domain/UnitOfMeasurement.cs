using Asset.Domain.Events;
using MachineryManager.SharedKernel;
using MachineryManager.SharedKernel.Abstractions;

namespace Asset.Domain;

/// <summary>
/// Aggregate Root representing a selectable unit of measurement (e.g.
/// "اسب بخار" / "کیلووات" for power). Scoped per-Organization, same as
/// Color. Grouped by Category (e.g. Power, Volume, Length, Weight) for
/// future filtering — a free-text field for now, not a closed list
/// (chat, 2026-08-29).
/// </summary>
public sealed class UnitOfMeasurement : AggregateRoot<UnitOfMeasurementId>
{
    /// <summary>The maximum allowed length for the unit's name.</summary>
    public const int MaxNameLength = 50;

    /// <summary>The maximum allowed length for the category.</summary>
    public const int MaxCategoryLength = 50;

    /// <summary>Gets the identifier of the owning Organization.</summary>
    public Guid OrganizationId { get; private set; }

    /// <summary>Gets the display name of the unit (e.g. "کیلووات").</summary>
    public string Name { get; private set; } = string.Empty;

    /// <summary>Gets the category this unit belongs to (e.g. "Power", "Volume").</summary>
    public string Category { get; private set; } = string.Empty;

    // Reserved for ORM materialization only. Never used by application code.
    private UnitOfMeasurement()
    {
    }

    private UnitOfMeasurement(UnitOfMeasurementId id, Guid organizationId, string name, string category)
        : base(id)
    {
        OrganizationId = organizationId;
        Name = name;
        Category = category;
    }

    /// <summary>Registers a new Unit of Measurement within an Organization.</summary>
    /// <param name="organizationId">The owning Organization.</param>
    /// <param name="name">The display name (required, max 50 characters).</param>
    /// <param name="category">The category (required, max 50 characters).</param>
    /// <param name="dateTimeProvider">Provides the current UTC time for the raised domain event.</param>
    /// <returns>A <see cref="Result{UnitOfMeasurement}"/> containing the new aggregate, or a validation error.</returns>
    public static Result<UnitOfMeasurement> Register(
        Guid organizationId,
        string name,
        string category,
        IDateTimeProvider dateTimeProvider)
    {
        if (string.IsNullOrWhiteSpace(name))
        {
            return Result.Failure<UnitOfMeasurement>(UnitOfMeasurementErrors.NameRequired());
        }

        if (name.Length > MaxNameLength)
        {
            return Result.Failure<UnitOfMeasurement>(UnitOfMeasurementErrors.NameTooLong(MaxNameLength));
        }

        if (string.IsNullOrWhiteSpace(category))
        {
            return Result.Failure<UnitOfMeasurement>(UnitOfMeasurementErrors.CategoryRequired());
        }

        if (category.Length > MaxCategoryLength)
        {
            return Result.Failure<UnitOfMeasurement>(UnitOfMeasurementErrors.CategoryTooLong(MaxCategoryLength));
        }

        var unit = new UnitOfMeasurement(UnitOfMeasurementId.New(), organizationId, name.Trim(), category.Trim());

        unit.RaiseDomainEvent(new UnitOfMeasurementRegistered(
            unit.Id, organizationId, unit.Name, unit.Category, dateTimeProvider.UtcNow));

        return unit;
    }
}