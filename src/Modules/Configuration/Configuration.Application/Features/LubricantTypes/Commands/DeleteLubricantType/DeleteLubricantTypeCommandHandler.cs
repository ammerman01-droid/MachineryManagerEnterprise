using MachineryManagerEnterprise.Configuration.Application.Abstractions;
using MachineryManagerEnterprise.SharedKernel;
using MachineryManagerEnterprise.SharedKernel.Abstractions;
using MediatR;

namespace MachineryManagerEnterprise.Configuration.Application.Features.LubricantTypes.Commands.DeleteLubricantType;

/// <summary>
/// Handles <see cref="DeleteLubricantTypeCommand"/>. Deletion is blocked
/// (Conflict) if any Lubricant Overflow Report line currently references
/// this Lubricant Type, checked via the cross-module
/// <see cref="IConsumptionUsageLookupService"/>.
/// </summary>
public sealed class DeleteLubricantTypeCommandHandler : IRequestHandler<DeleteLubricantTypeCommand, Result>
{
    private const string RequiredPermission = "LubricantType.Delete";

    private readonly ILubricantTypeRepository _lubricantTypeRepository;
    private readonly IConsumptionUsageLookupService _consumptionUsageLookupService;
    private readonly IConfigurationUnitOfWork _unitOfWork;
    private readonly ICurrentUserService _currentUserService;
    private readonly IPermissionEvaluator _permissionEvaluator;

    /// <summary>Initializes a new instance of the <see cref="DeleteLubricantTypeCommandHandler"/> class.</summary>
    public DeleteLubricantTypeCommandHandler(
        ILubricantTypeRepository lubricantTypeRepository,
        IConsumptionUsageLookupService consumptionUsageLookupService,
        IConfigurationUnitOfWork unitOfWork,
        ICurrentUserService currentUserService,
        IPermissionEvaluator permissionEvaluator)
    {
        _lubricantTypeRepository = lubricantTypeRepository;
        _consumptionUsageLookupService = consumptionUsageLookupService;
        _unitOfWork = unitOfWork;
        _currentUserService = currentUserService;
        _permissionEvaluator = permissionEvaluator;
    }

    /// <summary>Handles <see cref="DeleteLubricantTypeCommand"/>.</summary>
    public async Task<Result> Handle(DeleteLubricantTypeCommand request, CancellationToken cancellationToken)
    {
        if (_currentUserService.UserId is not { } userId)
        {
            return Result.Failure(global::Configuration.Domain.LubricantTypeErrors.NotAuthorized());
        }

        var entity = await _lubricantTypeRepository.GetByIdAsync(
            global::Configuration.Domain.LubricantTypeId.From(request.LubricantTypeId), cancellationToken);

        if (entity is null)
        {
            return Result.Failure(Error.NotFound("LubricantType.NotFound", $"Lubricant Type with id {request.LubricantTypeId} was not found."));
        }

        var scope = new ResourceScope(entity.HoldingId, null, null);
        var isAuthorized = await _permissionEvaluator.HasPermissionAsync(userId, RequiredPermission, scope, cancellationToken);

        if (!isAuthorized)
        {
            return Result.Failure(global::Configuration.Domain.LubricantTypeErrors.NotAuthorized());
        }

        if (await _consumptionUsageLookupService.IsLubricantTypeInUseAsync(request.LubricantTypeId, cancellationToken))
        {
            return Result.Failure(global::Configuration.Domain.LubricantTypeErrors.InUse());
        }

        _lubricantTypeRepository.Remove(entity);
        await _unitOfWork.SaveChangesAsync(cancellationToken);

        return Result.Success();
    }
}
