using MachineryManagerEnterprise.Configuration.Application.Abstractions;
using MachineryManagerEnterprise.SharedKernel;
using MachineryManagerEnterprise.SharedKernel.Abstractions;
using MediatR;

namespace MachineryManagerEnterprise.Configuration.Application.Features.AssetOperationalStatuses.Commands.RegisterAssetOperationalStatus;

/// <summary>Handles <see cref="RegisterAssetOperationalStatusCommand"/>.</summary>
public sealed class RegisterAssetOperationalStatusCommandHandler
    : IRequestHandler<RegisterAssetOperationalStatusCommand, Result<Guid>>
{
    private const string RequiredPermission = "AssetOperationalStatus.Create";

    private readonly IAssetOperationalStatusRepository _repository;
    private readonly IHoldingLookupService _holdingLookupService;
    private readonly IConfigurationUnitOfWork _unitOfWork;
    private readonly IDateTimeProvider _dateTimeProvider;
    private readonly ICurrentUserService _currentUserService;
    private readonly IPermissionEvaluator _permissionEvaluator;

    /// <summary>Initializes a new instance of the <see cref="RegisterAssetOperationalStatusCommandHandler"/> class.</summary>
    public RegisterAssetOperationalStatusCommandHandler(
        IAssetOperationalStatusRepository repository,
        IHoldingLookupService holdingLookupService,
        IConfigurationUnitOfWork unitOfWork,
        IDateTimeProvider dateTimeProvider,
        ICurrentUserService currentUserService,
        IPermissionEvaluator permissionEvaluator)
    {
        _repository = repository;
        _holdingLookupService = holdingLookupService;
        _unitOfWork = unitOfWork;
        _dateTimeProvider = dateTimeProvider;
        _currentUserService = currentUserService;
        _permissionEvaluator = permissionEvaluator;
    }

    /// <summary>Executes the registration use case.</summary>
    public async Task<Result<Guid>> Handle(RegisterAssetOperationalStatusCommand request, CancellationToken cancellationToken)
    {
        if (_currentUserService.UserId is not { } userId)
        {
            return Result.Failure<Guid>(global::Configuration.Domain.AssetOperationalStatusErrors.NotAuthorized());
        }

        if (!await _holdingLookupService.ExistsAsync(request.HoldingId, cancellationToken))
        {
            return Result.Failure<Guid>(Error.NotFound("Holding.NotFound", $"Holding with id {request.HoldingId} was not found."));
        }

        var scope = new ResourceScope(request.HoldingId, null, null);
        var isAuthorized = await _permissionEvaluator.HasPermissionAsync(userId, RequiredPermission, scope, cancellationToken);

        if (!isAuthorized)
        {
            return Result.Failure<Guid>(global::Configuration.Domain.AssetOperationalStatusErrors.NotAuthorized());
        }

        var result = global::Configuration.Domain.AssetOperationalStatus.Register(request.HoldingId, request.Name, _dateTimeProvider);

        if (result.IsFailure)
        {
            return Result.Failure<Guid>(result.Error);
        }

        _repository.Add(result.Value);
        await _unitOfWork.SaveChangesAsync(cancellationToken);

        return Result.Success(result.Value.Id.Value);
    }
}