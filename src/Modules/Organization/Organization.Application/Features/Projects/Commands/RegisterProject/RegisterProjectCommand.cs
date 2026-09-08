using MachineryManagerEnterprise.SharedKernel;
using MediatR;

namespace MachineryManagerEnterprise.Organization.Application.Features.Projects.Commands.RegisterProject;

/// <summary>Command to register a new Project under an Organization.</summary>
public sealed record RegisterProjectCommand(Guid OrganizationId, string Name)
    : IRequest<Result<Guid>>;