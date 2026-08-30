using MachineryManager.Configuration.Application.Abstractions;
using MachineryManager.Configuration.Application.Features.UnitCategories.Dtos;
using MachineryManager.SharedKernel;
using MachineryManager.SharedKernel.Abstractions;
using MediatR;

namespace MachineryManager.Configuration.Application.Features.UnitCategories.Queries.GetUnitCategoriesByHolding;

/// <summary>Handles <see cref="GetUnitCategoriesByHoldingQuery"/>.</summary>
public sealed class GetUnitCategoriesByHoldingQueryHandler
    : IRequestHandler<GetUnitCategoriesByHoldingQuery, Result<IReadOnlyList<UnitCategoryDto>>>
{
    private const string RequiredPermission = "UnitOfMeasurement.View";

    private readonly IUnitCategoryRepository _unitCategoryRepository;
    private readonly ICurrentUserService _currentUserService;
    private readonly IPermissionEvaluator _permissionEvaluator;

    /// <summary>Initializes a new instance of the <see cref="GetUnitCategoriesByHoldingQueryHandler"/> class.</summary>
    /// <param name="unitCategoryRepository">The Unit Category repository.</param>
    /// <param name="currentUserService">Provides the current authenticated user's identifier.</param>
    /// <param name="permissionEvaluator">Evaluates the current user's authorization.</param>
    public GetUnitCategoriesByHoldingQueryHandler(
        IUnitCategoryRepository unitCategoryRepository,
        ICurrentUserService currentUserService,
        IPermissionEvaluator permissionEvaluator)
    {
        _unitCategoryRepository = unitCategoryRepository;
        _currentUserService = currentUserService;
        _permissionEvaluator = permissionEvaluator;
    }

    /// <summary>Executes the query.</summary>
    /// <param name="request">The query to handle.</param>
    /// <param name="cancellationToken">Token to cancel the asynchronous operation.</param>
    /// <returns>A <see cref="Result{T}"/> containing the Holding's category list, or an authorization error.</returns>
    public async Task<Result<IReadOnlyList<UnitCategoryDto>>> Handle(
        GetUnitCategoriesByHoldingQuery request, CancellationToken cancellationToken)
    {
        if (_currentUserService.UserId is not { } userId)
        {
            return Result.Failure<IReadOnlyList<UnitCategoryDto>>(global::Configuration.Domain.UnitCategoryErrors.NotAuthorized());
        }

        var scope = new ResourceScope(request.HoldingId, null, null);
        var isAuthorized = await _permissionEvaluator.HasPermissionAsync(userId, RequiredPermission, scope, cancellationToken);

        if (!isAuthorized)
        {
            return Result.Failure<IReadOnlyList<UnitCategoryDto>>(global::Configuration.Domain.UnitCategoryErrors.NotAuthorized());
        }

        var categories = await _unitCategoryRepository.GetByHoldingAsync(request.HoldingId, cancellationToken);

        return Result.Success(categories);
    }
}