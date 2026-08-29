using MachineryManager.Asset.Application.Abstractions;
using MachineryManager.Asset.Application.Features.Colors.Dtos;
using MachineryManager.SharedKernel;
using MachineryManager.SharedKernel.Abstractions;
using MediatR;

namespace MachineryManager.Asset.Application.Features.Colors.Queries.GetColorsByOrganization;

/// <summary>
/// Handles <see cref="GetColorsByOrganizationQuery"/> by verifying the
/// caller holds Asset.View for the Organization, then returning the
/// Organization's full color list.
/// </summary>
public sealed class GetColorsByOrganizationQueryHandler
    : IRequestHandler<GetColorsByOrganizationQuery, Result<IReadOnlyList<ColorDto>>>
{
    private const string RequiredPermission = "Color.View";
    private readonly IColorRepository _colorRepository;
    private readonly ICurrentUserService _currentUserService;
    private readonly IPermissionEvaluator _permissionEvaluator;
    private readonly IOrganizationLookupService _organizationLookupService;

    /// <summary>Initializes a new instance of the <see cref="GetColorsByOrganizationQueryHandler"/> class.</summary>
    public GetColorsByOrganizationQueryHandler(
        IColorRepository colorRepository,
        ICurrentUserService currentUserService,
        IPermissionEvaluator permissionEvaluator,
        IOrganizationLookupService organizationLookupService)
    {
        _colorRepository = colorRepository;
        _currentUserService = currentUserService;
        _permissionEvaluator = permissionEvaluator;
        _organizationLookupService = organizationLookupService;
    }

    /// <summary>Executes the lookup use case.</summary>
    public async Task<Result<IReadOnlyList<ColorDto>>> Handle(
        GetColorsByOrganizationQuery request,
        CancellationToken cancellationToken)
    {
        if (_currentUserService.UserId is not { } userId)
        {
            return Result.Failure<IReadOnlyList<ColorDto>>(global::Asset.Domain.ColorErrors.NotAuthorized());
        }

        var holdingId = await _organizationLookupService.GetHoldingIdAsync(request.OrganizationId, cancellationToken);

        var isAuthorized = await _permissionEvaluator.HasPermissionAsync(
            userId,
            RequiredPermission,
            new ResourceScope(holdingId, request.OrganizationId, null),
            cancellationToken);

        if (!isAuthorized)
        {
            return Result.Failure<IReadOnlyList<ColorDto>>(global::Asset.Domain.ColorErrors.NotAuthorized());
        }

        var colors = await _colorRepository.GetByOrganizationIdAsync(request.OrganizationId, cancellationToken);

        return Result.Success(colors);
    }
}