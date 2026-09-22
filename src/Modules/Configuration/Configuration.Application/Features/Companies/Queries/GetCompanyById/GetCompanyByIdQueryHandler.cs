using MachineryManagerEnterprise.Configuration.Application.Abstractions;
using MachineryManagerEnterprise.Configuration.Application.Features.Companies.Dtos;
using MachineryManagerEnterprise.SharedKernel;
using MachineryManagerEnterprise.SharedKernel.Abstractions;
using MapsterMapper;
using MediatR;

namespace MachineryManagerEnterprise.Configuration.Application.Features.Companies.Queries.GetCompanyById;

/// <summary>Handles <see cref="GetCompanyByIdQuery"/>.</summary>
public sealed class GetCompanyByIdQueryHandler : IRequestHandler<GetCompanyByIdQuery, Result<CompanyDto>>
{
    private const string RequiredPermission = "Company.View";

    private readonly ICompanyRepository _companyRepository;
    private readonly ICurrentUserService _currentUserService;
    private readonly IPermissionEvaluator _permissionEvaluator;
    private readonly IMapper _mapper;

    /// <summary>Initializes a new instance of the <see cref="GetCompanyByIdQueryHandler"/> class.</summary>
    /// <param name="companyRepository">The Company repository.</param>
    /// <param name="currentUserService">Provides the current authenticated user's identifier.</param>
    /// <param name="permissionEvaluator">Evaluates the current user's authorization.</param>
    /// <param name="mapper">The Mapster-backed mapper used to project the entity to a DTO.</param>
    public GetCompanyByIdQueryHandler(
        ICompanyRepository companyRepository,
        ICurrentUserService currentUserService,
        IPermissionEvaluator permissionEvaluator,
        IMapper mapper)
    {
        _companyRepository = companyRepository;
        _currentUserService = currentUserService;
        _permissionEvaluator = permissionEvaluator;
        _mapper = mapper;
    }

    /// <summary>Executes the query.</summary>
    /// <param name="request">The query to handle.</param>
    /// <param name="cancellationToken">Token to cancel the asynchronous operation.</param>
    /// <returns>A <see cref="Result{CompanyDto}"/> containing the Company, or a not-found/authorization error.</returns>
    public async Task<Result<CompanyDto>> Handle(GetCompanyByIdQuery request, CancellationToken cancellationToken)
    {
        if (_currentUserService.UserId is not { } userId)
        {
            return Result.Failure<CompanyDto>(global::Configuration.Domain.CompanyErrors.NotAuthorized());
        }

        var company = await _companyRepository.GetByIdAsync(
            global::Configuration.Domain.CompanyId.From(request.Id), cancellationToken);

        if (company is null)
        {
            return Result.Failure<CompanyDto>(global::Configuration.Domain.CompanyErrors.NotFound(request.Id));
        }

        var scope = new ResourceScope(company.HoldingId, null, null);
        var isAuthorized = await _permissionEvaluator.HasPermissionAsync(userId, RequiredPermission, scope, cancellationToken);

        if (!isAuthorized)
        {
            return Result.Failure<CompanyDto>(global::Configuration.Domain.CompanyErrors.NotAuthorized());
        }

        return Result.Success(_mapper.Map<CompanyDto>(company));
    }
}
