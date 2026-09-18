using MachineryManagerEnterprise.Personnel.Application.Abstractions;
using MachineryManagerEnterprise.Personnel.Application.Features.Dtos;
using MachineryManagerEnterprise.Personnel.Domain;
using MapsterMapper;
using Microsoft.EntityFrameworkCore;

namespace MachineryManagerEnterprise.Personnel.Infrastructure.Persistence;

/// <summary>EF Core implementation of <see cref="IPersonnelRepository"/>.</summary>
public sealed class PersonnelRepository : IPersonnelRepository
{
    private readonly PersonnelDbContext _dbContext;
    private readonly IMapper _mapper;

    /// <summary>Initializes a new instance of the <see cref="PersonnelRepository"/> class.</summary>
    public PersonnelRepository(PersonnelDbContext dbContext, IMapper mapper)
    {
        _dbContext = dbContext;
        _mapper = mapper;
    }

    /// <inheritdoc />
    public Task<global::MachineryManagerEnterprise.Personnel.Domain.Personnel?> GetByIdAsync(PersonnelId id, CancellationToken cancellationToken = default) =>
        _dbContext.PersonnelRecords
            .Include(p => p.DrivingLicenses)
            .FirstOrDefaultAsync(p => p.Id == id, cancellationToken);

    /// <inheritdoc />
    public void Add(global::MachineryManagerEnterprise.Personnel.Domain.Personnel aggregate) => _dbContext.PersonnelRecords.Add(aggregate);

    /// <inheritdoc />
    public void Update(global::MachineryManagerEnterprise.Personnel.Domain.Personnel aggregate) => _dbContext.PersonnelRecords.Update(aggregate);

    /// <inheritdoc />
    public void Remove(global::MachineryManagerEnterprise.Personnel.Domain.Personnel aggregate) => _dbContext.PersonnelRecords.Remove(aggregate);

    /// <inheritdoc />
    public async Task<IReadOnlyList<PersonnelDto>> GetByOrganizationAsync(Guid organizationId, CancellationToken cancellationToken = default)
    {
        var entities = await _dbContext.PersonnelRecords
            .AsNoTracking()
            .Include(p => p.DrivingLicenses)
            .Where(p => p.OrganizationId == organizationId)
            .OrderBy(p => p.LastName)
            .ThenBy(p => p.FirstName)
            .ToListAsync(cancellationToken);

        return _mapper.Map<List<PersonnelDto>>(entities);
    }

    /// <inheritdoc />
    public async Task<bool> ExistsByCodeInOrganizationAsync(Guid organizationId, string personnelCode, CancellationToken cancellationToken = default)
    {
        return await _dbContext.PersonnelRecords
            .AsNoTracking()
            .AnyAsync(p => p.OrganizationId == organizationId && p.PersonnelCode == personnelCode, cancellationToken);
    }

        /// <inheritdoc />
    public async Task<bool> ExistsByCodeInOrganizationAsync(Guid organizationId, string personnelCode, Guid excludingPersonnelId, CancellationToken cancellationToken = default)
    {
        var excludingId = PersonnelId.From(excludingPersonnelId);

        return await _dbContext.PersonnelRecords
            .AsNoTracking()
            .AnyAsync(p => p.OrganizationId == organizationId && p.PersonnelCode == personnelCode && p.Id != excludingId, cancellationToken);
    }

}