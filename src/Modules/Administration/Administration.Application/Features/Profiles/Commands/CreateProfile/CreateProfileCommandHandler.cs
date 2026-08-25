using MachineryManager.Administration.Application.Abstractions;
using MachineryManager.SharedKernel;
using MachineryManager.SharedKernel.Abstractions;
using MediatR;

namespace MachineryManager.Administration.Application.Features.Profiles.Commands.CreateProfile;

/// <summary>
/// Handles <see cref="CreateProfileCommand"/> by orchestrating domain
/// creation, persisting the aggregate, and committing the unit of work.
/// </summary>
public sealed class CreateProfileCommandHandler
    : IRequestHandler<CreateProfileCommand, Result<Guid>>
{
    private readonly IProfileRepository _profileRepository;
    private readonly IAdministrationUnitOfWork _unitOfWork;
    private readonly IDateTimeProvider _dateTimeProvider;

    /// <summary>
    /// Initializes a new instance of the <see cref="CreateProfileCommandHandler"/> class.
    /// </summary>
    /// <param name="profileRepository">The profile repository.</param>
    /// <param name="unitOfWork">The unit of work for atomic persistence.</param>
    /// <param name="dateTimeProvider">Provider for deterministic UTC timestamps.</param>
    public CreateProfileCommandHandler(
        IProfileRepository profileRepository,
        IAdministrationUnitOfWork unitOfWork,
        IDateTimeProvider dateTimeProvider)
    {
        _profileRepository = profileRepository;
        _unitOfWork = unitOfWork;
        _dateTimeProvider = dateTimeProvider;
    }

    /// <summary>
    /// Executes the creation use case.
    /// </summary>
    /// <param name="request">The creation command.</param>
    /// <param name="cancellationToken">Token to cancel the asynchronous operation.</param>
    /// <returns>A result containing the new profile's GUID on success.</returns>
    public async Task<Result<Guid>> Handle(
        CreateProfileCommand request,
        CancellationToken cancellationToken)
    {
        var result = global::Administration.Domain.Profile.Create(
            request.Name,
            request.Permissions,
            _dateTimeProvider);

        if (result.IsFailure)
        {
            return Result.Failure<Guid>(result.Error);
        }

        _profileRepository.Add(result.Value);
        await _unitOfWork.SaveChangesAsync(cancellationToken);

        return Result.Success(result.Value.Id.Value);
    }
}