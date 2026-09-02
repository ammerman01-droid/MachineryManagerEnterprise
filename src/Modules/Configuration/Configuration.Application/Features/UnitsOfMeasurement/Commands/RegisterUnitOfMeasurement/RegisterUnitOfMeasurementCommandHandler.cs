using MachineryManager.Configuration.Application.Abstractions;
using MachineryManager.SharedKernel;
using MachineryManager.SharedKernel.Abstractions;
using MediatR;

namespace MachineryManager.Configuration.Application.Features.UnitsOfMeasurement.Commands.RegisterUnitOfMeasurement;

/// <summary>Handles <see cref="RegisterUnitOfMeasurementCommand"/>.</summary>
public sealed class RegisterUnitOfMeasurementCommandHandler : IRequestHandler<RegisterUnitOfMeasurementCommand, Result<Guid>>
{
    private const string RequiredPermission = "UnitOfMeasurement.Create";

    private readonly IUnitOfMeasurementRepository _unitOfMeasurementRepository;
    private readonly IHoldingLookupService _holdingLookupService;
    private readonly IConfigurationUnitOfWork _unitOfWork;
    private readonly IDateTimeProvider _dateTimeProvider;
    private readonly ICurrentUserService _currentUserService;
    private readonly IPermissionEvaluator _permissionEvaluator;

/// <summary>Initializes a new instance of the <see cref="RegisterUnitOfMeasurementCommandHandler"/> class.</summary>
    /// <param name="unitOfMeasurementRepository">The Unit of Measurement repository.</param>
    /// <param name="holdingLookupService">Used to verify the target Holding exists.</param>
    /// <param name="unitOfWork">The Configuration module's unit of work.</param>
    /// <param name="dateTimeProvider">Provides the current UTC time.</param>
    /// <param name="currentUserService">Provides the current authenticated user's identifier.</param>
    /// <param name="permissionEvaluator">Evaluates the current user's authorization.</param>
    public RegisterUnitOfMeasurementCommandHandler(
        IUnitOfMeasurementRepository unitOfMeasurementRepository,
        IHoldingLookupService holdingLookupService,
        IConfigurationUnitOfWork unitOfWork,
        IDateTimeProvider dateTimeProvider,
        ICurrentUserService currentUserService,
        IPermissionEvaluator permissionEvaluator)
    {
        _unitOfMeasurementRepository = unitOfMeasurementRepository;
        _holdingLookupService = holdingLookupService;
        _unitOfWork = unitOfWork;
        _dateTimeProvider = dateTimeProvider;
        _currentUserService = currentUserService;
        _permissionEvaluator = permissionEvaluator;
    }

    /// <summary>Executes the registration use case.</summary>
    /// <param name="request">The command to handle.</param>
    /// <param name="cancellationToken">Token to cancel the asynchronous operation.</param>
    /// <returns>A <see cref="Result{Guid}"/> containing the new unit's identifier, or a validation/authorization error.</returns>
    public async Task<Result<Guid>> Handle(RegisterUnitOfMeasurementCommand request, CancellationToken cancellationToken)
    {
        if (_currentUserService.UserId is not { } userId)
            return Result.Failure<Guid>(global::Configuration.Domain.UnitOfMeasurementErrors.NotAuthorized());

        if (!await _holdingLookupService.ExistsAsync(request.HoldingId, cancellationToken))
            return Result.Failure<Guid>(Error.NotFound("Holding.NotFound", $"Holding with id {request.HoldingId} was not found."));

        var scope = new ResourceScope(request.HoldingId, null, null);
        var isAuthorized = await _permissionEvaluator.HasPermissionAsync(userId, RequiredPermission, scope, cancellationToken);

        if (!isAuthorized)
            return Result.Failure<Guid>(global::Configuration.Domain.UnitOfMeasurementErrors.NotAuthorized());

        var result = global::Configuration.Domain.UnitOfMeasurement.Register(
            request.HoldingId, request.Name, request.Kind, _dateTimeProvider);

        if (result.IsFailure)
            return Result.Failure<Guid>(result.Error);

        _unitOfMeasurementRepository.Add(result.Value);
        await _unitOfWork.SaveChangesAsync(cancellationToken);

        return Result.Success(result.Value.Id.Value);
    }
}