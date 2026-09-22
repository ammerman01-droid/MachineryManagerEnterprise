using MachineryManagerEnterprise.SharedKernel;
using MediatR;

namespace MachineryManagerEnterprise.Configuration.Application.Features.Companies.Commands.DeactivateCompany;

/// <summary>Deactivates (soft-deletes) an existing Company.</summary>
/// <param name="Id">The identifier of the Company to deactivate.</param>
public sealed record DeactivateCompanyCommand(Guid Id) : IRequest<Result>;
