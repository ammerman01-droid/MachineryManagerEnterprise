using Configuration.Domain.Events;
using MachineryManager.SharedKernel;
using MachineryManager.SharedKernel.Abstractions;

namespace Configuration.Domain;

/// <summary>
/// Aggregate Root representing a selectable body color for Assets.
/// Moved out of the Asset module into the independent Configuration
/// module (per Section 5.2's "Configuration" module definition) so it
/// can be reused by any future module. Scope promoted from
/// Organization to Holding (chat, 2026-08-30) — every Organization
/// under the same Holding now shares one color catalog, matching
/// AssetModel/EngineModel's scope.
/// </summary>
public sealed class Color : AggregateRoot<ColorId>
{
/// <summary>Gets the MaxNameLength constant.</summary>
    public const int MaxNameLength = 50;

/// <summary>Gets the HoldingId value.</summary>
    public Guid HoldingId { get; private set; }
/// <summary>Gets the Name value.</summary>
    public string Name { get; private set; } = string.Empty;

    private Color()
    {
    }

    private Color(ColorId id, Guid holdingId, string name)
        : base(id)
    {
        HoldingId = holdingId;
        Name = name;
    }

/// <summary>Executes the Register operation.</summary>
    public static Result<Color> Register(Guid holdingId, string name, IDateTimeProvider dateTimeProvider)
    {
        if (string.IsNullOrWhiteSpace(name))
        {
            return Result.Failure<Color>(ColorErrors.NameRequired());
        }

        if (name.Length > MaxNameLength)
        {
            return Result.Failure<Color>(ColorErrors.NameTooLong(MaxNameLength));
        }

        var color = new Color(ColorId.New(), holdingId, name.Trim());

        color.RaiseDomainEvent(new ColorRegistered(color.Id, holdingId, color.Name, dateTimeProvider.UtcNow));

        return color;
    }
}