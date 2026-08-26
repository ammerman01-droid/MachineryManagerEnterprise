using MachineryManager.Asset.Application.Abstractions;
using MachineryManager.SharedKernel;
using MachineryManager.SharedKernel.Abstractions;
using MediatR;

namespace MachineryManager.Asset.Application.Features.EngineModels.Commands.RenameEngineModel;

/// <summary>
/// Handles <see cref="RenameEngineModelCommand"/> by loading the
/// aggregate, invoking the domain rename behavior, and committing the
/// unit of work.
/// </summary>
public sealed class RenameEngineModelCommandHandler
    : IRequestHandler<RenameEngineModelCommand, Result>
{
    private const string RequiredPermission = "Asset.Edit";

    private readonly IEngineModelRepository _engineModelRepository;
    private readonly IUnitOfWork _unitOfWork;
    private readonly ICurrentUserService _currentUserService;
    private readonly IPermissionEvaluator _permissionEvaluator;
    private readonly IOrganizationLookupService _organizationLookupService;

    /// <summary>Initializes a new instance of the <see cref="RenameEngineModelCommandHandler"/> class.</summary>
    public RenameEngineModelCommandHandler(
        IEngineModelRepository engineModelRepository,
        IUnitOfWork unitOfWork,
        ICurrentUserService currentUserService,
        IPermissionEvaluator permissionEvaluator,
        IOrganizationLookupService organizationLookupService)
    {
        _engineModelRepository = engineModelRepository;
        _unitOfWork = unitOfWork;
        _currentUserService = currentUserService;
        _permissionEvaluator = permissionEvaluator;
        _organizationLookupService = organizationLookupService;
    }

    /// <summary>Executes the rename use case.</summary>
    public async Task<Result> Handle(RenameEngineModelCommand request, CancellationToken cancellationToken)
    {
        var id = global::Asset.Domain.EngineModelId.From(request.EngineModelId);
        var engineModel = await _engineModelRepository.GetByIdAsync(id, cancellationToken);

        if (engineModel is null)
        {
            return Result.Failure(
                Error.NotFound("EngineModel.NotFound", $"Engine model with id {request.EngineModelId} was not found."));
        }

        if (_currentUserService.UserId is not { } userId)
        {
            return Result.Failure(global::Asset.Domain.EngineModelErrors.NotAuthorized());
        }

        var holdingId = await _organizationLookupService.GetHoldingIdAsync(engineModel.OrganizationId, cancellationToken);

        var isAuthorized = await _permissionEvaluator.HasPermissionAsync(
            userId,
            RequiredPermission,
            new ResourceScope(holdingId, engineModel.OrganizationId, null),
            cancellationToken);

        if (!isAuthorized)
        {
            return Result.Failure(global::Asset.Domain.EngineModelErrors.NotAuthorized());
        }

        var result = engineModel.Rename(request.Name);

        if (result.IsFailure)
        {
            return result;
        }

        _engineModelRepository.Update(engineModel);
        await _unitOfWork.SaveChangesAsync(cancellationToken);

        return Result.Success();
    }
}