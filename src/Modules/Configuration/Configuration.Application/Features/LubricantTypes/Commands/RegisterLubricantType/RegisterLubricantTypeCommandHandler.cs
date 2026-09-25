using MachineryManagerEnterprise.Configuration.Application.Abstractions;
using MachineryManagerEnterprise.SharedKernel;
using MachineryManagerEnterprise.SharedKernel.Abstractions;
using MediatR;

namespace MachineryManagerEnterprise.Configuration.Application.Features.LubricantTypes.Commands.RegisterLubricantType;

/// <summary>Handles <see cref="RegisterLubricantTypeCommand"/>.</summary>
public sealed class RegisterLubricantTypeCommandHandler : IRequestHandler<RegisterLubricantTypeCommand, Result<Guid>>
{
    private const string RequiredPermission = "LubricantType.Create";

    private readonly ILubricantTypeRepository _lubricantTypeRepository;
    private readonly IHoldingLookupService _holdingLookupService;
    private readonly IConfigurationUnitOfWork _unitOfWork;
    private readonly IDateTimeProvider _dateTimeProvider;
    private readonly ICurrentUserService _currentUserService;
    private readonly IPermissionEvaluator _permissionEvaluator;

    /// <summary>Initializes a new instance of the <see cref="RegisterLubricantTypeCommandHandler"/> class.</summary>
    public RegisterLubricantTypeCommandHandler(
        ILubricantTypeRepository lubricantTypeRepository,
        IHoldingLookupService holdingLookupService,
        IConfigurationUnitOfWork unitOfWork,
        IDateTimeProvider dateTimeProvider,
        ICurrentUserService currentUserService,
        IPermissionEvaluator permissionEvaluator)
    {
        _lubricantTypeRepository = lubricantTypeRepository;
        _holdingLookupService = holdingLookupService;
        _unitOfWork = unitOfWork;
        _dateTimeProvider = dateTimeProvider;
        _currentUserService = currentUserService;
        _permissionEvaluator = permissionEvaluator;
    }

    /// <summary>Handles <see cref="RegisterLubricantTypeCommand"/>.</summary>
    public async Task<Result<Guid>> Handle(RegisterLubricantTypeCommand request, CancellationToken cancellationToken)
    {
        if (_currentUserService.UserId is not { } userId)
        {
            return Result.Failure<Guid>(global::Configuration.Domain.LubricantTypeErrors.NotAuthorized());
        }

        if (!await _holdingLookupService.ExistsAsync(request.HoldingId, cancellationToken))
        {
            return Result.Failure<Guid>(Error.NotFound("Holding.NotFound", $"Holding with id {request.HoldingId} was not found."));
        }

        var scope = new ResourceScope(request.HoldingId, null, null);
        var isAuthorized = await _permissionEvaluator.HasPermissionAsync(userId, RequiredPermission, scope, cancellationToken);

        if (!isAuthorized)
        {
            return Result.Failure<Guid>(global::Configuration.Domain.LubricantTypeErrors.NotAuthorized());
        }

        var result = global::Configuration.Domain.LubricantType.Register(request.HoldingId, request.Name, _dateTimeProvider);

        if (result.IsFailure)
        {
            return Result.Failure<Guid>(result.Error);
        }

        _lubricantTypeRepository.Add(result.Value);
        await _unitOfWork.SaveChangesAsync(cancellationToken);

        return Result.Success(result.Value.Id.Value);
    }
}
