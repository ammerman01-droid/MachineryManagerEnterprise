using MachineryManagerEnterprise.Maintenance.Application.Abstractions;
using MachineryManagerEnterprise.Maintenance.Domain;
using MachineryManagerEnterprise.SharedKernel;
using MachineryManagerEnterprise.SharedKernel.Abstractions;
using MediatR;

namespace MachineryManagerEnterprise.Maintenance.Application.Features.WorkOrders.Commands.EditWorkOrder;

/// <summary>
/// Handles <see cref="EditWorkOrderCommand"/> by loading the aggregate,
/// re-validating the (possibly changed) responsible Personnel,
/// invoking the domain edit behavior, re-applying any resulting Asset
/// status change, and committing the unit of work.
/// </summary>
public sealed class EditWorkOrderCommandHandler
    : IRequestHandler<EditWorkOrderCommand, Result>
{
    private const string RequiredPermission = "WorkOrder.Edit";

    private readonly IWorkOrderRepository _workOrderRepository;
    private readonly IAssetStatusUpdateService _assetStatusUpdateService;
    private readonly IPersonnelLookupService _personnelLookupService;
    private readonly IMaintenanceUnitOfWork _unitOfWork;
    private readonly IDateTimeProvider _dateTimeProvider;
    private readonly ICurrentUserService _currentUserService;
    private readonly IPermissionEvaluator _permissionEvaluator;
    private readonly IOrganizationLookupService _organizationLookupService;

    /// <summary>Initializes a new instance of the <see cref="EditWorkOrderCommandHandler"/> class.</summary>
    public EditWorkOrderCommandHandler(
        IWorkOrderRepository workOrderRepository,
        IAssetStatusUpdateService assetStatusUpdateService,
        IPersonnelLookupService personnelLookupService,
        IMaintenanceUnitOfWork unitOfWork,
        IDateTimeProvider dateTimeProvider,
        ICurrentUserService currentUserService,
        IPermissionEvaluator permissionEvaluator,
        IOrganizationLookupService organizationLookupService)
    {
        _workOrderRepository = workOrderRepository;
        _assetStatusUpdateService = assetStatusUpdateService;
        _personnelLookupService = personnelLookupService;
        _unitOfWork = unitOfWork;
        _dateTimeProvider = dateTimeProvider;
        _currentUserService = currentUserService;
        _permissionEvaluator = permissionEvaluator;
        _organizationLookupService = organizationLookupService;
    }

    /// <summary>Executes the edit use case.</summary>
    public async Task<Result> Handle(EditWorkOrderCommand request, CancellationToken cancellationToken)
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

        var personnelExists = await _personnelLookupService.ExistsAsync(request.ResponsiblePersonnelId, cancellationToken);
        if (!personnelExists)
        {
            return Result.Failure(WorkOrderErrors.ResponsiblePersonnelNotFound(request.ResponsiblePersonnelId));
        }

        var personnelOrganizationId = await _personnelLookupService.GetOrganizationIdAsync(request.ResponsiblePersonnelId, cancellationToken);
        if (personnelOrganizationId != workOrder.OrganizationId)
        {
            return Result.Failure(WorkOrderErrors.ResponsiblePersonnelOrganizationMismatch());
        }

        var previousResultingStatus = workOrder.ResultingAssetStatus;

        var result = workOrder.Edit(
            request.ReportedAt,
            request.ResultingAssetStatus,
            request.MeterReading,
            request.Priority,
            request.RepairType,
            request.ResponsiblePersonnelId,
            request.ObservationDescription,
            request.PredictedRepairLocation,
            request.PartNeedingRepair,
            _dateTimeProvider);

        if (result.IsFailure)
        {
            return result;
        }

        // Re-apply the resulting Asset status only if the edit actually
        // changed it (chat, 2026-09-22 — extends the register-time rule
        // to edits, so the Asset never drifts out of sync with a
        // subsequently-corrected Work Order).
        if (previousResultingStatus != request.ResultingAssetStatus)
        {
            var statusUpdateResult = await _assetStatusUpdateService.SetOperationalStatusAsync(
                workOrder.AssetId,
                outOfService: request.ResultingAssetStatus == WorkOrderAssetStatus.OutOfService,
                cancellationToken);

            if (statusUpdateResult.IsFailure)
            {
                return statusUpdateResult;
            }
        }

        _workOrderRepository.Update(workOrder);
        await _unitOfWork.SaveChangesAsync(cancellationToken);

        return Result.Success();
    }
}
