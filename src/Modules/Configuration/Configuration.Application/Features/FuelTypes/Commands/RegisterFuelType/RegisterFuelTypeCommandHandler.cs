using MachineryManager.Configuration.Application.Abstractions;
using MachineryManager.SharedKernel;
using MachineryManager.SharedKernel.Abstractions;
using MediatR;

namespace MachineryManager.Configuration.Application.Features.FuelTypes.Commands.RegisterFuelType;

/// <summary>
/// Handles the registration of a new Fuel Type. FuelType is
/// Holding-scoped, mirroring UnitCategory (chat, 2026-09-02).
/// </summary>
public sealed class RegisterFuelTypeCommandHandler
    : IRequestHandler<RegisterFuelTypeCommand, Result<Guid>>
{
    private const string RequiredPermission = "FuelType.Create";

    private readonly IFuelTypeRepository _fuelTypeRepository;
    private readonly IConfigurationUnitOfWork _unitOfWork;
    private readonly IDateTimeProvider _dateTimeProvider;
    private readonly ICurrentUserService _currentUserService;
    private readonly IPermissionEvaluator _permissionEvaluator;
    private readonly IHoldingLookupService _holdingLookupService;

    /// <summary>Initializes a new instance of the <see cref="RegisterFuelTypeCommandHandler"/> class.</summary>
    /// <param name="fuelTypeRepository">The Fuel Type repository.</param>
    /// <param name="unitOfWork">The Configuration module's Unit of Work.</param>
    /// <param name="dateTimeProvider">Provides the current UTC time for the raised domain event.</param>
    /// <param name="currentUserService">Provides the authenticated user context.</param>
    /// <param name="permissionEvaluator">Evaluates the current user's permissions at request time.</param>
    /// <param name="holdingLookupService">Cross-module, read-only lookup into the Organization module, used to verify the target Holding exists.</param>
    public RegisterFuelTypeCommandHandler(
        IFuelTypeRepository fuelTypeRepository,
        IConfigurationUnitOfWork unitOfWork,
        IDateTimeProvider dateTimeProvider,
        ICurrentUserService currentUserService,
        IPermissionEvaluator permissionEvaluator,
        IHoldingLookupService holdingLookupService)
    {
        _fuelTypeRepository = fuelTypeRepository;
        _unitOfWork = unitOfWork;
        _dateTimeProvider = dateTimeProvider;
        _currentUserService = currentUserService;
        _permissionEvaluator = permissionEvaluator;
        _holdingLookupService = holdingLookupService;
    }

    /// <summary>Handles the Fuel Type registration request.</summary>
    /// <param name="request">The registration command.</param>
    /// <param name="cancellationToken">Token to cancel the asynchronous operation.</param>
    /// <returns>A <see cref="Result{Guid}"/> containing the new fuel type's identifier on success; otherwise a validation, not-found, or authorization error.</returns>
    public async Task<Result<Guid>> Handle(RegisterFuelTypeCommand request, CancellationToken cancellationToken)
    {
        if (_currentUserService.UserId is not { } userId)
        {
            return Result.Failure<Guid>(global::Configuration.Domain.FuelTypeErrors.NotAuthorized());
        }

        var scope = new ResourceScope(request.HoldingId, null, null);

        var isAuthorized = await _permissionEvaluator.HasPermissionAsync(userId, RequiredPermission, scope, cancellationToken);

        if (!isAuthorized)
        {
            return Result.Failure<Guid>(global::Configuration.Domain.FuelTypeErrors.NotAuthorized());
        }

        var holdingExists = await _holdingLookupService.ExistsAsync(request.HoldingId, cancellationToken);

        if (!holdingExists)
        {
            return Result.Failure<Guid>(
                Error.NotFound("FuelType.HoldingNotFound", $"Holding with id {request.HoldingId} was not found."));
        }

        var result = global::Configuration.Domain.FuelType.Register(
            request.HoldingId, request.Name, request.Price, request.Kind, _dateTimeProvider);

        if (result.IsFailure)
        {
            return Result.Failure<Guid>(result.Error);
        }

        _fuelTypeRepository.Add(result.Value);
        await _unitOfWork.SaveChangesAsync(cancellationToken);

        return Result.Success(result.Value.Id.Value);
    }
}