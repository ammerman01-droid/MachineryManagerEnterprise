using Asset.Domain.Events;
using MachineryManager.SharedKernel;
using MachineryManager.SharedKernel.Abstractions;

namespace Asset.Domain;

/// <summary>
/// Aggregate Root representing a selectable color option for Assets.
/// Scoped per-Organization (chat, 2026-08-28) — each Organization
/// maintains its own color list, unlike AssetModel/EngineModel which
/// are Holding-scoped.
/// </summary>
public sealed class Color : AggregateRoot<ColorId>
{
    /// <summary>The maximum allowed length for the color name.</summary>
    public const int MaxNameLength = 50;

    /// <summary>Gets the identifier of the owning Organization.</summary>
    public Guid OrganizationId { get; private set; }

    /// <summary>Gets the display name of the color (e.g. "سفید", "مشکی").</summary>
    public string Name { get; private set; } = string.Empty;

    // Reserved for ORM materialization only. Never used by application code.
    private Color()
    {
    }

    private Color(ColorId id, Guid organizationId, string name)
        : base(id)
    {
        OrganizationId = organizationId;
        Name = name;
    }

    /// <summary>Registers a new Color option within an Organization.</summary>
    /// <param name="organizationId">The owning Organization.</param>
    /// <param name="name">The display name (required, max 50 characters).</param>
    /// <param name="dateTimeProvider">Provides the current UTC time for the raised domain event.</param>
    /// <returns>A <see cref="Result{Color}"/> containing the new aggregate, or a validation error.</returns>
    public static Result<Color> Register(Guid organizationId, string name, IDateTimeProvider dateTimeProvider)
    {
        if (string.IsNullOrWhiteSpace(name))
        {
            return Result.Failure<Color>(ColorErrors.NameRequired());
        }

        if (name.Length > MaxNameLength)
        {
            return Result.Failure<Color>(ColorErrors.NameTooLong(MaxNameLength));
        }

        var color = new Color(ColorId.New(), organizationId, name.Trim());

        color.RaiseDomainEvent(new ColorRegistered(color.Id, organizationId, color.Name, dateTimeProvider.UtcNow));

        return color;
    }
}