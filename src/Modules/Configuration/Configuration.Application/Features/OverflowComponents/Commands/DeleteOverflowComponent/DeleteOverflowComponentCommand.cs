using MachineryManagerEnterprise.SharedKernel;
using MediatR;

namespace MachineryManagerEnterprise.Configuration.Application.Features.OverflowComponents.Commands.DeleteOverflowComponent;

/// <summary>Deletes a Overflow Component, provided it is not referenced by any Lubricant Overflow Report line.</summary>
public sealed record DeleteOverflowComponentCommand(Guid OverflowComponentId) : IRequest<Result>;
