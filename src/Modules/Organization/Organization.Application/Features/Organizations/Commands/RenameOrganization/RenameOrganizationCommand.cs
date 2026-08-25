using MachineryManager.SharedKernel;
using MediatR;

namespace MachineryManager.Organization.Application.Features.Organizations.Commands.RenameOrganization;

/// <summary>Command to rename an existing Organization.</summary>
/// <param name="OrganizationId">The identifier of the organization to rename.</param>
/// <param name="Name">The new name.</param>
public sealed record RenameOrganizationCommand(Guid OrganizationId, string Name) : IRequest<Result>;