using Configuration.Domain.Events;
using MachineryManagerEnterprise.SharedKernel;
using MachineryManagerEnterprise.SharedKernel.Abstractions;

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

    /// <summary>
    /// Gets whether this Color is currently active. A deactivated
    /// (soft-deleted) Color is kept in the database for historical and
    /// audit purposes, but is excluded from selection lists by default
    /// (chat, 2026-09-19).
    /// </summary>
    public bool IsActive { get; private set; } = true;

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

    /// <summary>
    /// Updates the Color's display name.
    /// </summary>
    /// <param name="name">The new display name.</param>
    /// <param name="dateTimeProvider">Provides the current UTC time.</param>
    /// <returns>A successful <see cref="Result"/>, or a validation failure.</returns>
    public Result Rename(string name, IDateTimeProvider dateTimeProvider)
    {
        if (string.IsNullOrWhiteSpace(name))
        {
            return Result.Failure(ColorErrors.NameRequired());
        }

        if (name.Length > MaxNameLength)
        {
            return Result.Failure(ColorErrors.NameTooLong(MaxNameLength));
        }

        var trimmed = name.Trim();

        if (trimmed == Name)
        {
            return Result.Success();
        }

        Name = trimmed;

        RaiseDomainEvent(new ColorUpdated(Id, HoldingId, Name, dateTimeProvider.UtcNow));

        return Result.Success();
    }

    /// <summary>
    /// Deactivates the Color (soft delete). A deactivated Color is
    /// excluded from selection lists but remains in the database.
    /// </summary>
    /// <param name="dateTimeProvider">Provides the current UTC time.</param>
    /// <returns>A successful <see cref="Result"/>, or a failure if the Color is already inactive.</returns>
    public Result Deactivate(IDateTimeProvider dateTimeProvider)
    {
        if (!IsActive)
        {
            return Result.Failure(ColorErrors.AlreadyInactive());
        }

        IsActive = false;

        RaiseDomainEvent(new ColorDeactivated(Id, HoldingId, dateTimeProvider.UtcNow));

        return Result.Success();
    }

    /// <summary>
    /// Reactivates a previously deactivated Color.
    /// </summary>
    /// <param name="dateTimeProvider">Provides the current UTC time.</param>
    /// <returns>A successful <see cref="Result"/>, or a failure if the Color is already active.</returns>
    public Result Activate(IDateTimeProvider dateTimeProvider)
    {
        if (IsActive)
        {
            return Result.Failure(ColorErrors.AlreadyActive());
        }

        IsActive = true;

        RaiseDomainEvent(new ColorActivated(Id, HoldingId, dateTimeProvider.UtcNow));

        return Result.Success();
    }
}
