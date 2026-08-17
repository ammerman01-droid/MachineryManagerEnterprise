using MachineryManager.Organization.Application.Abstractions;
using MachineryManager.SharedKernel;
using MachineryManager.SharedKernel.Abstractions;
using MediatR;

namespace MachineryManager.Organization.Application.Features.Organizations.Commands.RegisterOrganization;

/// <summary>
/// Handles <see cref="RegisterOrganizationCommand"/> by orchestrating domain
/// registration, persisting the aggregate, and committing the unit of work.
/// </summary>
public sealed class RegisterOrganizationCommandHandler
    : IRequestHandler<RegisterOrganizationCommand, Result<Guid>>
{
    private readonly IOrganizationRepository _organizationRepository;
    private readonly IUnitOfWork _unitOfWork;
    private readonly IDateTimeProvider _dateTimeProvider;

    /// <summary>
    /// Initializes a new instance of the <see cref="RegisterOrganizationCommandHandler"/> class.
    /// </summary>
    /// <param name="organizationRepository">The organization repository.</param>
    /// <param name="unitOfWork">The unit of work for atomic persistence.</param>
    /// <param name="dateTimeProvider">Provider for deterministic UTC timestamps.</param>
    public RegisterOrganizationCommandHandler(
        IOrganizationRepository organizationRepository,
        IUnitOfWork unitOfWork,
        IDateTimeProvider dateTimeProvider)
    {
        _organizationRepository = organizationRepository;
        _unitOfWork = unitOfWork;
        _dateTimeProvider = dateTimeProvider;
    }

    /// <summary>
    /// Executes the registration use case.
    /// </summary>
    /// <param name="request">The registration command.</param>
    /// <param name="cancellationToken">Token to cancel the asynchronous operation.</param>
    /// <returns>A result containing the new organization's GUID on success.</returns>
    public async Task<Result<Guid>> Handle(
        RegisterOrganizationCommand request,
        CancellationToken cancellationToken)
    {
        var result = global::Organization.Domain.Organization.Register(
            request.Name,
            _dateTimeProvider);

        if (result.IsFailure)
        {
            return Result.Failure<Guid>(result.Error);
        }

        _organizationRepository.Add(result.Value);
        await _unitOfWork.SaveChangesAsync(cancellationToken);

        return Result.Success(result.Value.Id.Value);
    }
}