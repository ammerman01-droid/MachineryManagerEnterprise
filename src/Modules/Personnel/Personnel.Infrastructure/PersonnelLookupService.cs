using MachineryManagerEnterprise.Personnel.Domain;
using MachineryManagerEnterprise.Personnel.Infrastructure.Persistence;
using MachineryManagerEnterprise.SharedKernel.Abstractions;
using Microsoft.EntityFrameworkCore;

namespace MachineryManagerEnterprise.Personnel.Infrastructure;

/// <summary>
/// EF Core-backed implementation of <see cref="IPersonnelLookupService"/>,
/// providing other modules (currently Consumption, for its
/// fuel-deliverer/fuel-receiver references) with read-only, cross-module
/// access to Personnel data without depending on
/// Personnel.Domain/Personnel.Application directly (Modular Monolith
/// boundary — same pattern as
/// the corresponding Organization lookup service).
/// </summary>
public sealed class PersonnelLookupService : IPersonnelLookupService
{
    private readonly PersonnelDbContext _dbContext;

    /// <summary>Initializes a new instance of the <see cref="PersonnelLookupService"/> class.</summary>
    /// <param name="dbContext">The Personnel module's persistence context.</param>
    public PersonnelLookupService(PersonnelDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    /// <summary>Determines whether a Personnel with the given identifier exists.</summary>
    /// <param name="personnelId">The identifier of the Personnel to look up.</param>
    /// <param name="cancellationToken">Token to cancel the asynchronous operation.</param>
    /// <returns><see langword="true"/> if the Personnel exists; otherwise <see langword="false"/>.</returns>
    public async Task<bool> ExistsAsync(Guid personnelId, CancellationToken cancellationToken = default)
    {
        var id = PersonnelId.From(personnelId);

        return await _dbContext.PersonnelRecords
            .AsNoTracking()
            .AnyAsync(p => p.Id == id, cancellationToken);
    }

    /// <summary>Retrieves the identifier of the Organization a Personnel belongs to.</summary>
    /// <param name="personnelId">The identifier of the Personnel to look up.</param>
    /// <param name="cancellationToken">Token to cancel the asynchronous operation.</param>
    /// <returns>
    /// The identifier of the owning Organization, or <see langword="null"/>
    /// if the Personnel does not exist.
    /// </returns>
    public async Task<Guid?> GetOrganizationIdAsync(Guid personnelId, CancellationToken cancellationToken = default)
    {
        var id = PersonnelId.From(personnelId);

        return await _dbContext.PersonnelRecords
            .AsNoTracking()
            .Where(p => p.Id == id)
            .Select(p => (Guid?)p.OrganizationId)
            .FirstOrDefaultAsync(cancellationToken);
    }

    /// <inheritdoc />
    public async Task<string?> GetFullNameAsync(Guid personnelId, CancellationToken cancellationToken = default)
    {
        var id = global::MachineryManagerEnterprise.Personnel.Domain.PersonnelId.From(personnelId);
        var personnel = await _dbContext.PersonnelRecords.AsNoTracking().FirstOrDefaultAsync(p => p.Id == id, cancellationToken);
        return personnel is null ? null : $"{personnel.FirstName} {personnel.LastName}";
    }
}