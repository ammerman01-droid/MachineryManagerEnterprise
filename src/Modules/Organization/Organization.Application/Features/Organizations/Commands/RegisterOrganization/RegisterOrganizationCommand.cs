using MachineryManager.SharedKernel;
using MediatR;

namespace MachineryManager.Organization.Application.Features.Organizations.Commands.RegisterOrganization;

/// <summary>
/// Command to register a new Organization (UC-1301 / CMD-950).
/// </summary>
/// <param name="Name">The display name of the organization to create.</param>
public sealed record RegisterOrganizationCommand(string Name)
    : IRequest<Result<Guid>>;