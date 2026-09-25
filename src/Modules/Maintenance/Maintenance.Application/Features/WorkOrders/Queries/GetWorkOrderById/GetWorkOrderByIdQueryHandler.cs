using MachineryManagerEnterprise.Maintenance.Application.Abstractions;
using MachineryManagerEnterprise.Maintenance.Application.Features.WorkOrders.Dtos;
using MachineryManagerEnterprise.Maintenance.Domain;
using MachineryManagerEnterprise.SharedKernel;
using MachineryManagerEnterprise.SharedKernel.Abstractions;
using MapsterMapper;
using MediatR;

namespace MachineryManagerEnterprise.Maintenance.Application.Features.WorkOrders.Queries.GetWorkOrderById;

/// <summary>
/// Handles <see cref="GetWorkOrderByIdQuery"/> by loading the
/// aggregate, verifying the caller is authorized for its scope, mapping
/// it to a DTO, and enriching it with the Asset's and responsible
/// Personnel's display names via cross-module lookups.
/// </summary>
public sealed class GetWorkOrderByIdQueryHandler
    : IRequestHandler<GetWorkOrderByIdQuery, Result<WorkOrderDto>>
{
    private const string RequiredPermission = "WorkOrder.View";

    private readonly IWorkOrderRepository _workOrderRepository;
    private readonly IAssetLookupService _assetLookupService;
    private readonly IPersonnelLookupService _personnelLookupService;
    private readonly ICurrentUserService _currentUserService;
    private readonly IPermissionEvaluator _permissionEvaluator;
    private readonly IOrganizationLookupService _organizationLookupService;
    private readonly IMapper _mapper;

    /// <summary>Initializes a new instance of the <see cref="GetWorkOrderByIdQueryHandler"/> class.</summary>
    public GetWorkOrderByIdQueryHandler(
        IWorkOrderRepository workOrderRepository,
        IAssetLookupService assetLookupService,
        IPersonnelLookupService personnelLookupService,
        ICurrentUserService currentUserService,
        IPermissionEvaluator permissionEvaluator,
        IOrganizationLookupService organizationLookupService,
        IMapper mapper)
    {
        _workOrderRepository = workOrderRepository;
        _assetLookupService = assetLookupService;
        _personnelLookupService = personnelLookupService;
        _currentUserService = currentUserService;
        _permissionEvaluator = permissionEvaluator;
        _organizationLookupService = organizationLookupService;
        _mapper = mapper;
    }

    /// <summary>Executes the lookup use case.</summary>
    public async Task<Result<WorkOrderDto>> Handle(GetWorkOrderByIdQuery request, CancellationToken cancellationToken)
    {
        var id = WorkOrderId.From(request.WorkOrderId);
        var workOrder = await _workOrderRepository.GetByIdAsync(id, cancellationToken);

        if (workOrder is null)
        {
            return Result.Failure<WorkOrderDto>(WorkOrderErrors.NotFound(request.WorkOrderId));
        }

        if (_currentUserService.UserId is not { } userId)
        {
            return Result.Failure<WorkOrderDto>(WorkOrderErrors.NotAuthorized());
        }

        var holdingId = await _organizationLookupService.GetHoldingIdAsync(workOrder.OrganizationId, cancellationToken);

        var isAuthorized = await _permissionEvaluator.HasPermissionAsync(
            userId,
            RequiredPermission,
            new ResourceScope(holdingId, workOrder.OrganizationId, workOrder.ProjectId),
            cancellationToken);

        if (!isAuthorized)
        {
            return Result.Failure<WorkOrderDto>(WorkOrderErrors.NotAuthorized());
        }

        var dto = _mapper.Map<WorkOrderDto>(workOrder);

        var assetCode = await _assetLookupService.GetCodeAsync(workOrder.AssetId, cancellationToken);
        var assetName = await _assetLookupService.GetNameAsync(workOrder.AssetId, cancellationToken);
        var responsibleName = await _personnelLookupService.GetFullNameAsync(workOrder.ResponsiblePersonnelId, cancellationToken);

        dto = dto with
        {
            AssetCode = assetCode,
            AssetName = assetName,
            ResponsiblePersonnelName = responsibleName,
        };

        return Result.Success(dto);
    }
}
