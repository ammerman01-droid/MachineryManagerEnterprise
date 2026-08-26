using Asset.Domain.Events;
using MachineryManager.SharedKernel;
using MachineryManager.SharedKernel.Abstractions;

namespace Asset.Domain;

/// <summary>
/// Aggregate Root representing a catalog of shared specifications for a
/// class of Engines (e.g. "Volvo D13"). Individual Engine instances
/// (own aggregate, not modeled in this increment) reference exactly one
/// EngineModel and inherit its shared specifications; identity-specific
/// data (serial number, install history) lives on the Engine instance
/// itself, not here (chat, 2026-08-25).
/// Scoped per Organization — each Organization maintains its own
/// catalog (chat, 2026-08-25).
/// </summary>
public sealed class EngineModel : AggregateRoot<EngineModelId>
{
    /// <summary>The maximum allowed length for the engine model's name.</summary>
    public const int MaxNameLength = 200;

    /// <summary>The maximum allowed length for the manufacturer's name.</summary>
    public const int MaxManufacturerLength = 200;

    /// <summary>Gets the identifier of the Organization that owns this catalog entry.</summary>
    public Guid OrganizationId { get; private set; }

    /// <summary>Gets the display name of the engine model (e.g. "D13").</summary>
    public string Name { get; private set; } = string.Empty;

    /// <summary>Gets the manufacturer of this engine model (e.g. "Volvo").</summary>
    public string Manufacturer { get; private set; } = string.Empty;

    // Reserved for ORM materialization only. Never used by application code.
    private EngineModel()
    {
    }

    private EngineModel(EngineModelId id, Guid organizationId, string name, string manufacturer)
        : base(id)
    {
        OrganizationId = organizationId;
        Name = name;
        Manufacturer = manufacturer;
    }

    /// <summary>Registers a new Engine Model.</summary>
    /// <param name="organizationId">The owning Organization.</param>
    /// <param name="name">The display name.</param>
    /// <param name="manufacturer">The manufacturer.</param>
    /// <param name="dateTimeProvider">Provides the current UTC time for the raised domain event.</param>
    /// <returns>A <see cref="Result{EngineModel}"/> containing the new aggregate, or a validation error.</returns>
    public static Result<EngineModel> Register(
        Guid organizationId,
        string name,
        string manufacturer,
        IDateTimeProvider dateTimeProvider)
    {
        if (string.IsNullOrWhiteSpace(name))
        {
            return Result.Failure<EngineModel>(EngineModelErrors.NameRequired());
        }

        if (name.Length > MaxNameLength)
        {
            return Result.Failure<EngineModel>(EngineModelErrors.NameTooLong(MaxNameLength));
        }

        if (string.IsNullOrWhiteSpace(manufacturer))
        {
            return Result.Failure<EngineModel>(EngineModelErrors.ManufacturerRequired());
        }

        var engineModel = new EngineModel(EngineModelId.New(), organizationId, name.Trim(), manufacturer.Trim());

        engineModel.RaiseDomainEvent(new EngineModelRegistered(
            engineModel.Id,
            organizationId,
            engineModel.Name,
            dateTimeProvider.UtcNow));

        return engineModel;
    }

    /// <summary>Renames this Engine Model.</summary>
    /// <param name="name">The new name.</param>
    /// <returns>A <see cref="Result"/> indicating success or a validation error.</returns>
    public Result Rename(string name)
    {
        if (string.IsNullOrWhiteSpace(name))
        {
            return Result.Failure(EngineModelErrors.NameRequired());
        }

        if (name.Length > MaxNameLength)
        {
            return Result.Failure(EngineModelErrors.NameTooLong(MaxNameLength));
        }

        Name = name.Trim();

        return Result.Success();
    }
}