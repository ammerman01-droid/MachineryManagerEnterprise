using MachineryManager.Configuration.Application.Abstractions;
using MachineryManager.SharedKernel;
using MachineryManager.SharedKernel.Abstractions;
using MediatR;

namespace MachineryManager.Configuration.Application.Features.Companies.Commands.RegisterCompany;

/// <summary>
/// Handles <see cref="RegisterCompanyCommand"/> by verifying the target
/// Holding, checking authorization and duplicate names, invoking domain
/// registration, persisting the aggregate, and committing the unit of work.
/// </summary>
public sealed class RegisterCompanyCommandHandler : IRequestHandler<RegisterCompanyCommand, Result<Guid>>
{
    private const string RequiredPermission = "Company.Create";

    private readonly ICompanyRepository _companyRepository;
    private readonly IHoldingLookupService _holdingLookupService;
    private readonly IConfigurationUnitOfWork _unitOfWork;
    private readonly IDateTimeProvider _dateTimeProvider;
    private readonly ICurrentUserService _currentUserService;
    private readonly IPermissionEvaluator _permissionEvaluator;

    /// <summary>Initializes a new instance of the <see cref="RegisterCompanyCommandHandler"/> class.</summary>
    public RegisterCompanyCommandHandler(
        ICompanyRepository companyRepository,
        IHoldingLookupService holdingLookupService,
        IConfigurationUnitOfWork unitOfWork,
        IDateTimeProvider dateTimeProvider,
        ICurrentUserService currentUserService,
        IPermissionEvaluator permissionEvaluator)
    {
        _companyRepository = companyRepository;
        _holdingLookupService = holdingLookupService;
        _unitOfWork = unitOfWork;
        _dateTimeProvider = dateTimeProvider;
        _currentUserService = currentUserService;
        _permissionEvaluator = permissionEvaluator;
    }

    /// <summary>Executes the Company registration use case.</summary>
    public async Task<Result<Guid>> Handle(RegisterCompanyCommand request, CancellationToken cancellationToken)
    {
        if (_currentUserService.UserId is not { } userId)
        {
            return Result.Failure<Guid>(global::Configuration.Domain.CompanyErrors.NotAuthorized());
        }

        if (!await _holdingLookupService.ExistsAsync(request.HoldingId, cancellationToken))
        {
            return Result.Failure<Guid>(
                Error.NotFound("Holding.NotFound", $"Holding with id {request.HoldingId} was not found."));
        }

        var scope = new ResourceScope(request.HoldingId, null, null);
        var isAuthorized = await _permissionEvaluator.HasPermissionAsync(
            userId, RequiredPermission, scope, cancellationToken);

        if (!isAuthorized)
        {
            return Result.Failure<Guid>(global::Configuration.Domain.CompanyErrors.NotAuthorized());
        }

        if (await _companyRepository.ExistsByNameInHoldingAsync(request.HoldingId, request.Name, cancellationToken))
        {
            return Result.Failure<Guid>(global::Configuration.Domain.CompanyErrors.AlreadyExists());
        }

        var result = global::Configuration.Domain.Company.Register(
            request.HoldingId,
            request.Name,
            _dateTimeProvider);

        if (result.IsFailure)
        {
            return Result.Failure<Guid>(result.Error);
        }

        _companyRepository.Add(result.Value);
        await _unitOfWork.SaveChangesAsync(cancellationToken);

        return Result.Success(result.Value.Id.Value);
    }
}