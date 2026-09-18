using Configuration.Domain;
using MachineryManagerEnterprise.Configuration.Application.Abstractions;
using MachineryManagerEnterprise.Configuration.Application.Features.JobTitles.Dtos;
using MapsterMapper;
using Microsoft.EntityFrameworkCore;

namespace MachineryManagerEnterprise.Configuration.Infrastructure.Persistence;

/// <summary>EF Core implementation of <see cref="IJobTitleRepository"/>.</summary>
public sealed class JobTitleRepository : IJobTitleRepository
{
    private readonly ConfigurationDbContext _dbContext;
    private readonly IMapper _mapper;

    /// <summary>Initializes a new instance of the <see cref="JobTitleRepository"/> class.</summary>
    public JobTitleRepository(ConfigurationDbContext dbContext, IMapper mapper)
    {
        _dbContext = dbContext;
        _mapper = mapper;
    }

    /// <inheritdoc />
    public Task<JobTitle?> GetByIdAsync(JobTitleId id, CancellationToken cancellationToken = default) =>
        _dbContext.JobTitles.FirstOrDefaultAsync(j => j.Id == id, cancellationToken);

    /// <inheritdoc />
    public void Add(JobTitle aggregate) => _dbContext.JobTitles.Add(aggregate);

    /// <inheritdoc />
    public void Update(JobTitle aggregate) => _dbContext.JobTitles.Update(aggregate);

    /// <inheritdoc />
    public void Remove(JobTitle aggregate) => _dbContext.JobTitles.Remove(aggregate);

    /// <inheritdoc />
    public async Task<IReadOnlyList<JobTitleDto>> GetByHoldingAsync(Guid holdingId, CancellationToken cancellationToken = default)
    {
        var entities = await _dbContext.JobTitles
            .AsNoTracking()
            .Where(j => j.HoldingId == holdingId)
            .OrderBy(j => j.Name)
            .ToListAsync(cancellationToken);

        return _mapper.Map<List<JobTitleDto>>(entities);
    }
}