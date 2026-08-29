using MachineryManager.SharedKernel;
using MediatR;

namespace MachineryManager.Asset.Application.Features.Colors.Commands.RegisterColor;

/// <summary>Command to register a new Color option within an Organization.</summary>
public sealed record RegisterColorCommand(Guid OrganizationId, string Name) : IRequest<Result<Guid>>;