using MachineryManagerEnterprise.Personnel.Application.Abstractions;
using MachineryManagerEnterprise.Personnel.Application.Features.Dtos;
using MachineryManagerEnterprise.SharedKernel;
using MachineryManagerEnterprise.SharedKernel.Abstractions;
using MediatR;

namespace MachineryManagerEnterprise.Personnel.Application.Features.Queries.GetPersonnelByOrganization;

/// <summary>Handles <see cref="GetPersonnelByOrganizationQuery"/>.</summary>
public sealed class GetPersonnelByOrganizationQueryHandler : IRequestHandler<GetPersonnelByOrganizationQuery, Result<IReadOnlyList<PersonnelDto>>>
{
    private const string RequiredPermission = "Personnel.View";

    private readonly IPersonnelRepository _personnelRepository;
    private readonly ICurrentUserService _currentUserService;
    private readonly IPermissionEvaluator _permissionEvaluator;

    /// <summary>Initializes a new instance of the <see cref="GetPersonnelByOrganizationQueryHandler"/> class.</summary>
    public GetPersonnelByOrganizationQueryHandler(
        IPersonnelRepository personnelRepository,
        ICurrentUserService currentUserService,
        IPermissionEvaluator permissionEvaluator)
    {
        _personnelRepository = personnelRepository;
        _currentUserService = currentUserService;
        _permissionEvaluator = permissionEvaluator;
    }

    /// <summary>Handles <see cref="GetPersonnelByOrganizationQuery"/>.</summary>
    public async Task<Result<IReadOnlyList<PersonnelDto>>> Handle(GetPersonnelByOrganizationQuery request, CancellationToken cancellationToken)
    {
        if (_currentUserService.UserId is not { } userId)
        {
            return Result.Failure<IReadOnlyList<PersonnelDto>>(global::MachineryManagerEnterprise.Personnel.Domain.PersonnelErrors.NotAuthorized());
        }

        var scope = new ResourceScope(null, request.OrganizationId, null);
        var isAuthorized = await _permissionEvaluator.HasPermissionAsync(userId, RequiredPermission, scope, cancellationToken);

        if (!isAuthorized)
        {
            return Result.Failure<IReadOnlyList<PersonnelDto>>(global::MachineryManagerEnterprise.Personnel.Domain.PersonnelErrors.NotAuthorized());
        }

        var personnel = await _personnelRepository.GetByOrganizationAsync(request.OrganizationId, cancellationToken);

        return Result.Success(personnel);
    }
}