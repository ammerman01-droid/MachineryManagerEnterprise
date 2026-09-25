using MachineryManagerEnterprise.Maintenance.Application.Abstractions;
using MachineryManagerEnterprise.Maintenance.Application.Features.WorkOrders.Dtos;
using MachineryManagerEnterprise.Maintenance.Application.Features.WorkOrders.Queries.SearchWorkOrders;
using MachineryManagerEnterprise.Maintenance.Domain;
using MapsterMapper;
using Microsoft.EntityFrameworkCore;

namespace MachineryManagerEnterprise.Maintenance.Infrastructure.Persistence;

/// <summary>EF Core implementation of <see cref="IWorkOrderRepository"/>.</summary>
public sealed class WorkOrderRepository : IWorkOrderRepository
{
    private readonly MaintenanceDbContext _dbContext;
    private readonly IMapper _mapper;

    /// <summary>Initializes a new instance of the <see cref="WorkOrderRepository"/> class.</summary>
    /// <param name="dbContext">The Maintenance module's persistence context.</param>
    /// <param name="mapper">The Mapster-backed mapper used to project entities to DTOs.</param>
    public WorkOrderRepository(MaintenanceDbContext dbContext, IMapper mapper)
    {
        _dbContext = dbContext;
        _mapper = mapper;
    }

    /// <inheritdoc />
    public Task<WorkOrder?> GetByIdAsync(WorkOrderId id, CancellationToken cancellationToken = default) =>
        _dbContext.WorkOrders.FirstOrDefaultAsync(w => w.Id == id, cancellationToken);

    /// <inheritdoc />
    public void Add(WorkOrder aggregate) => _dbContext.WorkOrders.Add(aggregate);

    /// <inheritdoc />
    public void Update(WorkOrder aggregate) => _dbContext.WorkOrders.Update(aggregate);

    /// <inheritdoc />
    public void Remove(WorkOrder aggregate) => _dbContext.WorkOrders.Remove(aggregate);

    /// <inheritdoc />
    public async Task<SearchWorkOrdersResponse> SearchAsync(
        Guid organizationId,
        string? searchTerm,
        int page,
        int pageSize,
        CancellationToken cancellationToken = default)
    {
        var query = _dbContext.WorkOrders
            .AsNoTracking()
            .Where(w => w.OrganizationId == organizationId);

        if (!string.IsNullOrWhiteSpace(searchTerm))
        {
            if (int.TryParse(searchTerm, out var numberTerm))
            {
                query = query.Where(w =>
                    w.Number == numberTerm ||
                    w.ObservationDescription.Contains(searchTerm));
            }
            else
            {
                query = query.Where(w => w.ObservationDescription.Contains(searchTerm));
            }
        }

        var totalItems = await query.CountAsync(cancellationToken);

        // Materialize entities first, map to DTO in memory via
        // Mapster's IMapper afterward, mirroring AssetRepository's
        // documented EF Core 10 Select-projection workaround for
        // field-backed value-object properties.
        var entities = await query
            .OrderByDescending(w => w.Number)
            .Skip((page - 1) * pageSize)
            .Take(pageSize)
            .ToListAsync(cancellationToken);

        var items = _mapper.Map<List<WorkOrderDto>>(entities);

        var totalPages = totalItems == 0 ? 0 : (int)Math.Ceiling(totalItems / (double)pageSize);

        return new SearchWorkOrdersResponse(
            items, page, pageSize, totalItems, totalPages, page < totalPages, page > 1);
    }
}
