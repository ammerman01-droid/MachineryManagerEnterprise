using MachineryManager.SharedKernel;
using MachineryManager.SharedKernel.Abstractions;
using Organization.Domain.Events;

namespace Organization.Domain;

/// <summary>
/// Project: the bottom tier of the tenant hierarchy (chat,
/// 2026-08-19) — the scope that owns Assets, personnel, warehouses,
/// and workshops within a single Organization. A Project always
/// belongs to exactly one Organization.
/// </summary>
/// <remarks>
/// Per explicit clarification (chat, 2026-08-19): Project does NOT
/// own Assets, Personnel, or Warehouse inventory — Organization does,
/// unchanged from BR-017. Project represents only the CURRENT
/// operational assignment of those resources, which may change over
/// time (e.g. an Asset moves between Projects). Historical
/// Usage/Maintenance records remain permanently scoped to whichever
/// Project was current when each record was created (append-only,
/// per BR-016's Historical Entities principle) — that scoping logic
/// belongs to the Usage/Maintenance Bounded Contexts, not yet
/// implemented, and is not built here.
/// </remarks>
public sealed class Project : AggregateRoot<ProjectId>
{
    /// <summary>The maximum allowed length for a project name.</summary>
    public const int MaxNameLength = 200;

    /// <summary>Gets the identifier of the Organization that owns this Project.</summary>
    public OrganizationId OrganizationId { get; private set; }

    /// <summary>Gets the name of the project.</summary>
    public string Name { get; private set; }

    // Reserved for EF Core materialization only.
    private Project()
    {
        OrganizationId = null!;
        Name = string.Empty;
    }

    private Project(ProjectId id, OrganizationId organizationId, string name)
        : base(id)
    {
        OrganizationId = organizationId;
        Name = name;
    }

    /// <summary>Registers a new Project under the given Organization. This is the only way a Project comes into existence.</summary>
    /// <param name="organizationId">The identifier of the owning Organization.</param>
    /// <param name="name">The project's name.</param>
    /// <param name="dateTimeProvider">Provides the current UTC time for the raised domain event.</param>
    /// <returns>A <see cref="Result{Project}"/> containing the new project, or a validation error.</returns>
    public static Result<Project> Register(OrganizationId organizationId, string name, IDateTimeProvider dateTimeProvider)
    {
        if (organizationId is null)
        {
            return Result.Failure<Project>(ProjectErrors.OrganizationRequired());
        }

        if (string.IsNullOrWhiteSpace(name))
        {
            return Result.Failure<Project>(ProjectErrors.NameRequired());
        }

        if (name.Length > MaxNameLength)
        {
            return Result.Failure<Project>(ProjectErrors.NameTooLong(MaxNameLength));
        }

        var project = new Project(ProjectId.New(), organizationId, name.Trim());

        project.RaiseDomainEvent(
            new ProjectRegistered(project.Id, project.OrganizationId, project.Name, dateTimeProvider.UtcNow));

        return project;
    }
    
    /// <summary>Renames this Project.</summary>
    /// <param name="name">The new name.</param>
    /// <param name="dateTimeProvider">Provides the current UTC time for the raised domain event.</param>
    /// <returns>A <see cref="Result"/> indicating success or a validation error.</returns>
    public Result Rename(string name, IDateTimeProvider dateTimeProvider)
    {
        if (string.IsNullOrWhiteSpace(name))
        {
            return Result.Failure(ProjectErrors.NameRequired());
        }

        if (name.Length > MaxNameLength)
        {
            return Result.Failure(ProjectErrors.NameTooLong(MaxNameLength));
        }

        Name = name.Trim();

        RaiseDomainEvent(new ProjectRenamed(Id, Name, dateTimeProvider.UtcNow));

        return Result.Success();
    }
}