using MachineryManager.SharedKernel;
using MediatR;

namespace MachineryManager.Organization.Application.Features.Organizations.Commands.SuspendOrganization;

/// <summary>
/// Command to suspend an Organization (BR-017, Section 10.16,
/// RESOLVED). Suspension is a status flag only — historical records
/// remain intact and are never deleted.
/// </summary>
/// <param name="OrganizationId">The GUID of the organization to suspend.</param>
public sealed record SuspendOrganizationCommand(Guid OrganizationId)
    : IRequest<Result>;