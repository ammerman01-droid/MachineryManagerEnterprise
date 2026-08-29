using MachineryManager.Asset.Application.Abstractions;
using MachineryManager.SharedKernel;
using MachineryManager.SharedKernel.Abstractions;
using MediatR;

namespace MachineryManager.Asset.Application.Features.Colors.Commands.RegisterColor;

/// <summary>
/// Handles <see cref="RegisterColorCommand"/> by verifying the caller
/// holds either Asset.Create or Asset.Edit (chat, 2026-08-28 — any user
/// who can work with Assets may extend the Organization's color list),
/// then invoking domain registration.
/// </summary>
public sealed class RegisterColorCommandHandler
    : IRequestHandler<RegisterColorCommand, Result<Guid>>
{
    private readonly IColorRepository _colorRepository;
    private readonly IAssetUnitOfWork _unitOfWork;
    private readonly IDateTimeProvider _dateTimeProvider;
    private readonly ICurrentUserService _currentUserService;
    private readonly IPermissionEvaluator _permissionEvaluator;
    private readonly IOrganizationLookupService _organizationLookupService;

    /// <summary>Initializes a new instance of the <see cref="RegisterColorCommandHandler"/> class.</summary>
    public RegisterColorCommandHandler(
        IColorRepository colorRepository,
        IAssetUnitOfWork unitOfWork,
        IDateTimeProvider dateTimeProvider,
        ICurrentUserService currentUserService,
        IPermissionEvaluator permissionEvaluator,
        IOrganizationLookupService organizationLookupService)
    {
        _colorRepository = colorRepository;
        _unitOfWork = unitOfWork;
        _dateTimeProvider = dateTimeProvider;
        _currentUserService = currentUserService;
        _permissionEvaluator = permissionEvaluator;
        _organizationLookupService = organizationLookupService;
    }

    /// <summary>Executes the registration use case.</summary>
    public async Task<Result<Guid>> Handle(RegisterColorCommand request, CancellationToken cancellationToken)
    {
        if (_currentUserService.UserId is not { } userId)
        {
            return Result.Failure<Guid>(global::Asset.Domain.ColorErrors.NotAuthorized());
        }

        var holdingId = await _organizationLookupService.GetHoldingIdAsync(request.OrganizationId, cancellationToken);
        var scope = new ResourceScope(holdingId, request.OrganizationId, null);

        var canCreate = await _permissionEvaluator.HasPermissionAsync(userId, "Asset.Create", scope, cancellationToken);
        var canEdit = canCreate || await _permissionEvaluator.HasPermissionAsync(userId, "Asset.Edit", scope, cancellationToken);

        if (!canEdit)
        {
            return Result.Failure<Guid>(global::Asset.Domain.ColorErrors.NotAuthorized());
        }

        var result = global::Asset.Domain.Color.Register(request.OrganizationId, request.Name, _dateTimeProvider);

        if (result.IsFailure)
        {
            return Result.Failure<Guid>(result.Error);
        }

        _colorRepository.Add(result.Value);
        await _unitOfWork.SaveChangesAsync(cancellationToken);

        return Result.Success(result.Value.Id.Value);
    }
}