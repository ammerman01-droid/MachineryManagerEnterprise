using MachineryManager.SharedKernel;

namespace Configuration.Domain.Events;

/// <summary>Represents the UnitCategoryRegistered type.</summary>
public sealed class UnitCategoryRegistered : IDomainEvent
{
/// <summary>Gets the UnitCategoryId value.</summary>
    public UnitCategoryId UnitCategoryId { get; }
/// <summary>Gets the HoldingId value.</summary>
    public Guid HoldingId { get; }
/// <summary>Gets the Name value.</summary>
    public string Name { get; }
/// <summary>Gets the OccurredOn value.</summary>
    public DateTimeOffset OccurredOn { get; }

/// <summary>Initializes a new instance of the <see cref="UnitCategoryRegistered"/> class.</summary>
    public UnitCategoryRegistered(UnitCategoryId unitCategoryId, Guid holdingId, string name, DateTimeOffset occurredOn)
    {
        UnitCategoryId = unitCategoryId;
        HoldingId = holdingId;
        Name = name;
        OccurredOn = occurredOn;
    }
}