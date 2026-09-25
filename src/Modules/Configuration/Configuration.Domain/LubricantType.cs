using Configuration.Domain.Events;
using MachineryManagerEnterprise.SharedKernel;
using MachineryManagerEnterprise.SharedKernel.Abstractions;

namespace Configuration.Domain;

/// <summary>
/// Aggregate Root representing a manageable Lubricant Type option (e.g.
/// "Engine Oil 15W40", "Hydraulic Oil ISO VG46", "Antifreeze/Coolant"),
/// used by the Consumption module's Lubricant Overflow Report feature.
/// Lives in the Configuration module as reference/master data,
/// Holding-scoped, user-definable — same pattern as <see cref="JobTitle"/>
/// and <see cref="DrivingLicenseType"/>. The Name carries both grade and
/// commercial name together (e.g. "Engine Oil 15W40"), matching how the
/// business describes a lubricant in practice (chat, 2026-09-16).
/// </summary>
public sealed class LubricantType : AggregateRoot<LubricantTypeId>
{
    /// <summary>Gets the maximum allowed length of <see cref="Name"/>.</summary>
    public const int MaxNameLength = 100;

    /// <summary>Gets the identifier of the owning Holding.</summary>
    public Guid HoldingId { get; private set; }

    /// <summary>Gets the name of the Lubricant Type (grade and commercial name together, e.g. "Engine Oil 15W40").</summary>
    public string Name { get; private set; } = string.Empty;

    private LubricantType()
    {
    }

    private LubricantType(LubricantTypeId id, Guid holdingId, string name)
        : base(id)
    {
        HoldingId = holdingId;
        Name = name;
    }

    /// <summary>Registers a new Lubricant Type option within a Holding.</summary>
    public static Result<LubricantType> Register(Guid holdingId, string name, IDateTimeProvider dateTimeProvider)
    {
        if (string.IsNullOrWhiteSpace(name))
        {
            return Result.Failure<LubricantType>(LubricantTypeErrors.NameRequired());
        }

        if (name.Length > MaxNameLength)
        {
            return Result.Failure<LubricantType>(LubricantTypeErrors.NameTooLong(MaxNameLength));
        }

        var lubricantType = new LubricantType(LubricantTypeId.New(), holdingId, name.Trim());

        lubricantType.RaiseDomainEvent(new LubricantTypeRegistered(lubricantType.Id, holdingId, lubricantType.Name, dateTimeProvider.UtcNow));

        return lubricantType;
    }

    /// <summary>Renames this Lubricant Type.</summary>
    public Result Rename(string name, IDateTimeProvider dateTimeProvider)
    {
        if (string.IsNullOrWhiteSpace(name))
        {
            return Result.Failure(LubricantTypeErrors.NameRequired());
        }

        if (name.Length > MaxNameLength)
        {
            return Result.Failure(LubricantTypeErrors.NameTooLong(MaxNameLength));
        }

        Name = name.Trim();
        RaiseDomainEvent(new LubricantTypeRenamed(Id, Name, dateTimeProvider.UtcNow));

        return Result.Success();
    }
}
