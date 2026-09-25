using MachineryManagerEnterprise.SharedKernel;

namespace Consumption.Domain;

/// <summary>Represents the strongly-typed identifier of a <see cref="LubricantOverflowReport"/> aggregate.</summary>
public sealed class LubricantOverflowReportId : ValueObject
{
    /// <summary>Gets the underlying identifier value.</summary>
    public Guid Value { get; }

    private LubricantOverflowReportId(Guid value) => Value = value;

    /// <summary>Creates a new, unique identifier.</summary>
    public static LubricantOverflowReportId New() => new(Guid.NewGuid());

    /// <summary>Wraps an existing identifier value.</summary>
    public static LubricantOverflowReportId From(Guid value) => new(value);

    /// <inheritdoc />
    protected override IEnumerable<object?> GetEqualityComponents()
    {
        yield return Value;
    }

    /// <inheritdoc />
    public override string ToString() => Value.ToString();
}
