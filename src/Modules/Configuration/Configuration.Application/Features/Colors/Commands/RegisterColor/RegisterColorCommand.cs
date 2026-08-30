using MachineryManager.SharedKernel;
using MediatR;

namespace MachineryManager.Configuration.Application.Features.Colors.Commands.RegisterColor;

/// <summary>Registers a new Color option within a Holding.</summary>
/// <param name="HoldingId">The identifier of the owning Holding.</param>
/// <param name="Name">The display name of the color.</param>
public sealed record RegisterColorCommand(Guid HoldingId, string Name) : IRequest<Result<Guid>>;