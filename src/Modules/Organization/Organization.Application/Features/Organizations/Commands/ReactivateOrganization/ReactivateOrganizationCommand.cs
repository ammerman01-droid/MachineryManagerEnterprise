using MachineryManagerEnterprise.SharedKernel;
using MediatR;

namespace MachineryManagerEnterprise.Organization.Application.Features.Organizations.Commands.ReactivateOrganization;

/// <summary>Command to reactivate a previously suspended Organization (BR-017, Section 10.16).</summary>
/// <param name="OrganizationId">The GUID of the organization to reactivate.</param>
public sealed record ReactivateOrganizationCommand(Guid OrganizationId)
    : IRequest<Result>;