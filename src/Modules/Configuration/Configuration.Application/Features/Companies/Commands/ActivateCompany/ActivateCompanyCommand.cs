using MachineryManagerEnterprise.SharedKernel;
using MediatR;

namespace MachineryManagerEnterprise.Configuration.Application.Features.Companies.Commands.ActivateCompany;

/// <summary>Reactivates a previously deactivated Company.</summary>
/// <param name="Id">The identifier of the Company to reactivate.</param>
public sealed record ActivateCompanyCommand(Guid Id) : IRequest<Result>;
