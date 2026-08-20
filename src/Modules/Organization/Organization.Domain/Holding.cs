using MachineryManager.SharedKernel;
using MachineryManager.SharedKernel.Abstractions;
using Organization.Domain.Events;

namespace Organization.Domain;

/// <summary>
/// Holding: the top-level tenant grouping — a collection of one or
/// more Organizations under common administrative oversight (chat,
/// 2026-08-19). This resolves BR-017's previously open
/// "sub-organizations" question as a parent tier ABOVE Organization —
/// not a redefinition of Organization itself, which remains the
/// authorization scope boundary per BR-017.
/// </summary>
/// <remarks>
/// A Holding is optional: an Organization may exist without belonging
/// to any Holding (e.g. a standalone single-company tenant). This
/// aggregate is intentionally minimal — no lifecycle beyond
/// registration is added here until further specified.
/// </remarks>
public sealed class Holding : AggregateRoot<HoldingId>
{
    /// <summary>The maximum allowed length for a holding name.</summary>
    public const int MaxNameLength = 200;

    /// <summary>Gets the name of the holding.</summary>
    public string Name { get; private set; }

    // Reserved for EF Core materialization only.
    private Holding()
    {
        Name = string.Empty;
    }

    private Holding(HoldingId id, string name)
        : base(id)
    {
        Name = name;
    }

    /// <summary>Registers a new Holding. This is the only way a Holding comes into existence.</summary>
    /// <param name="name">The holding's name.</param>
    /// <param name="dateTimeProvider">Provides the current UTC time for the raised domain event.</param>
    /// <returns>A <see cref="Result{Holding}"/> containing the new holding, or a validation error.</returns>
    public static Result<Holding> Register(string name, IDateTimeProvider dateTimeProvider)
    {
        if (string.IsNullOrWhiteSpace(name))
        {
            return Result.Failure<Holding>(HoldingErrors.NameRequired());
        }

        if (name.Length > MaxNameLength)
        {
            return Result.Failure<Holding>(HoldingErrors.NameTooLong(MaxNameLength));
        }

        var holding = new Holding(HoldingId.New(), name.Trim());

        holding.RaiseDomainEvent(
            new HoldingRegistered(holding.Id, holding.Name, dateTimeProvider.UtcNow));

        return holding;
    }
}