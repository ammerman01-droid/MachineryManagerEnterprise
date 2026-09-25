using MachineryManagerEnterprise.SharedKernel;

namespace MachineryManagerEnterprise.Maintenance.Domain;

/// <summary>Strongly-typed identifier for a <see cref="WorkOrder"/> aggregate.</summary>
public sealed class WorkOrderId : ValueObject
{
    /// <summary>Gets the underlying GUID value.</summary>
    public Guid Value { get; }

    private WorkOrderId(Guid value) => Value = value;

    /// <summary>Creates a new, unique WorkOrderId.</summary>
    public static WorkOrderId New() => new(Guid.NewGuid());

    /// <summary>Wraps an existing identifier value (e.g. read from persistence).</summary>
    public static WorkOrderId From(Guid value) => new(value);

    /// <inheritdoc />
    protected override IEnumerable<object?> GetEqualityComponents()
    {
        yield return Value;
    }

    /// <inheritdoc />
    public override string ToString() => Value.ToString();
}
