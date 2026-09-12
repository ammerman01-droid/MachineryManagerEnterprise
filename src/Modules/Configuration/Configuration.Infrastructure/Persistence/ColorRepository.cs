using Configuration.Domain;
using MachineryManagerEnterprise.Configuration.Application.Abstractions;
using MachineryManagerEnterprise.Configuration.Application.Features.Colors.Dtos;
using MapsterMapper;
using Microsoft.EntityFrameworkCore;

namespace MachineryManagerEnterprise.Configuration.Infrastructure.Persistence;

/// <summary>EF Core implementation of <see cref="IColorRepository"/>.</summary>
public sealed class ColorRepository : IColorRepository
{
    private readonly ConfigurationDbContext _dbContext;
    private readonly IMapper _mapper;

    /// <summary>Initializes a new instance of the <see cref="ColorRepository"/> class.</summary>
    /// <param name="dbContext">The Configuration module's persistence context.</param>
    /// <param name="mapper">The Mapster-backed mapper used to project entities to DTOs.</param>
    public ColorRepository(ConfigurationDbContext dbContext, IMapper mapper)
    {
        _dbContext = dbContext;
        _mapper = mapper;
    }

    /// <inheritdoc />
    public Task<Color?> GetByIdAsync(ColorId id, CancellationToken cancellationToken = default) =>
        _dbContext.Colors.FirstOrDefaultAsync(c => c.Id == id, cancellationToken);

    /// <inheritdoc />
    public void Add(Color aggregate) => _dbContext.Colors.Add(aggregate);

    /// <inheritdoc />
    public void Update(Color aggregate) => _dbContext.Colors.Update(aggregate);

    /// <inheritdoc />
    public void Remove(Color aggregate) => _dbContext.Colors.Remove(aggregate);

    /// <inheritdoc />
    public async Task<IReadOnlyList<ColorDto>> GetByHoldingAsync(Guid holdingId, CancellationToken cancellationToken = default)
    {
        var entities = await _dbContext.Colors
            .AsNoTracking()
            .Where(c => c.HoldingId == holdingId)
            .OrderBy(c => c.Name)
            .ToListAsync(cancellationToken);

        return _mapper.Map<List<ColorDto>>(entities);
    }
}