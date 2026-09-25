using MachineryManagerEnterprise.Configuration.Application.Abstractions;
using MachineryManagerEnterprise.SharedKernel;
using MachineryManagerEnterprise.SharedKernel.Abstractions;
using MediatR;

namespace MachineryManagerEnterprise.Configuration.Application.Features.OverflowComponents.Commands.DeleteOverflowComponent;

/// <summary>
/// Handles <see cref="DeleteOverflowComponentCommand"/>. Deletion is blocked
/// (Conflict) if any Lubricant Overflow Report line currently references
/// this Overflow Component, checked via the cross-module
/// <see cref="IConsumptionUsageLookupService"/>.
/// </summary>
public sealed class DeleteOverflowComponentCommandHandler : IRequestHandler<DeleteOverflowComponentCommand, Result>
{
    private const string RequiredPermission = "OverflowComponent.Delete";

    private readonly IOverflowComponentRepository _overflowComponentRepository;
    private readonly IConsumptionUsageLookupService _consumptionUsageLookupService;
    private readonly IConfigurationUnitOfWork _unitOfWork;
    private readonly ICurrentUserService _currentUserService;
    private readonly IPermissionEvaluator _permissionEvaluator;

    /// <summary>Initializes a new instance of the <see cref="DeleteOverflowComponentCommandHandler"/> class.</summary>
    public DeleteOverflowComponentCommandHandler(
        IOverflowComponentRepository overflowComponentRepository,
        IConsumptionUsageLookupService consumptionUsageLookupService,
        IConfigurationUnitOfWork unitOfWork,
        ICurrentUserService currentUserService,
        IPermissionEvaluator permissionEvaluator)
    {
        _overflowComponentRepository = overflowComponentRepository;
        _consumptionUsageLookupService = consumptionUsageLookupService;
        _unitOfWork = unitOfWork;
        _currentUserService = currentUserService;
        _permissionEvaluator = permissionEvaluator;
    }

    /// <summary>Handles <see cref="DeleteOverflowComponentCommand"/>.</summary>
    public async Task<Result> Handle(DeleteOverflowComponentCommand request, CancellationToken cancellationToken)
    {
        if (_currentUserService.UserId is not { } userId)
        {
            return Result.Failure(global::Configuration.Domain.OverflowComponentErrors.NotAuthorized());
        }

        var entity = await _overflowComponentRepository.GetByIdAsync(
            global::Configuration.Domain.OverflowComponentId.From(request.OverflowComponentId), cancellationToken);

        if (entity is null)
        {
            return Result.Failure(Error.NotFound("OverflowComponent.NotFound", $"Overflow Component with id {request.OverflowComponentId} was not found."));
        }

        var scope = new ResourceScope(entity.HoldingId, null, null);
        var isAuthorized = await _permissionEvaluator.HasPermissionAsync(userId, RequiredPermission, scope, cancellationToken);

        if (!isAuthorized)
        {
            return Result.Failure(global::Configuration.Domain.OverflowComponentErrors.NotAuthorized());
        }

        if (await _consumptionUsageLookupService.IsOverflowComponentInUseAsync(request.OverflowComponentId, cancellationToken))
        {
            return Result.Failure(global::Configuration.Domain.OverflowComponentErrors.InUse());
        }

        _overflowComponentRepository.Remove(entity);
        await _unitOfWork.SaveChangesAsync(cancellationToken);

        return Result.Success();
    }
}
