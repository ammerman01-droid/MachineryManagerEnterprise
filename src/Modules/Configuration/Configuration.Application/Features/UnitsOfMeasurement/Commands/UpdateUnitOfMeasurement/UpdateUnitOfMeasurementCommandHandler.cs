using MachineryManagerEnterprise.Configuration.Application.Abstractions;
using MachineryManagerEnterprise.SharedKernel;
using MachineryManagerEnterprise.SharedKernel.Abstractions;
using MediatR;

namespace MachineryManagerEnterprise.Configuration.Application.Features.UnitsOfMeasurement.Commands.UpdateUnitOfMeasurement;

/// <summary>Handles <see cref="UpdateUnitOfMeasurementCommand"/>.</summary>
public sealed class UpdateUnitOfMeasurementCommandHandler : IRequestHandler<UpdateUnitOfMeasurementCommand, Result>
{
    // There is no dedicated "UnitOfMeasurement.Edit" permission in the
    // permission catalog yet, so editing reuses the Create permission
    // (chat, 2026-09-19). Switch to a dedicated permission once one is
    // added to the catalog and granted to the relevant profiles.
    private const string RequiredPermission = "UnitOfMeasurement.Create";

    private readonly IUnitOfMeasurementRepository _unitOfMeasurementRepository;
    private readonly IConfigurationUnitOfWork _unitOfWork;
    private readonly ICurrentUserService _currentUserService;
    private readonly IPermissionEvaluator _permissionEvaluator;

    /// <summary>Initializes a new instance of the <see cref="UpdateUnitOfMeasurementCommandHandler"/> class.</summary>
    /// <param name="unitOfMeasurementRepository">The Unit of Measurement repository.</param>
    /// <param name="unitOfWork">The Configuration module's unit of work.</param>
    /// <param name="currentUserService">Provides the current authenticated user's identifier.</param>
    /// <param name="permissionEvaluator">Evaluates the current user's authorization.</param>
    public UpdateUnitOfMeasurementCommandHandler(
        IUnitOfMeasurementRepository unitOfMeasurementRepository,
        IConfigurationUnitOfWork unitOfWork,
        ICurrentUserService currentUserService,
        IPermissionEvaluator permissionEvaluator)
    {
        _unitOfMeasurementRepository = unitOfMeasurementRepository;
        _unitOfWork = unitOfWork;
        _currentUserService = currentUserService;
        _permissionEvaluator = permissionEvaluator;
    }

    /// <summary>Executes the update use case.</summary>
    /// <param name="request">The command to handle.</param>
    /// <param name="cancellationToken">Token to cancel the asynchronous operation.</param>
    /// <returns>A <see cref="Result"/> indicating success, or a not-found, validation, or authorization error.</returns>
    public async Task<Result> Handle(UpdateUnitOfMeasurementCommand request, CancellationToken cancellationToken)
    {
        if (_currentUserService.UserId is not { } userId)
            return Result.Failure(global::Configuration.Domain.UnitOfMeasurementErrors.NotAuthorized());

        var unit = await _unitOfMeasurementRepository.GetByIdAsync(
            global::Configuration.Domain.UnitOfMeasurementId.From(request.UnitOfMeasurementId), cancellationToken);

        if (unit is null)
            return Result.Failure(global::Configuration.Domain.UnitOfMeasurementErrors.NotFound(request.UnitOfMeasurementId));

        var scope = new ResourceScope(unit.HoldingId, null, null);
        var isAuthorized = await _permissionEvaluator.HasPermissionAsync(userId, RequiredPermission, scope, cancellationToken);

        if (!isAuthorized)
            return Result.Failure(global::Configuration.Domain.UnitOfMeasurementErrors.NotAuthorized());

        var result = unit.Update(request.Name, request.Kind);

        if (result.IsFailure)
            return result;

        _unitOfMeasurementRepository.Update(unit);
        await _unitOfWork.SaveChangesAsync(cancellationToken);

        return Result.Success();
    }
}
