using MachineryManagerEnterprise.Maintenance.Application.Abstractions;
using MachineryManagerEnterprise.Maintenance.Domain;
using MachineryManagerEnterprise.SharedKernel;
using MachineryManagerEnterprise.SharedKernel.Abstractions;
using MediatR;

namespace MachineryManagerEnterprise.Maintenance.Application.Features.WorkOrders.Commands.CancelWorkOrder;

/// <summary>
/// Handles <see cref="CancelWorkOrderCommand"/> by loading the
/// aggregate, invoking the domain cancellation behavior, and
/// committing the unit of work.
/// </summary>
public sealed class CancelWorkOrderCommandHandler
    : IRequestHandler<CancelWorkOrderCommand, Result>
{
    private const string RequiredPermission = "WorkOrder.Edit";

    private readonly IWorkOrderRepository _workOrderRepository;
    private readonly IMaintenanceUnitOfWork _unitOfWork;
    private readonly IDateTimeProvider _dateTimeProvider;
    private readonly ICurrentUserService _currentUserService;
    private readonly IPermissionEvaluator _permissionEvaluator;
    private readonly IOrganizationLookupService _organizationLookupService;

    /// <summary>Initializes a new instance of the <see cref="CancelWorkOrderCommandHandler"/> class.</summary>
    public CancelWorkOrderCommandHandler(
        IWorkOrderRepository workOrderRepository,
        IMaintenanceUnitOfWork unitOfWork,
        IDateTimeProvider dateTimeProvider,
        ICurrentUserService currentUserService,
        IPermissionEvaluator permissionEvaluator,
        IOrganizationLookupService organizationLookupService)
    {
        _workOrderRepository = workOrderRepository;
        _unitOfWork = unitOfWork;
        _dateTimeProvider = dateTimeProvider;
        _currentUserService = currentUserService;
        _permissionEvaluator = permissionEvaluator;
        _organizationLookupService = organizationLookupService;
    }

    /// <summary>Executes the cancellation use case.</summary>
    public async Task<Result> Handle(CancelWorkOrderCommand request, CancellationToken cancellationToken)
    {
        var id = WorkOrderId.From(request.WorkOrderId);
        var workOrder = await _workOrderRepository.GetByIdAsync(id, cancellationToken);

        if (workOrder is null)
        {
            return Result.Failure(WorkOrderErrors.NotFound(request.WorkOrderId));
        }

        if (_currentUserService.UserId is not { } userId)
        {
            return Result.Failure(WorkOrderErrors.NotAuthorized());
        }

        var holdingId = await _organizationLookupService.GetHoldingIdAsync(workOrder.OrganizationId, cancellationToken);

        var isAuthorized = await _permissionEvaluator.HasPermissionAsync(
            userId,
            RequiredPermission,
            new ResourceScope(holdingId, workOrder.OrganizationId, workOrder.ProjectId),
            cancellationToken);

        if (!isAuthorized)
        {
            return Result.Failure(WorkOrderErrors.NotAuthorized());
        }

        var result = workOrder.Cancel(request.Reason, _dateTimeProvider);

        if (result.IsFailure)
        {
            return result;
        }

        _workOrderRepository.Update(workOrder);
        await _unitOfWork.SaveChangesAsync(cancellationToken);

        return Result.Success();
    }
}
