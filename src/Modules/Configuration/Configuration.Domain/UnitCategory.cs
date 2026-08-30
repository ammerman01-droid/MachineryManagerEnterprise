using Configuration.Domain.Events;
using MachineryManager.SharedKernel;
using MachineryManager.SharedKernel.Abstractions;

namespace Configuration.Domain;

/// <summary>
/// Aggregate Root representing a category used to classify Units of
/// Measurement (e.g. "Power", "Volume"). Scope promoted from
/// Organization to Holding (chat, 2026-08-30).
/// </summary>
public sealed class UnitCategory : AggregateRoot<UnitCategoryId>
{
/// <summary>Gets the MaxNameLength constant.</summary>
    public const int MaxNameLength = 50;

/// <summary>Gets the HoldingId value.</summary>
    public Guid HoldingId { get; private set; }
/// <summary>Gets the Name value.</summary>
    public string Name { get; private set; } = string.Empty;

    private UnitCategory()
    {
    }

    private UnitCategory(UnitCategoryId id, Guid holdingId, string name)
        : base(id)
    {
        HoldingId = holdingId;
        Name = name;
    }

/// <summary>Executes the Register operation.</summary>
    public static Result<UnitCategory> Register(Guid holdingId, string name, IDateTimeProvider dateTimeProvider)
    {
        if (string.IsNullOrWhiteSpace(name))
        {
            return Result.Failure<UnitCategory>(UnitCategoryErrors.NameRequired());
        }

        if (name.Length > MaxNameLength)
        {
            return Result.Failure<UnitCategory>(UnitCategoryErrors.NameTooLong(MaxNameLength));
        }

        var category = new UnitCategory(UnitCategoryId.New(), holdingId, name.Trim());

        category.RaiseDomainEvent(new UnitCategoryRegistered(category.Id, holdingId, category.Name, dateTimeProvider.UtcNow));

        return category;
    }
}