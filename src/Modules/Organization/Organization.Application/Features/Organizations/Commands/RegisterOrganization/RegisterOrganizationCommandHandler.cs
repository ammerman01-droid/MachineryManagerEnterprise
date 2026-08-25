using MachineryManager.SharedKernel;
using MachineryManager.SharedKernel.Abstractions;
using MediatR;
using Organization.Domain;
using MachineryManager.Organization.Application.Abstractions;

namespace MachineryManager.Organization.Application.Features.Organizations.Commands.RegisterOrganization;

/// <summary>Handles <see cref="RegisterOrganizationCommand"/>.</summary>
public sealed class RegisterOrganizationCommandHandler
    : IRequestHandler<RegisterOrganizationCommand, Result<OrganizationId>>
{
    // Aligned with the permission matrix's naming convention
    // (Section.Action) — chat, 2026-08-23.
    private const string RequiredPermission = "Organization.Create";

    private readonly IOrganizationRepository _repository;
    private readonly IUnitOfWork _unitOfWork;
    private readonly IDateTimeProvider _dateTimeProvider;
    private readonly ICurrentUserService _currentUserService;
    private readonly IPermissionEvaluator _permissionEvaluator;

    /// <summary>Initializes a new instance of the <see cref="RegisterOrganizationCommandHandler"/> class.</summary>
    public RegisterOrganizationCommandHandler(
        IOrganizationRepository repository,
        IUnitOfWork unitOfWork,
        IDateTimeProvider dateTimeProvider,
        ICurrentUserService currentUserService,
        IPermissionEvaluator permissionEvaluator)
    {
        _repository = repository;
        _unitOfWork = unitOfWork;
        _dateTimeProvider = dateTimeProvider;
        _currentUserService = currentUserService;
        _permissionEvaluator = permissionEvaluator;
    }

    /// <inheritdoc />
    public async Task<Result<OrganizationId>> Handle(RegisterOrganizationCommand request, CancellationToken cancellationToken)
    {
        if (_currentUserService.UserId is not { } userId)
        {
            return Result.Failure<OrganizationId>(OrganizationErrors.NotAuthorized());
        }

        // Registering a new Organization is a platform/Holding-level
        // action (it has no OrganizationId of its own yet) — checked
        // against ResourceScope.PlatformWide (chat, 2026-08-22).
        var isAuthorized = await _permissionEvaluator.HasPermissionAsync(
            userId,
            RequiredPermission,
            ResourceScope.PlatformWide,
            cancellationToken);

        if (!isAuthorized)
        {
            return Result.Failure<OrganizationId>(OrganizationErrors.NotAuthorized());
        }

        var organizationResult = global::Organization.Domain.Organization.Register(request.Name, _dateTimeProvider);

        if (organizationResult.IsFailure)
        {
            return Result.Failure<OrganizationId>(organizationResult.Error);
        }

        var organization = organizationResult.Value;

        _repository.Add(organization);
        await _unitOfWork.SaveChangesAsync(cancellationToken);

        return organization.Id;
    }
}