using MachineryManager.SharedKernel;

namespace Asset.Domain.Events;

/// <summary>
/// Raised when a new Unit of Measurement is registered.
/// </summary>
public sealed class UnitOfMeasurementRegistered : IDomainEvent
{
    /// <summary>
    /// Gets the identifier of the registered Unit of Measurement.
    /// </summary>
    public UnitOfMeasurementId UnitOfMeasurementId { get; }

    /// <summary>
    /// Gets the identifier of the owning Organization.
    /// </summary>
    public Guid OrganizationId { get; }

    /// <summary>
    /// Gets the name of the registered Unit of Measurement.
    /// </summary>
    public string Name { get; }

    /// <summary>
    /// Gets the category of the Unit of Measurement.
    /// </summary>
    public string Category { get; }

    /// <summary>
    /// Gets the UTC timestamp when the event occurred.
    /// </summary>
    public DateTimeOffset OccurredOn { get; }

    /// <summary>
    /// Initializes a new instance of the <see cref="UnitOfMeasurementRegistered"/> class.
    /// </summary>
    /// <param name="unitOfMeasurementId">
    /// The identifier of the registered Unit of Measurement.
    /// </param>
    /// <param name="organizationId">
    /// The identifier of the owning Organization.
    /// </param>
    /// <param name="name">
    /// The name of the registered Unit of Measurement.
    /// </param>
    /// <param name="category">
    /// The category of the Unit of Measurement.
    /// </param>
    /// <param name="occurredOn">
    /// The UTC timestamp when the event occurred.
    /// </param>
    public UnitOfMeasurementRegistered(
        UnitOfMeasurementId unitOfMeasurementId,
        Guid organizationId,
        string name,
        string category,
        DateTimeOffset occurredOn)
    {
        UnitOfMeasurementId = unitOfMeasurementId;
        OrganizationId = organizationId;
        Name = name;
        Category = category;
        OccurredOn = occurredOn;
    }
}