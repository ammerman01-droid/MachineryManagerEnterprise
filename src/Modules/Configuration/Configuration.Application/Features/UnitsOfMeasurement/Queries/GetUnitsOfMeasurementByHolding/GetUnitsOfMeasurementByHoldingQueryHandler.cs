using MachineryManager.Configuration.Application.Abstractions;
using MachineryManager.Configuration.Application.Features.UnitsOfMeasurement.Dtos;
using MachineryManager.SharedKernel;
using MachineryManager.SharedKernel.Abstractions;
using MediatR;

namespace MachineryManager.Configuration.Application.Features.UnitsOfMeasurement.Queries.GetUnitsOfMeasurementByHolding;

/// <summary>Handles <see cref="GetUnitsOfMeasurementByHoldingQuery"/>.</summary>
public sealed class GetUnitsOfMeasurementByHoldingQueryHandler
    : IRequestHandler<GetUnitsOfMeasurementByHoldingQuery, Result<IReadOnlyList<UnitOfMeasurementDto>>>
{
    private const string RequiredPermission = "UnitOfMeasurement.View";

    private readonly IUnitOfMeasurementRepository _unitOfMeasurementRepository;
    private readonly IUnitCategoryRepository _unitCategoryRepository;
    private readonly ICurrentUserService _currentUserService;
    private readonly IPermissionEvaluator _permissionEvaluator;

    /// <summary>Initializes a new instance of the <see cref="GetUnitsOfMeasurementByHoldingQueryHandler"/> class.</summary>
    /// <param name="unitOfMeasurementRepository">The Unit of Measurement repository.</param>
    /// <param name="unitCategoryRepository">The Unit Category repository, used to resolve each unit's category name.</param>
    /// <param name="currentUserService">Provides the current authenticated user's identifier.</param>
    /// <param name="permissionEvaluator">Evaluates the current user's authorization.</param>
    public GetUnitsOfMeasurementByHoldingQueryHandler(
        IUnitOfMeasurementRepository unitOfMeasurementRepository,
        IUnitCategoryRepository unitCategoryRepository,
        ICurrentUserService currentUserService,
        IPermissionEvaluator permissionEvaluator)
    {
        _unitOfMeasurementRepository = unitOfMeasurementRepository;
        _unitCategoryRepository = unitCategoryRepository;
        _currentUserService = currentUserService;
        _permissionEvaluator = permissionEvaluator;
    }

    /// <summary>Executes the query.</summary>
    /// <param name="request">The query to handle.</param>
    /// <param name="cancellationToken">Token to cancel the asynchronous operation.</param>
    /// <returns>A <see cref="Result{T}"/> containing the Holding's unit list (with category names), or an authorization error.</returns>
    public async Task<Result<IReadOnlyList<UnitOfMeasurementDto>>> Handle(
        GetUnitsOfMeasurementByHoldingQuery request, CancellationToken cancellationToken)
    {
        if (_currentUserService.UserId is not { } userId)
        {
            return Result.Failure<IReadOnlyList<UnitOfMeasurementDto>>(global::Configuration.Domain.UnitOfMeasurementErrors.NotAuthorized());
        }

        var scope = new ResourceScope(request.HoldingId, null, null);
        var isAuthorized = await _permissionEvaluator.HasPermissionAsync(userId, RequiredPermission, scope, cancellationToken);

        if (!isAuthorized)
        {
            return Result.Failure<IReadOnlyList<UnitOfMeasurementDto>>(global::Configuration.Domain.UnitOfMeasurementErrors.NotAuthorized());
        }

        var units = await _unitOfMeasurementRepository.GetByHoldingAsync(request.HoldingId, cancellationToken);
        var categories = await _unitCategoryRepository.GetByHoldingAsync(request.HoldingId, cancellationToken);
        var categoryNameById = categories.ToDictionary(c => c.Id, c => c.Name);

        var items = units
            .Select(u => new UnitOfMeasurementDto(
                u.Id, u.Name, u.CategoryId, categoryNameById.GetValueOrDefault(u.CategoryId, "—")))
            .ToList();

        return Result.Success<IReadOnlyList<UnitOfMeasurementDto>>(items);
    }
}