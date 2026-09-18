using Configuration.Domain;
using MachineryManagerEnterprise.Configuration.Application.Abstractions;
using MachineryManagerEnterprise.Configuration.Application.Features.DrivingLicenseTypes.Dtos;
using MapsterMapper;
using Microsoft.EntityFrameworkCore;

namespace MachineryManagerEnterprise.Configuration.Infrastructure.Persistence;

/// <summary>EF Core implementation of <see cref="IDrivingLicenseTypeRepository"/>.</summary>
public sealed class DrivingLicenseTypeRepository : IDrivingLicenseTypeRepository
{
    private readonly ConfigurationDbContext _dbContext;
    private readonly IMapper _mapper;

    /// <summary>Initializes a new instance of the <see cref="DrivingLicenseTypeRepository"/> class.</summary>
    public DrivingLicenseTypeRepository(ConfigurationDbContext dbContext, IMapper mapper)
    {
        _dbContext = dbContext;
        _mapper = mapper;
    }

    /// <inheritdoc />
    public Task<DrivingLicenseType?> GetByIdAsync(DrivingLicenseTypeId id, CancellationToken cancellationToken = default) =>
        _dbContext.DrivingLicenseTypes.FirstOrDefaultAsync(d => d.Id == id, cancellationToken);

    /// <inheritdoc />
    public void Add(DrivingLicenseType aggregate) => _dbContext.DrivingLicenseTypes.Add(aggregate);

    /// <inheritdoc />
    public void Update(DrivingLicenseType aggregate) => _dbContext.DrivingLicenseTypes.Update(aggregate);

    /// <inheritdoc />
    public void Remove(DrivingLicenseType aggregate) => _dbContext.DrivingLicenseTypes.Remove(aggregate);

    /// <inheritdoc />
    public async Task<IReadOnlyList<DrivingLicenseTypeDto>> GetByHoldingAsync(Guid holdingId, CancellationToken cancellationToken = default)
    {
        var entities = await _dbContext.DrivingLicenseTypes
            .AsNoTracking()
            .Where(d => d.HoldingId == holdingId)
            .OrderBy(d => d.Name)
            .ToListAsync(cancellationToken);

        return _mapper.Map<List<DrivingLicenseTypeDto>>(entities);
    }
}