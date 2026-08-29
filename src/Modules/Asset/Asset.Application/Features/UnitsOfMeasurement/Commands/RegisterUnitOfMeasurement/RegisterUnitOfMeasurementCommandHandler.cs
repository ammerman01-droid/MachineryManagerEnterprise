using MachineryManager.Asset.Application.Abstractions;
using MachineryManager.SharedKernel;
using MachineryManager.SharedKernel.Abstractions;
using MediatR;

namespace MachineryManager.Asset.Application.Features.UnitsOfMeasurement.Commands.RegisterUnitOfMeasurement;

/// <summary>
/// Handles <see cref="RegisterUnitOfMeasurementCommand"/> by verifying
/// the caller holds UnitOfMeasurement.Create (a dedicated Permission
/// section, independent of Asset.* — chat, 2026-08-29), then invoking
/// domain registration.
/// </summary>
public sealed class RegisterUnitOfMeasurementCommandHandler
    : IRequestHandler<RegisterUnitOfMeasurementCommand, Result<Guid>>
{
    private const string RequiredPermission = "UnitOfMeasurement.Create";

    private readonly IUnitOfMeasurementRepository _unitOfMeasurementRepository;
    private readonly IAssetUnitOfWork _unitOfWork;
    private readonly IDateTimeProvider _dateTimeProvider;
    private readonly ICurrentUserService _currentUserService;
    private readonly IPermissionEvaluator _permissionEvaluator;
    private readonly IOrganizationLookupService _organizationLookupService;

    /// <summary>Initializes a new instance of the <see cref="RegisterUnitOfMeasurementCommandHandler"/> class.</summary>
    public RegisterUnitOfMeasurementCommandHandler(
        IUnitOfMeasurementRepository unitOfMeasurementRepository,
        IAssetUnitOfWork unitOfWork,
        IDateTimeProvider dateTimeProvider,
        ICurrentUserService currentUserService,
        IPermissionEvaluator permissionEvaluator,
        IOrganizationLookupService organizationLookupService)
    {
        _unitOfMeasurementRepository = unitOfMeasurementRepository;
        _unitOfWork = unitOfWork;
        _dateTimeProvider = dateTimeProvider;
        _currentUserService = currentUserService;
        _permissionEvaluator = permissionEvaluator;
        _organizationLookupService = organizationLookupService;
    }

    /// <summary>Executes the registration use case.</summary>
    public async Task<Result<Guid>> Handle(RegisterUnitOfMeasurementCommand request, CancellationToken cancellationToken)
    {
        if (_currentUserService.UserId is not { } userId)
        {
            return Result.Failure<Guid>(global::Asset.Domain.UnitOfMeasurementErrors.NotAuthorized());
        }

        var holdingId = await _organizationLookupService.GetHoldingIdAsync(request.OrganizationId, cancellationToken);
        var scope = new ResourceScope(holdingId, request.OrganizationId, null);

        var isAuthorized = await _permissionEvaluator.HasPermissionAsync(userId, RequiredPermission, scope, cancellationToken);

        if (!isAuthorized)
        {
            return Result.Failure<Guid>(global::Asset.Domain.UnitOfMeasurementErrors.NotAuthorized());
        }

        var result = global::Asset.Domain.UnitOfMeasurement.Register(
            request.OrganizationId, request.Name, request.Category, _dateTimeProvider);

        if (result.IsFailure)
        {
            return Result.Failure<Guid>(result.Error);
        }

        _unitOfMeasurementRepository.Add(result.Value);
        await _unitOfWork.SaveChangesAsync(cancellationToken);

        return Result.Success(result.Value.Id.Value);
    }
}