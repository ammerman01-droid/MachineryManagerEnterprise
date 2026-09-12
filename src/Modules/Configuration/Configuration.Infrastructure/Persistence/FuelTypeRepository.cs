using Configuration.Domain;
using MachineryManagerEnterprise.Configuration.Application.Abstractions;
using MachineryManagerEnterprise.Configuration.Application.Features.FuelTypes.Dtos;
using MapsterMapper;
using Microsoft.EntityFrameworkCore;

namespace MachineryManagerEnterprise.Configuration.Infrastructure.Persistence;

/// <summary>EF Core implementation of <see cref="IFuelTypeRepository"/>.</summary>
public sealed class FuelTypeRepository : IFuelTypeRepository
{
    private readonly ConfigurationDbContext _dbContext;
    private readonly IMapper _mapper;

    /// <summary>Initializes a new instance of the <see cref="FuelTypeRepository"/> class.</summary>
    /// <param name="dbContext">The Configuration module's persistence context.</param>
    /// <param name="mapper">The Mapster-backed mapper used to project entities to DTOs.</param>
    public FuelTypeRepository(ConfigurationDbContext dbContext, IMapper mapper)
    {
        _dbContext = dbContext;
        _mapper = mapper;
    }

    /// <inheritdoc />
    public Task<FuelType?> GetByIdAsync(FuelTypeId id, CancellationToken cancellationToken = default) =>
        _dbContext.FuelTypes.FirstOrDefaultAsync(f => f.Id == id, cancellationToken);

    /// <inheritdoc />
    public void Add(FuelType aggregate) => _dbContext.FuelTypes.Add(aggregate);

    /// <inheritdoc />
    public void Update(FuelType aggregate) => _dbContext.FuelTypes.Update(aggregate);

    /// <inheritdoc />
    public void Remove(FuelType aggregate) => _dbContext.FuelTypes.Remove(aggregate);

    /// <inheritdoc />
    public async Task<IReadOnlyList<FuelTypeDto>> GetByHoldingAsync(Guid holdingId, CancellationToken cancellationToken = default)
    {
        var entities = await _dbContext.FuelTypes
            .AsNoTracking()
            .Where(f => f.HoldingId == holdingId)
            .OrderBy(f => f.Name)
            .ToListAsync(cancellationToken);

        return _mapper.Map<List<FuelTypeDto>>(entities);
    }
}