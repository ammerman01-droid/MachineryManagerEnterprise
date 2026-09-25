using Configuration.Domain;
using MachineryManagerEnterprise.Configuration.Application.Abstractions;
using MachineryManagerEnterprise.Configuration.Application.Features.LubricantTypes.Dtos;
using Microsoft.EntityFrameworkCore;

namespace MachineryManagerEnterprise.Configuration.Infrastructure.Persistence;

/// <inheritdoc cref="ILubricantTypeRepository" />
public sealed class LubricantTypeRepository : ILubricantTypeRepository
{
    private readonly ConfigurationDbContext _dbContext;

    /// <summary>Initializes a new instance of the <see cref="LubricantTypeRepository"/> class.</summary>
    public LubricantTypeRepository(ConfigurationDbContext dbContext) => _dbContext = dbContext;

    /// <inheritdoc />
    public Task<LubricantType?> GetByIdAsync(LubricantTypeId id, CancellationToken cancellationToken = default) =>
        _dbContext.LubricantTypes.FirstOrDefaultAsync(l => l.Id == id, cancellationToken);

    /// <inheritdoc />
    public async Task<IReadOnlyList<LubricantTypeDto>> GetByHoldingAsync(Guid holdingId, CancellationToken cancellationToken = default)
    {
        var lubricantTypes = await _dbContext.LubricantTypes
            .AsNoTracking()
            .Where(l => l.HoldingId == holdingId)
            .OrderBy(l => l.Name)
            .Select(l => new LubricantTypeDto(l.Id.Value, l.Name))
            .ToListAsync(cancellationToken);

        return lubricantTypes;
    }

    /// <inheritdoc />
    public void Add(LubricantType lubricantType) => _dbContext.LubricantTypes.Add(lubricantType);

    /// <inheritdoc />
    public void Update(LubricantType lubricantType) => _dbContext.LubricantTypes.Update(lubricantType);

    /// <inheritdoc />
    public void Remove(LubricantType lubricantType) => _dbContext.LubricantTypes.Remove(lubricantType);
}
