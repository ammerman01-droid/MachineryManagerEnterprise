using MachineryManager.SharedKernel;
using MediatR;

namespace MachineryManager.Organization.Application.Features.Projects.Commands.RenameProject;

/// <summary>Command to rename an existing Project.</summary>
/// <param name="ProjectId">The identifier of the project to rename.</param>
/// <param name="Name">The new name.</param>
public sealed record RenameProjectCommand(Guid ProjectId, string Name) : IRequest<Result>;