using MachineryManagerEnterprise.Administration.Application.Abstractions;
using MachineryManagerEnterprise.SharedKernel;
using MachineryManagerEnterprise.SharedKernel.Abstractions;
using MediatR;

namespace MachineryManagerEnterprise.Administration.Application.Features.Profiles.Commands.CreateProfile;

/// <summary>
/// Handles <see cref="CreateProfileCommand"/>: only Platform users (Platform-owned
/// profiles) and the owning Holding's users (Holding-owned profiles) may create profiles,
/// and only with permissions they hold themselves.
/// </summary>
public sealed class CreateProfileCommandHandler
    : IRequestHandler<CreateProfileCommand, Result<Guid>>
{
    private readonly IProfileRepository _profileRepository;
    private readonly IAdministrationUnitOfWork _unitOfWork;
    private readonly IDateTimeProvider _dateTimeProvider;
    private readonly IProfileAuthorizationService _authorization;
    private readonly IHoldingLookupService _holdingLookupService;

    /// <summary>Initializes a new instance of the <see cref="CreateProfileCommandHandler"/> class.</summary>
    /// <param name="profileRepository">The profile repository.</param>
    /// <param name="unitOfWork">The unit of work for atomic persistence.</param>
    /// <param name="dateTimeProvider">Provider for deterministic UTC timestamps.</param>
    /// <param name="authorization">Decides who may create profiles.</param>
    /// <param name="holdingLookupService">Verifies that the owning Holding exists.</param>
    public CreateProfileCommandHandler(
        IProfileRepository profileRepository,
        IAdministrationUnitOfWork unitOfWork,
        IDateTimeProvider dateTimeProvider,
        IProfileAuthorizationService authorization,
        IHoldingLookupService holdingLookupService)
    {
        _profileRepository = profileRepository;
        _unitOfWork = unitOfWork;
        _dateTimeProvider = dateTimeProvider;
        _authorization = authorization;
        _holdingLookupService = holdingLookupService;
    }

    /// <inheritdoc />
    public async Task<Result<Guid>> Handle(
        CreateProfileCommand request,
        CancellationToken cancellationToken)
    {
        var authorization = await _authorization.AuthorizeManageAsync(
            "Create",
            request.HoldingId,
            request.Permissions,
            cancellationToken);

        if (authorization.IsFailure)
        {
            return Result.Failure<Guid>(authorization.Error);
        }

        if (request.HoldingId is { } holdingId
            && !await _holdingLookupService.ExistsAsync(holdingId, cancellationToken))
        {
            return Result.Failure<Guid>(Error.NotFound(
                "Holding.NotFound",
                $"Holding with id {holdingId} was not found."));
        }

        var result = global::Administration.Domain.Profile.Create(
            request.Name,
            request.Permissions,
            _dateTimeProvider,
            request.HoldingId);

        if (result.IsFailure)
        {
            return Result.Failure<Guid>(result.Error);
        }

        _profileRepository.Add(result.Value);
        await _unitOfWork.SaveChangesAsync(cancellationToken);

        return Result.Success(result.Value.Id.Value);
    }
}
