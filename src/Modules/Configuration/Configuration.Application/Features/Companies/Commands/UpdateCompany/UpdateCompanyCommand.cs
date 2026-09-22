using MachineryManagerEnterprise.SharedKernel;
using MediatR;

namespace MachineryManagerEnterprise.Configuration.Application.Features.Companies.Commands.UpdateCompany;

/// <summary>Updates an existing Company's display name.</summary>
/// <param name="Id">The identifier of the Company to update.</param>
/// <param name="Name">The new display name of the company.</param>
public sealed record UpdateCompanyCommand(Guid Id, string Name) : IRequest<Result>;
