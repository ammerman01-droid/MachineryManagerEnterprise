using MachineryManagerEnterprise.Configuration.Application.Abstractions;
using MachineryManagerEnterprise.SharedKernel;
using MachineryManagerEnterprise.SharedKernel.Abstractions;
using MediatR;

namespace MachineryManagerEnterprise.Configuration.Application.Features.OverflowComponents.Commands.RenameOverflowComponent;

/// <summary>Handles <see cref="RenameOverflowComponentCommand"/>.</summary>
public sealed class RenameOverflowComponentCommandHandler : IRequestHandler<RenameOverflowComponentCommand, Result>
{
    private const string RequiredPermission = "OverflowComponent.Edit";

    private readonly IOverflowComponentRepository _overflowComponentRepository;
    private readonly IConfigurationUnitOfWork _unitOfWork;
    private readonly IDateTimeProvider _dateTimeProvider;
    private readonly ICurrentUserService _currentUserService;
    private readonly IPermissionEvaluator _permissionEvaluator;

    /// <summary>Initializes a new instance of the <see cref="RenameOverflowComponentCommandHandler"/> class.</summary>
    public RenameOverflowComponentCommandHandler(
        IOverflowComponentRepository overflowComponentRepository,
        IConfigurationUnitOfWork unitOfWork,
        IDateTimeProvider dateTimeProvider,
        ICurrentUserService currentUserService,
        IPermissionEvaluator permissionEvaluator)
    {
        _overflowComponentRepository = overflowComponentRepository;
        _unitOfWork = unitOfWork;
        _dateTimeProvider = dateTimeProvider;
        _currentUserService = currentUserService;
        _permissionEvaluator = permissionEvaluator;
    }

    /// <summary>Handles <see cref="RenameOverflowComponentCommand"/>.</summary>
    public async Task<Result> Handle(RenameOverflowComponentCommand request, CancellationToken cancellationToken)
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

        var renameResult = entity.Rename(request.Name, _dateTimeProvider);

        if (renameResult.IsFailure)
        {
            return renameResult;
        }

        _overflowComponentRepository.Update(entity);
        await _unitOfWork.SaveChangesAsync(cancellationToken);

        return Result.Success();
    }
}
