using MachineryManager.Configuration.Application.Abstractions;
using MachineryManager.Configuration.Application.Features.FuelTypes.Dtos;
using MachineryManager.SharedKernel;
using MachineryManager.SharedKernel.Abstractions;
using MediatR;

namespace MachineryManager.Configuration.Application.Features.FuelTypes.Queries.GetFuelTypesByHolding;

/// <summary>
/// Handles <see cref="GetFuelTypesByHoldingQuery"/> by verifying the
/// caller holds FuelType.View, then returning the Holding's full
/// fuel type catalog.
/// </summary>
public sealed class GetFuelTypesByHoldingQueryHandler
    : IRequestHandler<GetFuelTypesByHoldingQuery, Result<IReadOnlyList<FuelTypeDto>>>
{
    private const string RequiredPermission = "FuelType.View";

    private readonly IFuelTypeRepository _fuelTypeRepository;
    private readonly ICurrentUserService _currentUserService;
    private readonly IPermissionEvaluator _permissionEvaluator;

    /// <summary>Initializes a new instance of the <see cref="GetFuelTypesByHoldingQueryHandler"/> class.</summary>
    /// <param name="fuelTypeRepository">The Fuel Type repository.</param>
    /// <param name="currentUserService">Provides the authenticated user context.</param>
    /// <param name="permissionEvaluator">Evaluates the current user's permissions at request time.</param>
    public GetFuelTypesByHoldingQueryHandler(
        IFuelTypeRepository fuelTypeRepository,
        ICurrentUserService currentUserService,
        IPermissionEvaluator permissionEvaluator)
    {
        _fuelTypeRepository = fuelTypeRepository;
        _currentUserService = currentUserService;
        _permissionEvaluator = permissionEvaluator;
    }

    /// <summary>Executes the lookup use case.</summary>
    /// <param name="request">The query, containing the Holding whose fuel type catalog should be returned.</param>
    /// <param name="cancellationToken">Token to cancel the asynchronous operation.</param>
    /// <returns>A <see cref="Result{T}"/> containing the Holding's Fuel Type catalog on success; otherwise an authorization error.</returns>
    public async Task<Result<IReadOnlyList<FuelTypeDto>>> Handle(
        GetFuelTypesByHoldingQuery request, CancellationToken cancellationToken)
    {
        if (_currentUserService.UserId is not { } userId)
        {
            return Result.Failure<IReadOnlyList<FuelTypeDto>>(global::Configuration.Domain.FuelTypeErrors.NotAuthorized());
        }

        var scope = new ResourceScope(request.HoldingId, null, null);
        var isAuthorized = await _permissionEvaluator.HasPermissionAsync(userId, RequiredPermission, scope, cancellationToken);

        if (!isAuthorized)
        {
            return Result.Failure<IReadOnlyList<FuelTypeDto>>(global::Configuration.Domain.FuelTypeErrors.NotAuthorized());
        }

        var fuelTypes = await _fuelTypeRepository.GetByHoldingAsync(request.HoldingId, cancellationToken);

        return Result.Success(fuelTypes);
    }
}