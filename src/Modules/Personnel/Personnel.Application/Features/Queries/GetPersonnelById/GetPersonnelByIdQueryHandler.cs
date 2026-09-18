using MachineryManagerEnterprise.Personnel.Application.Abstractions;
using MachineryManagerEnterprise.Personnel.Application.Features.Dtos;
using MachineryManagerEnterprise.SharedKernel;
using MachineryManagerEnterprise.SharedKernel.Abstractions;
using Mapster;
using MediatR;

namespace MachineryManagerEnterprise.Personnel.Application.Features.Queries.GetPersonnelById;

/// <summary>Handles <see cref="GetPersonnelByIdQuery"/>.</summary>
public sealed class GetPersonnelByIdQueryHandler : IRequestHandler<GetPersonnelByIdQuery, Result<PersonnelDto>>
{
    private const string RequiredPermission = "Personnel.View";

    private readonly IPersonnelRepository _personnelRepository;
    private readonly IOrganizationLookupService _organizationLookupService;
    private readonly ICurrentUserService _currentUserService;
    private readonly IPermissionEvaluator _permissionEvaluator;

    /// <summary>Initializes a new instance of the <see cref="GetPersonnelByIdQueryHandler"/> class.</summary>
    public GetPersonnelByIdQueryHandler(
        IPersonnelRepository personnelRepository,
        IOrganizationLookupService organizationLookupService,
        ICurrentUserService currentUserService,
        IPermissionEvaluator permissionEvaluator)
    {
        _personnelRepository = personnelRepository;
        _organizationLookupService = organizationLookupService;
        _currentUserService = currentUserService;
        _permissionEvaluator = permissionEvaluator;
    }

    /// <summary>Handles <see cref="GetPersonnelByIdQuery"/>.</summary>
    public async Task<Result<PersonnelDto>> Handle(GetPersonnelByIdQuery request, CancellationToken cancellationToken)
    {
        if (_currentUserService.UserId is not { } userId)
        {
            return Result.Failure<PersonnelDto>(global::MachineryManagerEnterprise.Personnel.Domain.PersonnelErrors.NotAuthorized());
        }

        var entity = await _personnelRepository.GetByIdAsync(
            global::MachineryManagerEnterprise.Personnel.Domain.PersonnelId.From(request.PersonnelId), cancellationToken);

        if (entity is null)
        {
            return Result.Failure<PersonnelDto>(global::MachineryManagerEnterprise.Personnel.Domain.PersonnelErrors.NotFound());
        }

        var holdingId = await _organizationLookupService.GetHoldingIdAsync(entity.OrganizationId, cancellationToken);
        var scope = new ResourceScope(holdingId, entity.OrganizationId, entity.CurrentProjectId);
        var isAuthorized = await _permissionEvaluator.HasPermissionAsync(userId, RequiredPermission, scope, cancellationToken);

        if (!isAuthorized)
        {
            return Result.Failure<PersonnelDto>(global::MachineryManagerEnterprise.Personnel.Domain.PersonnelErrors.NotAuthorized());
        }

        return Result.Success(entity.Adapt<PersonnelDto>());
    }
}