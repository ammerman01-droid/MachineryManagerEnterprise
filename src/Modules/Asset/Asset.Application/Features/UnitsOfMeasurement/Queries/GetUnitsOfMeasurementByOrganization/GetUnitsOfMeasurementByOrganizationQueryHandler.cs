using MachineryManager.Asset.Application.Abstractions;
using MachineryManager.Asset.Application.Features.UnitsOfMeasurement.Dtos;
using MachineryManager.SharedKernel;
using MachineryManager.SharedKernel.Abstractions;
using MediatR;

namespace MachineryManager.Asset.Application.Features.UnitsOfMeasurement.Queries.GetUnitsOfMeasurementByOrganization;

/// <summary>
/// Handles <see cref="GetUnitsOfMeasurementByOrganizationQuery"/> by
/// verifying the caller holds UnitOfMeasurement.View, then returning
/// the Organization's full unit list.
/// </summary>
public sealed class GetUnitsOfMeasurementByOrganizationQueryHandler
    : IRequestHandler<GetUnitsOfMeasurementByOrganizationQuery, Result<IReadOnlyList<UnitOfMeasurementDto>>>
{
    private const string RequiredPermission = "UnitOfMeasurement.View";

    private readonly IUnitOfMeasurementRepository _unitOfMeasurementRepository;
    private readonly ICurrentUserService _currentUserService;
    private readonly IPermissionEvaluator _permissionEvaluator;
    private readonly IOrganizationLookupService _organizationLookupService;

    /// <summary>Initializes a new instance of the <see cref="GetUnitsOfMeasurementByOrganizationQueryHandler"/> class.</summary>
    public GetUnitsOfMeasurementByOrganizationQueryHandler(
        IUnitOfMeasurementRepository unitOfMeasurementRepository,
        ICurrentUserService currentUserService,
        IPermissionEvaluator permissionEvaluator,
        IOrganizationLookupService organizationLookupService)
    {
        _unitOfMeasurementRepository = unitOfMeasurementRepository;
        _currentUserService = currentUserService;
        _permissionEvaluator = permissionEvaluator;
        _organizationLookupService = organizationLookupService;
    }

    /// <summary>Executes the lookup use case.</summary>
    public async Task<Result<IReadOnlyList<UnitOfMeasurementDto>>> Handle(
        GetUnitsOfMeasurementByOrganizationQuery request,
        CancellationToken cancellationToken)
    {
        if (_currentUserService.UserId is not { } userId)
        {
            return Result.Failure<IReadOnlyList<UnitOfMeasurementDto>>(global::Asset.Domain.UnitOfMeasurementErrors.NotAuthorized());
        }

        var holdingId = await _organizationLookupService.GetHoldingIdAsync(request.OrganizationId, cancellationToken);
        var scope = new ResourceScope(holdingId, request.OrganizationId, null);

        var isAuthorized = await _permissionEvaluator.HasPermissionAsync(userId, RequiredPermission, scope, cancellationToken);

        if (!isAuthorized)
        {
            return Result.Failure<IReadOnlyList<UnitOfMeasurementDto>>(global::Asset.Domain.UnitOfMeasurementErrors.NotAuthorized());
        }

        var units = await _unitOfMeasurementRepository.GetByOrganizationAsync(request.OrganizationId, cancellationToken);

        return Result.Success(units);
    }
}