using MachineryManager.SharedKernel;

namespace Configuration.Domain.Events;

/// <summary>Represents the UnitOfMeasurementRegistered type.</summary>
public sealed class UnitOfMeasurementRegistered : IDomainEvent
{
/// <summary>Gets the UnitOfMeasurementId value.</summary>
    public UnitOfMeasurementId UnitOfMeasurementId { get; }
/// <summary>Gets the HoldingId value.</summary>
    public Guid HoldingId { get; }
/// <summary>Gets the Name value.</summary>
    public string Name { get; }
/// <summary>Gets the CategoryId value.</summary>
    public UnitCategoryId CategoryId { get; }
/// <summary>Gets the OccurredOn value.</summary>
    public DateTimeOffset OccurredOn { get; }

/// <summary>Initializes a new instance of the <see cref="UnitOfMeasurementRegistered"/> class.</summary>
    public UnitOfMeasurementRegistered(
        UnitOfMeasurementId unitOfMeasurementId,
        Guid holdingId,
        string name,
        UnitCategoryId categoryId,
        DateTimeOffset occurredOn)
    {
        UnitOfMeasurementId = unitOfMeasurementId;
        HoldingId = holdingId;
        Name = name;
        CategoryId = categoryId;
        OccurredOn = occurredOn;
    }
}