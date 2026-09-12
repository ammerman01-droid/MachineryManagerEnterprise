using Configuration.Domain;
using MachineryManagerEnterprise.Configuration.Application.Abstractions;
using MachineryManagerEnterprise.Configuration.Application.Features.UnitsOfMeasurement.Dtos;
using MapsterMapper;
using Microsoft.EntityFrameworkCore;

namespace MachineryManagerEnterprise.Configuration.Infrastructure.Persistence;

/// <summary>EF Core implementation of <see cref="IUnitOfMeasurementRepository"/>.</summary>
public sealed class UnitOfMeasurementRepository : IUnitOfMeasurementRepository
{
    private readonly ConfigurationDbContext _dbContext;
    private readonly IMapper _mapper;

    /// <summary>Initializes a new instance of the <see cref="UnitOfMeasurementRepository"/> class.</summary>
    /// <param name="dbContext">The Configuration module's persistence context.</param>
    /// <param name="mapper">The Mapster-backed mapper used to project entities to DTOs.</param>
    public UnitOfMeasurementRepository(ConfigurationDbContext dbContext, IMapper mapper)
    {
        _dbContext = dbContext;
        _mapper = mapper;
    }

    /// <inheritdoc />
    public Task<UnitOfMeasurement?> GetByIdAsync(UnitOfMeasurementId id, CancellationToken cancellationToken = default) =>
        _dbContext.UnitsOfMeasurement.FirstOrDefaultAsync(u => u.Id == id, cancellationToken);

    /// <inheritdoc />
    public void Add(UnitOfMeasurement aggregate) => _dbContext.UnitsOfMeasurement.Add(aggregate);

    /// <inheritdoc />
    public void Update(UnitOfMeasurement aggregate) => _dbContext.UnitsOfMeasurement.Update(aggregate);

    /// <inheritdoc />
    public void Remove(UnitOfMeasurement aggregate) => _dbContext.UnitsOfMeasurement.Remove(aggregate);

    /// <inheritdoc />
    public async Task<IReadOnlyList<UnitOfMeasurementDto>> GetByHoldingAsync(
        Guid holdingId, CancellationToken cancellationToken = default)
    {
        var entities = await _dbContext.UnitsOfMeasurement
            .AsNoTracking()
            .Where(u => u.HoldingId == holdingId)
            .OrderBy(u => u.Kind)
            .ThenBy(u => u.Name)
            .ToListAsync(cancellationToken);

        return _mapper.Map<List<UnitOfMeasurementDto>>(entities);
    }
}