using MachineryManagerEnterprise.Configuration.Application.Features.Companies.Dtos;
using MachineryManagerEnterprise.SharedKernel;
using MediatR;

namespace MachineryManagerEnterprise.Configuration.Application.Features.Companies.Queries.GetCompanyById;

/// <summary>Retrieves a single Company by its identifier (used to pre-fill the Edit form).</summary>
/// <param name="Id">The identifier of the Company to retrieve.</param>
public sealed record GetCompanyByIdQuery(Guid Id) : IRequest<Result<CompanyDto>>;
