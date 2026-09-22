using MachineryManagerEnterprise.Configuration.Application.Abstractions;
using MachineryManagerEnterprise.SharedKernel;
using MachineryManagerEnterprise.SharedKernel.Abstractions;
using MediatR;

namespace MachineryManagerEnterprise.Configuration.Application.Features.FuelTypes.Commands.DeactivateFuelType;

/// <summary>Handles <see cref="DeactivateFuelTypeCommand"/>.</summary>
public sealed class DeactivateFuelTypeCommandHandler : IRequestHandler<DeactivateFuelTypeCommand, Result>
{
    private const string RequiredPermission = "FuelType.Delete";

    private readonly IFuelTypeRepository _fuelTypeRepository;
    private readonly IConfigurationUnitOfWork _unitOfWork;
    private readonly IDateTimeProvider _dateTimeProvider;
    private readonly ICurrentUserService _currentUserService;
    private readonly IPermissionEvaluator _permissionEvaluator;

    /// <summary>Initializes a new instance of the <see cref="DeactivateFuelTypeCommandHandler"/> class.</summary>
    /// <param name="fuelTypeRepository">The Fuel Type repository.</param>
    /// <param name="unitOfWork">The Configuration module's unit of work.</param>
    /// <param name="dateTimeProvider">Provides the current UTC time.</param>
    /// <param name="currentUserService">Provides the current authenticated user's identifier.</param>
    /// <param name="permissionEvaluator">Evaluates the current user's authorization.</param>
    public DeactivateFuelTypeCommandHandler(
        IFuelTypeRepository fuelTypeRepository,
        IConfigurationUnitOfWork unitOfWork,
        IDateTimeProvider dateTimeProvider,
        ICurrentUserService currentUserService,
        IPermissionEvaluator permissionEvaluator)
    {
        _fuelTypeRepository = fuelTypeRepository;
        _unitOfWork = unitOfWork;
        _dateTimeProvider = dateTimeProvider;
        _currentUserService = currentUserService;
        _permissionEvaluator = permissionEvaluator;
    }

    /// <summary>Executes the deactivation use case.</summary>
    /// <param name="request">The command to handle.</param>
    /// <param name="cancellationToken">Token to cancel the asynchronous operation.</param>
    /// <returns>A successful <see cref="Result"/>, or a not-found/authorization/conflict error.</returns>
    public async Task<Result> Handle(DeactivateFuelTypeCommand request, CancellationToken cancellationToken)
    {
        if (_currentUserService.UserId is not { } userId)
        {
            return Result.Failure(global::Configuration.Domain.FuelTypeErrors.NotAuthorized());
        }

        var fuelType = await _fuelTypeRepository.GetByIdAsync(
            global::Configuration.Domain.FuelTypeId.From(request.Id), cancellationToken);

        if (fuelType is null)
        {
            return Result.Failure(global::Configuration.Domain.FuelTypeErrors.NotFound(request.Id));
        }

        var scope = new ResourceScope(fuelType.HoldingId, null, null);
        var isAuthorized = await _permissionEvaluator.HasPermissionAsync(userId, RequiredPermission, scope, cancellationToken);

        if (!isAuthorized)
        {
            return Result.Failure(global::Configuration.Domain.FuelTypeErrors.NotAuthorized());
        }

        var result = fuelType.Deactivate(_dateTimeProvider);

        if (result.IsFailure)
        {
            return result;
        }

        _fuelTypeRepository.Update(fuelType);
        await _unitOfWork.SaveChangesAsync(cancellationToken);

        return Result.Success();
    }
}
