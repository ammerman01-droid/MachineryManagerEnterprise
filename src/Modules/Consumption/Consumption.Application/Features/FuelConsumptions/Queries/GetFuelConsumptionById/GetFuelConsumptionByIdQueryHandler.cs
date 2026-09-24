using MachineryManagerEnterprise.Consumption.Application.Abstractions;
using MachineryManagerEnterprise.Consumption.Application.Features.FuelConsumptions.Dtos;
using MachineryManagerEnterprise.Consumption.Domain;
using MachineryManagerEnterprise.SharedKernel;
using MachineryManagerEnterprise.SharedKernel.Abstractions;
using MapsterMapper;
using MediatR;

namespace MachineryManagerEnterprise.Consumption.Application.Features.FuelConsumptions.Queries.GetFuelConsumptionById;

/// <summary>Handles <see cref="GetFuelConsumptionByIdQuery"/>.</summary>
public sealed class GetFuelConsumptionByIdQueryHandler
    : IRequestHandler<GetFuelConsumptionByIdQuery, Result<FuelConsumptionDto>>
{
    private const string RequiredPermission = "FuelConsumption.View";

    private readonly IFuelConsumptionRepository _fuelConsumptionRepository;
    private readonly IOrganizationLookupService _organizationLookupService;
    private readonly ICurrentUserService _currentUserService;
    private readonly IPermissionEvaluator _permissionEvaluator;
    private readonly IMapper _mapper;

    /// <summary>Initializes a new instance of the <see cref="GetFuelConsumptionByIdQueryHandler"/> class.</summary>
    public GetFuelConsumptionByIdQueryHandler(
        IFuelConsumptionRepository fuelConsumptionRepository,
        IOrganizationLookupService organizationLookupService,
        ICurrentUserService currentUserService,
        IPermissionEvaluator permissionEvaluator,
        IMapper mapper)
    {
        _fuelConsumptionRepository = fuelConsumptionRepository;
        _organizationLookupService = organizationLookupService;
        _currentUserService = currentUserService;
        _permissionEvaluator = permissionEvaluator;
        _mapper = mapper;
    }

    /// <inheritdoc />
    public async Task<Result<FuelConsumptionDto>> Handle(GetFuelConsumptionByIdQuery request, CancellationToken cancellationToken)
    {
        if (_currentUserService.UserId is not { } userId)
        {
            return Result.Failure<FuelConsumptionDto>(FuelConsumptionErrors.NotAuthorized());
        }

        var fuelConsumptionId = FuelConsumptionId.From(request.Id);
        var record = await _fuelConsumptionRepository.GetByIdAsync(fuelConsumptionId, cancellationToken);

        if (record is null)
        {
            return Result.Failure<FuelConsumptionDto>(FuelConsumptionErrors.NotFound(request.Id));
        }

        var holdingId = await _organizationLookupService.GetHoldingIdAsync(record.OrganizationId, cancellationToken);

        var isAuthorized = await _permissionEvaluator.HasPermissionAsync(
            userId,
            RequiredPermission,
            new ResourceScope(holdingId, record.OrganizationId, record.ProjectId),
            cancellationToken);

        if (!isAuthorized)
        {
            return Result.Failure<FuelConsumptionDto>(FuelConsumptionErrors.NotAuthorized());
        }

        return Result.Success(_mapper.Map<FuelConsumptionDto>(record));
    }
}
