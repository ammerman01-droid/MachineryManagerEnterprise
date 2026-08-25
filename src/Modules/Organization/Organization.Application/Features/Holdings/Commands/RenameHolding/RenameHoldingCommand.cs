using MachineryManager.SharedKernel;
using MediatR;

namespace MachineryManager.Organization.Application.Features.Holdings.Commands.RenameHolding;

/// <summary>Command to rename an existing Holding.</summary>
/// <param name="HoldingId">The identifier of the holding to rename.</param>
/// <param name="Name">The new name.</param>
public sealed record RenameHoldingCommand(Guid HoldingId, string Name) : IRequest<Result>;