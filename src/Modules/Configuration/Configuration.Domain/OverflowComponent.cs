using Configuration.Domain.Events;
using MachineryManagerEnterprise.SharedKernel;
using MachineryManagerEnterprise.SharedKernel.Abstractions;

namespace Configuration.Domain;

/// <summary>
/// Aggregate Root representing a manageable Overflow Component option
/// (e.g. "Engine", "Gearbox", "Hydraulic Tank") — the machine part a
/// lubricant overflowed from, used by the Consumption module's
/// Lubricant Overflow Report feature. Lives in the Configuration
/// module as reference/master data, Holding-scoped, user-definable —
/// same pattern as <see cref="JobTitle"/> and <see cref="LubricantType"/>
/// (chat, 2026-09-16).
/// </summary>
public sealed class OverflowComponent : AggregateRoot<OverflowComponentId>
{
    /// <summary>Gets the maximum allowed length of <see cref="Name"/>.</summary>
    public const int MaxNameLength = 100;

    /// <summary>Gets the identifier of the owning Holding.</summary>
    public Guid HoldingId { get; private set; }

    /// <summary>Gets the name of the Overflow Component (e.g. "Engine", "Gearbox").</summary>
    public string Name { get; private set; } = string.Empty;

    private OverflowComponent()
    {
    }

    private OverflowComponent(OverflowComponentId id, Guid holdingId, string name)
        : base(id)
    {
        HoldingId = holdingId;
        Name = name;
    }

    /// <summary>Registers a new Overflow Component option within a Holding.</summary>
    public static Result<OverflowComponent> Register(Guid holdingId, string name, IDateTimeProvider dateTimeProvider)
    {
        if (string.IsNullOrWhiteSpace(name))
        {
            return Result.Failure<OverflowComponent>(OverflowComponentErrors.NameRequired());
        }

        if (name.Length > MaxNameLength)
        {
            return Result.Failure<OverflowComponent>(OverflowComponentErrors.NameTooLong(MaxNameLength));
        }

        var overflowComponent = new OverflowComponent(OverflowComponentId.New(), holdingId, name.Trim());

        overflowComponent.RaiseDomainEvent(new OverflowComponentRegistered(overflowComponent.Id, holdingId, overflowComponent.Name, dateTimeProvider.UtcNow));

        return overflowComponent;
    }

    /// <summary>Renames this Overflow Component.</summary>
    public Result Rename(string name, IDateTimeProvider dateTimeProvider)
    {
        if (string.IsNullOrWhiteSpace(name))
        {
            return Result.Failure(OverflowComponentErrors.NameRequired());
        }

        if (name.Length > MaxNameLength)
        {
            return Result.Failure(OverflowComponentErrors.NameTooLong(MaxNameLength));
        }

        Name = name.Trim();
        RaiseDomainEvent(new OverflowComponentRenamed(Id, Name, dateTimeProvider.UtcNow));

        return Result.Success();
    }
}
