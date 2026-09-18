using MachineryManagerEnterprise.Personnel.Application.Abstractions;
using MachineryManagerEnterprise.SharedKernel;
using MachineryManagerEnterprise.SharedKernel.Abstractions;
using MediatR;

namespace MachineryManagerEnterprise.Personnel.Application.Features.Commands.DeletePersonnel;

/// <summary>
/// Handles <see cref="DeletePersonnelCommand"/>. No cross-module usage
/// guard currently applies (flagged gap — no other module yet
/// references a Personnel record; add one here once Usage/Maintenance
/// modules do).
/// </summary>
public sealed class DeletePersonnelCommandHandler : IRequestHandler<DeletePersonnelCommand, Result>
{
    private const string RequiredPermission = "Personnel.Delete";

    private readonly IPersonnelRepository _personnelRepository;
    private readonly IOrganizationLookupService _organizationLookupService;
    private readonly IPersonnelUnitOfWork _unitOfWork;
    private readonly ICurrentUserService _currentUserService;
    private readonly IPermissionEvaluator _permissionEvaluator;

    /// <summary>Initializes a new instance of the <see cref="DeletePersonnelCommandHandler"/> class.</summary>
    public DeletePersonnelCommandHandler(
        IPersonnelRepository personnelRepository,
        IOrganizationLookupService organizationLookupService,
        IPersonnelUnitOfWork unitOfWork,
        ICurrentUserService currentUserService,
        IPermissionEvaluator permissionEvaluator)
    {
        _personnelRepository = personnelRepository;
        _organizationLookupService = organizationLookupService;
        _unitOfWork = unitOfWork;
        _currentUserService = currentUserService;
        _permissionEvaluator = permissionEvaluator;
    }

    /// <summary>Handles <see cref="DeletePersonnelCommand"/>.</summary>
    public async Task<Result> Handle(DeletePersonnelCommand request, CancellationToken cancellationToken)
    {
        if (_currentUserService.UserId is not { } userId)
        {
            return Result.Failure(global::MachineryManagerEnterprise.Personnel.Domain.PersonnelErrors.NotAuthorized());
        }

        var entity = await _personnelRepository.GetByIdAsync(
            global::MachineryManagerEnterprise.Personnel.Domain.PersonnelId.From(request.PersonnelId), cancellationToken);

        if (entity is null)
        {
            return Result.Failure(global::MachineryManagerEnterprise.Personnel.Domain.PersonnelErrors.NotFound());
        }

        var holdingId = await _organizationLookupService.GetHoldingIdAsync(entity.OrganizationId, cancellationToken);
        var scope = new ResourceScope(holdingId, entity.OrganizationId, entity.CurrentProjectId);
        var isAuthorized = await _permissionEvaluator.HasPermissionAsync(userId, RequiredPermission, scope, cancellationToken);

        if (!isAuthorized)
        {
            return Result.Failure(global::MachineryManagerEnterprise.Personnel.Domain.PersonnelErrors.NotAuthorized());
        }

        _personnelRepository.Remove(entity);
        await _unitOfWork.SaveChangesAsync(cancellationToken);

        return Result.Success();
    }
}