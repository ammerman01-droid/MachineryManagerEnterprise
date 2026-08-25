using MachineryManager.SharedKernel;
using MediatR;

namespace MachineryManager.Organization.Application.Features.Organizations.Commands.AssignOrganizationToHolding;

/// <summary>
/// Command to assign an existing Organization to a Holding.
/// </summary>
/// <param name="OrganizationId">The GUID of the organization to assign.</param>
/// <param name="HoldingId">The GUID of the target holding.</param>
public sealed record AssignOrganizationToHoldingCommand(Guid OrganizationId, Guid HoldingId)
    : IRequest<Result>;