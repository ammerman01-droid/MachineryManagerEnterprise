using MachineryManager.SharedKernel;
using MediatR;

namespace MachineryManager.Configuration.Application.Features.Companies.Commands.RegisterCompany;

/// <summary>
/// Command to register a new Company within a Holding.
/// </summary>
public sealed record RegisterCompanyCommand(
    Guid HoldingId,
    string Name) : IRequest<Result<Guid>>;