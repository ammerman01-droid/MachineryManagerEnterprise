using MachineryManagerEnterprise.Configuration.Application.Abstractions;
using MachineryManagerEnterprise.Configuration.Application.Features.FuelTypes.Dtos;
using MachineryManagerEnterprise.SharedKernel;
using MachineryManagerEnterprise.SharedKernel.Abstractions;
using MapsterMapper;
using MediatR;

namespace MachineryManagerEnterprise.Configuration.Application.Features.FuelTypes.Queries.GetFuelTypeById;

/// <summary>Handles <see cref="GetFuelTypeByIdQuery"/>.</summary>
public sealed class GetFuelTypeByIdQueryHandler : IRequestHandler<GetFuelTypeByIdQuery, Result<FuelTypeDto>>
{
    private const string RequiredPermission = "FuelType.View";

    private readonly IFuelTypeRepository _fuelTypeRepository;
    private readonly ICurrentUserService _currentUserService;
    private readonly IPermissionEvaluator _permissionEvaluator;
    private readonly IMapper _mapper;

    /// <summary>Initializes a new instance of the <see cref="GetFuelTypeByIdQueryHandler"/> class.</summary>
    /// <param name="fuelTypeRepository">The Fuel Type repository.</param>
    /// <param name="currentUserService">Provides the current authenticated user's identifier.</param>
    /// <param name="permissionEvaluator">Evaluates the current user's authorization.</param>
    /// <param name="mapper">The Mapster-backed mapper used to project the entity to a DTO.</param>
    public GetFuelTypeByIdQueryHandler(
        IFuelTypeRepository fuelTypeRepository,
        ICurrentUserService currentUserService,
        IPermissionEvaluator permissionEvaluator,
        IMapper mapper)
    {
        _fuelTypeRepository = fuelTypeRepository;
        _currentUserService = currentUserService;
        _permissionEvaluator = permissionEvaluator;
        _mapper = mapper;
    }

    /// <summary>Executes the query.</summary>
    /// <param name="request">The query to handle.</param>
    /// <param name="cancellationToken">Token to cancel the asynchronous operation.</param>
    /// <returns>A <see cref="Result{FuelTypeDto}"/> containing the Fuel Type, or a not-found/authorization error.</returns>
    public async Task<Result<FuelTypeDto>> Handle(GetFuelTypeByIdQuery request, CancellationToken cancellationToken)
    {
        if (_currentUserService.UserId is not { } userId)
        {
            return Result.Failure<FuelTypeDto>(global::Configuration.Domain.FuelTypeErrors.NotAuthorized());
        }

        var fuelType = await _fuelTypeRepository.GetByIdAsync(
            global::Configuration.Domain.FuelTypeId.From(request.Id), cancellationToken);

        if (fuelType is null)
        {
            return Result.Failure<FuelTypeDto>(global::Configuration.Domain.FuelTypeErrors.NotFound(request.Id));
        }

        var scope = new ResourceScope(fuelType.HoldingId, null, null);
        var isAuthorized = await _permissionEvaluator.HasPermissionAsync(userId, RequiredPermission, scope, cancellationToken);

        if (!isAuthorized)
        {
            return Result.Failure<FuelTypeDto>(global::Configuration.Domain.FuelTypeErrors.NotAuthorized());
        }

        return Result.Success(_mapper.Map<FuelTypeDto>(fuelType));
    }
}
