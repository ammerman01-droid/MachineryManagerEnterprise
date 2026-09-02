using Configuration.Domain;
using MachineryManager.Configuration.Application.Abstractions;
using MachineryManager.Configuration.Application.Features.Companies.Dtos;
using Microsoft.EntityFrameworkCore;

namespace MachineryManager.Configuration.Infrastructure.Persistence;

/// <summary>EF Core implementation of <see cref="ICompanyRepository"/>.</summary>
public sealed class CompanyRepository : ICompanyRepository
{
    private readonly ConfigurationDbContext _dbContext;

    /// <summary>Initializes a new instance of the <see cref="CompanyRepository"/> class.</summary>
    /// <param name="dbContext">The Configuration module's persistence context.</param>
    public CompanyRepository(ConfigurationDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    /// <inheritdoc />
    public Task<Company?> GetByIdAsync(CompanyId id, CancellationToken cancellationToken = default) =>
        _dbContext.Companies.FirstOrDefaultAsync(c => c.Id == id, cancellationToken);

    /// <inheritdoc />
    public void Add(Company aggregate) => _dbContext.Companies.Add(aggregate);

    /// <inheritdoc />
    public void Update(Company aggregate) => _dbContext.Companies.Update(aggregate);

    /// <inheritdoc />
    public void Remove(Company aggregate) => _dbContext.Companies.Remove(aggregate);

    /// <inheritdoc />
    public async Task<IReadOnlyList<CompanyDto>> GetByHoldingAsync(
        Guid holdingId, CancellationToken cancellationToken = default)
    {
        var entities = await _dbContext.Companies
            .AsNoTracking()
            .Where(c => c.HoldingId == holdingId)
            .OrderBy(c => c.Name)
            .ToListAsync(cancellationToken);

        return entities.Select(c => new CompanyDto(c.Id.Value, c.Name)).ToList();
    }

    /// <inheritdoc />
    public Task<bool> ExistsByNameInHoldingAsync(
        Guid holdingId, string name, CancellationToken cancellationToken = default) =>
        _dbContext.Companies.AsNoTracking().AnyAsync(
            c => c.HoldingId == holdingId && c.Name == name, cancellationToken);
}