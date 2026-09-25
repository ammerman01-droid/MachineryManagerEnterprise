using MachineryManagerEnterprise.Configuration.Application.Abstractions;
using MachineryManagerEnterprise.SharedKernel;
using MachineryManagerEnterprise.SharedKernel.Abstractions;
using MediatR;

namespace MachineryManagerEnterprise.Configuration.Application.Features.LubricantTypes.Commands.RenameLubricantType;

/// <summary>Handles <see cref="RenameLubricantTypeCommand"/>.</summary>
public sealed class RenameLubricantTypeCommandHandler : IRequestHandler<RenameLubricantTypeCommand, Result>
{
    private const string RequiredPermission = "LubricantType.Edit";

    private readonly ILubricantTypeRepository _lubricantTypeRepository;
    private readonly IConfigurationUnitOfWork _unitOfWork;
    private readonly IDateTimeProvider _dateTimeProvider;
    private readonly ICurrentUserService _currentUserService;
    private readonly IPermissionEvaluator _permissionEvaluator;

    /// <summary>Initializes a new instance of the <see cref="RenameLubricantTypeCommandHandler"/> class.</summary>
    public RenameLubricantTypeCommandHandler(
        ILubricantTypeRepository lubricantTypeRepository,
        IConfigurationUnitOfWork unitOfWork,
        IDateTimeProvider dateTimeProvider,
        ICurrentUserService currentUserService,
        IPermissionEvaluator permissionEvaluator)
    {
        _lubricantTypeRepository = lubricantTypeRepository;
        _unitOfWork = unitOfWork;
        _dateTimeProvider = dateTimeProvider;
        _currentUserService = currentUserService;
        _permissionEvaluator = permissionEvaluator;
    }

    /// <summary>Handles <see cref="RenameLubricantTypeCommand"/>.</summary>
    public async Task<Result> Handle(RenameLubricantTypeCommand request, CancellationToken cancellationToken)
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

        var renameResult = entity.Rename(request.Name, _dateTimeProvider);

        if (renameResult.IsFailure)
        {
            return renameResult;
        }

        _lubricantTypeRepository.Update(entity);
        await _unitOfWork.SaveChangesAsync(cancellationToken);

        return Result.Success();
    }
}
