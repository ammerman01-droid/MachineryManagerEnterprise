using MachineryManagerEnterprise.Configuration.Application.Abstractions;
using MachineryManagerEnterprise.SharedKernel;
using MachineryManagerEnterprise.SharedKernel.Abstractions;
using MediatR;

namespace MachineryManagerEnterprise.Configuration.Application.Features.Companies.Commands.UpdateCompany;

/// <summary>Handles <see cref="UpdateCompanyCommand"/>.</summary>
public sealed class UpdateCompanyCommandHandler : IRequestHandler<UpdateCompanyCommand, Result>
{
    private const string RequiredPermission = "Company.Edit";

    private readonly ICompanyRepository _companyRepository;
    private readonly IConfigurationUnitOfWork _unitOfWork;
    private readonly IDateTimeProvider _dateTimeProvider;
    private readonly ICurrentUserService _currentUserService;
    private readonly IPermissionEvaluator _permissionEvaluator;

    /// <summary>Initializes a new instance of the <see cref="UpdateCompanyCommandHandler"/> class.</summary>
    /// <param name="companyRepository">The Company repository.</param>
    /// <param name="unitOfWork">The Configuration module's unit of work.</param>
    /// <param name="dateTimeProvider">Provides the current UTC time.</param>
    /// <param name="currentUserService">Provides the current authenticated user's identifier.</param>
    /// <param name="permissionEvaluator">Evaluates the current user's authorization.</param>
    public UpdateCompanyCommandHandler(
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

    /// <summary>Executes the update use case.</summary>
    /// <param name="request">The command to handle.</param>
    /// <param name="cancellationToken">Token to cancel the asynchronous operation.</param>
    /// <returns>A successful <see cref="Result"/>, or a not-found/authorization/validation/conflict error.</returns>
    public async Task<Result> Handle(UpdateCompanyCommand request, CancellationToken cancellationToken)
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

        var trimmedName = request.Name.Trim();

        if (!string.Equals(trimmedName, company.Name, StringComparison.Ordinal)
            && await _companyRepository.ExistsByNameInHoldingAsync(company.HoldingId, trimmedName, cancellationToken))
        {
            return Result.Failure(global::Configuration.Domain.CompanyErrors.AlreadyExists());
        }

        var result = company.Rename(request.Name, _dateTimeProvider);

        if (result.IsFailure)
        {
            return result;
        }

        _companyRepository.Update(company);
        await _unitOfWork.SaveChangesAsync(cancellationToken);

        return Result.Success();
    }
}
