using MachineryManager.Configuration.Application.Abstractions;
using MachineryManager.Configuration.Application.Features.Companies.Dtos;
using MachineryManager.SharedKernel;
using MachineryManager.SharedKernel.Abstractions;
using MediatR;

namespace MachineryManager.Configuration.Application.Features.Companies.Queries.GetCompaniesByHolding;

/// <summary>
/// Handles <see cref="GetCompaniesByHoldingQuery"/> by verifying the
/// caller holds Company.View for the Holding, then returning the
/// Holding's full company catalog.
/// </summary>
public sealed class GetCompaniesByHoldingQueryHandler
    : IRequestHandler<GetCompaniesByHoldingQuery, Result<IReadOnlyList<CompanyDto>>>
{
    private const string RequiredPermission = "Company.View";

    private readonly ICompanyRepository _companyRepository;
    private readonly ICurrentUserService _currentUserService;
    private readonly IPermissionEvaluator _permissionEvaluator;

    /// <summary>Initializes a new instance of the <see cref="GetCompaniesByHoldingQueryHandler"/> class.</summary>
    /// <param name="companyRepository">The Company repository.</param>
    /// <param name="currentUserService">Provides the authenticated user context.</param>
    /// <param name="permissionEvaluator">Evaluates the current user's permissions at request time.</param>
    public GetCompaniesByHoldingQueryHandler(
        ICompanyRepository companyRepository,
        ICurrentUserService currentUserService,
        IPermissionEvaluator permissionEvaluator)
    {
        _companyRepository = companyRepository;
        _currentUserService = currentUserService;
        _permissionEvaluator = permissionEvaluator;
    }

    /// <summary>Executes the lookup use case.</summary>
    /// <param name="request">The query containing the Holding identifier.</param>
    /// <param name="cancellationToken">Token to cancel the asynchronous operation.</param>
    /// <returns>A result containing the Holding's Company catalog, or a not-authorized error.</returns>
    public async Task<Result<IReadOnlyList<CompanyDto>>> Handle(
        GetCompaniesByHoldingQuery request, CancellationToken cancellationToken)
    {
        if (_currentUserService.UserId is not { } userId)
        {
            return Result.Failure<IReadOnlyList<CompanyDto>>(global::Configuration.Domain.CompanyErrors.NotAuthorized());
        }

        var isAuthorized = await _permissionEvaluator.HasPermissionAsync(
            userId, RequiredPermission, new ResourceScope(request.HoldingId, null, null), cancellationToken);

        if (!isAuthorized)
        {
            return Result.Failure<IReadOnlyList<CompanyDto>>(global::Configuration.Domain.CompanyErrors.NotAuthorized());
        }

        var companies = await _companyRepository.GetByHoldingAsync(request.HoldingId, cancellationToken);

        return Result.Success(companies);
    }
}