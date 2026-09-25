using MachineryManagerEnterprise.SharedKernel;
using MediatR;

namespace MachineryManagerEnterprise.Configuration.Application.Features.OverflowComponents.Commands.RenameOverflowComponent;

/// <summary>Renames an existing Overflow Component.</summary>
public sealed record RenameOverflowComponentCommand(Guid OverflowComponentId, string Name) : IRequest<Result>;
