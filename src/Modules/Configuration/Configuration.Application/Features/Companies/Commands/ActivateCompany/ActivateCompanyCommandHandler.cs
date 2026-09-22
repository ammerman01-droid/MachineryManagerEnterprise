using MachineryManagerEnterprise.Configuration.Application.Abstractions;
using MachineryManagerEnterprise.SharedKernel;
using MachineryManagerEnterprise.SharedKernel.Abstractions;
using MediatR;

namespace MachineryManagerEnterprise.Configuration.Application.Features.Companies.Commands.ActivateCompany;

/// <summary>Handles <see cref="ActivateCompanyCommand"/>.</summary>
public sealed class ActivateCompanyCommandHandler : IRequestHandler<ActivateCompanyCommand, Result>
{
    private const string RequiredPermission = "Company.Edit";

    private readonly ICompanyRepository _companyRepository;
    private readonly IConfigurationUnitOfWork _unitOfWork;
    private readonly IDateTimeProvider _dateTimeProvider;
    private readonly ICurrentUserService _currentUserService;
    private readonly IPermissionEvaluator _permissionEvaluator;

    /// <summary>Initializes a new instance of the <see cref="ActivateCompanyCommandHandler"/> class.</summary>
    /// <param name="companyRepository">The Company repository.</param>
    /// <param name="unitOfWork">The Configuration module's unit of work.</param>
    /// <param name="dateTimeProvider">Provides the current UTC time.</param>
    /// <param name="currentUserService">Provides the current authenticated user's identifier.</param>
    /// <param name="permissionEvaluator">Evaluates the current user's authorization.</param>
    public ActivateCompanyCommandHandler(
        ICompanyRepository companyRepository,
        IConfigurationUnitOfWork unitOfWork,
        IDateTimeProvider dateTimeProvider,
        ICurrentUserService currentUserService,
        IPermissionEvaluator permissionEvaluator)
    {
        _companyRepository = companyRepository;
        _unitOfWork = unitOfWork;
        _dateTimeProvider = dateTimeProvider;
        _currentUserService = currentUserService;
        _permissionEvaluator = permissionEvaluator;
    }

    /// <summary>Executes the reactivation use case.</summary>
    /// <param name="request">The command to handle.</param>
    /// <param name="cancellationToken">Token to cancel the asynchronous operation.</param>
    /// <returns>A successful <see cref="Result"/>, or a not-found/authorization/conflict error.</returns>
    public async Task<Result> Handle(ActivateCompanyCommand request, CancellationToken cancellationToken)
    {
        if (_currentUserService.UserId is not { } userId)
        {
            return Result.Failure(global::Configuration.Domain.CompanyErrors.NotAuthorized());
        }

        var company = await _companyRepository.GetByIdAsync(
            global::Configuration.Domain.CompanyId.From(request.Id), cancellationToken);

        if (company is null)
        {
            return Result.Failure(global::Configuration.Domain.CompanyErrors.NotFound(request.Id));
        }

        var scope = new ResourceScope(company.HoldingId, null, null);
        var isAuthorized = await _permissionEvaluator.HasPermissionAsync(userId, RequiredPermission, scope, cancellationToken);

        if (!isAuthorized)
        {
            return Result.Failure(global::Configuration.Domain.CompanyErrors.NotAuthorized());
        }

        var result = company.Activate(_dateTimeProvider);

        if (result.IsFailure)
        {
            return result;
        }

        _companyRepository.Update(company);
        await _unitOfWork.SaveChangesAsync(cancellationToken);

        return Result.Success();
    }
}
