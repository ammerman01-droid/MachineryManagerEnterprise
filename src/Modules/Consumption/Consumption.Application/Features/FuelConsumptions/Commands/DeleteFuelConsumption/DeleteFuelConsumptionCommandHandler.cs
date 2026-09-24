using MachineryManagerEnterprise.Consumption.Application.Abstractions;
using MachineryManagerEnterprise.Consumption.Domain;
using MachineryManagerEnterprise.SharedKernel;
using MachineryManagerEnterprise.SharedKernel.Abstractions;
using MediatR;

namespace MachineryManagerEnterprise.Consumption.Application.Features.FuelConsumptions.Commands.DeleteFuelConsumption;

/// <summary>Handles <see cref="DeleteFuelConsumptionCommand"/>.</summary>
public sealed class DeleteFuelConsumptionCommandHandler : IRequestHandler<DeleteFuelConsumptionCommand, Result>
{
    private const string RequiredPermission = "FuelConsumption.Delete";

    private readonly IFuelConsumptionRepository _fuelConsumptionRepository;
    private readonly IConsumptionUnitOfWork _unitOfWork;
    private readonly IOrganizationLookupService _organizationLookupService;
    private readonly ICurrentUserService _currentUserService;
    private readonly IPermissionEvaluator _permissionEvaluator;

    /// <summary>Initializes a new instance of the <see cref="DeleteFuelConsumptionCommandHandler"/> class.</summary>
    public DeleteFuelConsumptionCommandHandler(
        IFuelConsumptionRepository fuelConsumptionRepository,
        IConsumptionUnitOfWork unitOfWork,
        IOrganizationLookupService organizationLookupService,
        ICurrentUserService currentUserService,
        IPermissionEvaluator permissionEvaluator)
    {
        _fuelConsumptionRepository = fuelConsumptionRepository;
        _unitOfWork = unitOfWork;
        _organizationLookupService = organizationLookupService;
        _currentUserService = currentUserService;
        _permissionEvaluator = permissionEvaluator;
    }

    /// <inheritdoc />
    public async Task<Result> Handle(DeleteFuelConsumptionCommand request, CancellationToken cancellationToken)
    {
        if (_currentUserService.UserId is not { } userId)
        {
            return Result.Failure(FuelConsumptionErrors.NotAuthorized());
        }

        var record = await _fuelConsumptionRepository.GetByIdAsync(FuelConsumptionId.From(request.Id), cancellationToken);

        if (record is null)
        {
            return Result.Failure(FuelConsumptionErrors.NotFound(request.Id));
        }

        var holdingId = await _organizationLookupService.GetHoldingIdAsync(record.OrganizationId, cancellationToken);

        var isAuthorized = await _permissionEvaluator.HasPermissionAsync(
            userId,
            RequiredPermission,
            new ResourceScope(holdingId, record.OrganizationId, record.ProjectId),
            cancellationToken);

        if (!isAuthorized)
        {
            return Result.Failure(FuelConsumptionErrors.NotAuthorized());
        }

        _fuelConsumptionRepository.Remove(record);
        await _unitOfWork.SaveChangesAsync(cancellationToken);

        return Result.Success();
    }
}
