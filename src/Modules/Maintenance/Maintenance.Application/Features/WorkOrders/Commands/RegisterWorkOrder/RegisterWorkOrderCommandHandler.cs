using MachineryManagerEnterprise.Maintenance.Application.Abstractions;
using MachineryManagerEnterprise.Maintenance.Domain;
using MachineryManagerEnterprise.SharedKernel;
using MachineryManagerEnterprise.SharedKernel.Abstractions;
using MediatR;

namespace MachineryManagerEnterprise.Maintenance.Application.Features.WorkOrders.Commands.RegisterWorkOrder;

/// <summary>
/// Handles <see cref="RegisterWorkOrderCommand"/> by validating the
/// referenced Organization, Asset, and responsible Personnel, resolving
/// the Asset's current Project, reserving the next sequential Work
/// Order number, invoking domain registration, applying any resulting
/// Asset status change, persisting the aggregate, and committing the
/// unit of work.
/// </summary>
public sealed class RegisterWorkOrderCommandHandler
    : IRequestHandler<RegisterWorkOrderCommand, Result<Guid>>
{
    private const string RequiredPermission = "WorkOrder.Create";

    private readonly IWorkOrderRepository _workOrderRepository;
    private readonly IWorkOrderNumberGenerator _numberGenerator;
    private readonly IAssetLookupService _assetLookupService;
    private readonly IAssetStatusUpdateService _assetStatusUpdateService;
    private readonly IPersonnelLookupService _personnelLookupService;
    private readonly IMaintenanceUnitOfWork _unitOfWork;
    private readonly IDateTimeProvider _dateTimeProvider;
    private readonly ICurrentUserService _currentUserService;
    private readonly IPermissionEvaluator _permissionEvaluator;
    private readonly IOrganizationLookupService _organizationLookupService;

    /// <summary>Initializes a new instance of the <see cref="RegisterWorkOrderCommandHandler"/> class.</summary>
    /// <param name="workOrderRepository">The Work Order repository.</param>
    /// <param name="numberGenerator">Generates the Work Order's sequential, per-Organization number.</param>
    /// <param name="assetLookupService">Cross-module, read-only lookup into the Asset module, used to verify the referenced Asset exists, belongs to the target Organization, and to resolve its current Project.</param>
    /// <param name="assetStatusUpdateService">Cross-module write access into the Asset module, used to apply the Work Order's resulting Asset status.</param>
    /// <param name="personnelLookupService">Cross-module, read-only lookup into the Personnel module, used to verify the responsible Personnel exists and belongs to the target Organization.</param>
    /// <param name="unitOfWork">The Maintenance module's Unit of Work, used to commit the new aggregate.</param>
    /// <param name="dateTimeProvider">Provides the current UTC time for the raised domain event.</param>
    /// <param name="currentUserService">Provides the authenticated user context.</param>
    /// <param name="permissionEvaluator">Evaluates the current user's permissions at request time.</param>
    /// <param name="organizationLookupService">Cross-module, read-only lookup into the Organization module, used to verify the target Organization exists and to resolve its Holding.</param>
    public RegisterWorkOrderCommandHandler(
        IWorkOrderRepository workOrderRepository,
        IWorkOrderNumberGenerator numberGenerator,
        IAssetLookupService assetLookupService,
        IAssetStatusUpdateService assetStatusUpdateService,
        IPersonnelLookupService personnelLookupService,
        IMaintenanceUnitOfWork unitOfWork,
        IDateTimeProvider dateTimeProvider,
        ICurrentUserService currentUserService,
        IPermissionEvaluator permissionEvaluator,
        IOrganizationLookupService organizationLookupService)
    {
        _workOrderRepository = workOrderRepository;
        _numberGenerator = numberGenerator;
        _assetLookupService = assetLookupService;
        _assetStatusUpdateService = assetStatusUpdateService;
        _personnelLookupService = personnelLookupService;
        _unitOfWork = unitOfWork;
        _dateTimeProvider = dateTimeProvider;
        _currentUserService = currentUserService;
        _permissionEvaluator = permissionEvaluator;
        _organizationLookupService = organizationLookupService;
    }

    /// <summary>Executes the registration use case.</summary>
    /// <param name="request">The registration command.</param>
    /// <param name="cancellationToken">Token to cancel the asynchronous operation.</param>
    /// <returns>
    /// A <see cref="Result{Guid}"/> containing the new Work Order's identifier on success; otherwise a
    /// validation, not-found, conflict, or authorization error.
    /// </returns>
    public async Task<Result<Guid>> Handle(RegisterWorkOrderCommand request, CancellationToken cancellationToken)
    {
        if (_currentUserService.UserId is not { } userId)
        {
            return Result.Failure<Guid>(WorkOrderErrors.NotAuthorized());
        }

        var organizationExists = await _organizationLookupService.ExistsAsync(request.OrganizationId, cancellationToken);
        if (!organizationExists)
        {
            return Result.Failure<Guid>(WorkOrderErrors.OrganizationNotFound(request.OrganizationId));
        }

        var assetExists = await _assetLookupService.ExistsAsync(request.AssetId, cancellationToken);
        if (!assetExists)
        {
            return Result.Failure<Guid>(WorkOrderErrors.AssetNotFound(request.AssetId));
        }

        var assetOrganizationId = await _assetLookupService.GetOrganizationIdAsync(request.AssetId, cancellationToken);
        if (assetOrganizationId != request.OrganizationId)
        {
            return Result.Failure<Guid>(WorkOrderErrors.AssetOrganizationMismatch());
        }

        // Project is read from the Asset, never chosen separately
        // (chat, 2026-09-22).
        var projectId = await _assetLookupService.GetCurrentProjectIdAsync(request.AssetId, cancellationToken) ?? Guid.Empty;

        var holdingId = await _organizationLookupService.GetHoldingIdAsync(request.OrganizationId, cancellationToken);

        var isAuthorized = await _permissionEvaluator.HasPermissionAsync(
            userId,
            RequiredPermission,
            new ResourceScope(holdingId, request.OrganizationId, projectId),
            cancellationToken);

        if (!isAuthorized)
        {
            return Result.Failure<Guid>(WorkOrderErrors.NotAuthorized());
        }

        var personnelExists = await _personnelLookupService.ExistsAsync(request.ResponsiblePersonnelId, cancellationToken);
        if (!personnelExists)
        {
            return Result.Failure<Guid>(WorkOrderErrors.ResponsiblePersonnelNotFound(request.ResponsiblePersonnelId));
        }

        var personnelOrganizationId = await _personnelLookupService.GetOrganizationIdAsync(request.ResponsiblePersonnelId, cancellationToken);
        if (personnelOrganizationId != request.OrganizationId)
        {
            return Result.Failure<Guid>(WorkOrderErrors.ResponsiblePersonnelOrganizationMismatch());
        }

        var number = await _numberGenerator.GetNextNumberAsync(request.OrganizationId, cancellationToken);

        var result = WorkOrder.Register(
            request.OrganizationId,
            request.AssetId,
            projectId,
            number,
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
            return Result.Failure<Guid>(result.Error);
        }

        // Applied BEFORE persisting the Work Order (chat, 2026-09-22):
        // Asset and Maintenance are separate modules with separate
        // DbContexts/transactions, so true cross-module atomicity is
        // not possible. Attempting the Asset status change first and
        // aborting registration if it fails avoids the worse
        // inconsistency (a Work Order existing while the Asset's
        // status was never updated to match it).
        var statusUpdateResult = await _assetStatusUpdateService.SetOperationalStatusAsync(
            request.AssetId,
            outOfService: request.ResultingAssetStatus == WorkOrderAssetStatus.OutOfService,
            cancellationToken);

        if (statusUpdateResult.IsFailure)
        {
            return Result.Failure<Guid>(statusUpdateResult.Error);
        }

        _workOrderRepository.Add(result.Value);
        await _unitOfWork.SaveChangesAsync(cancellationToken);

        return Result.Success(result.Value.Id.Value);
    }
}