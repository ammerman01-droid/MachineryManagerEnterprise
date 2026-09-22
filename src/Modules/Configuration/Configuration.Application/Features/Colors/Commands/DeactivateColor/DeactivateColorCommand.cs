using MachineryManagerEnterprise.SharedKernel;
using MediatR;

namespace MachineryManagerEnterprise.Configuration.Application.Features.Colors.Commands.DeactivateColor;

/// <summary>Deactivates (soft-deletes) an existing Color.</summary>
/// <param name="Id">The identifier of the Color to deactivate.</param>
public sealed record DeactivateColorCommand(Guid Id) : IRequest<Result>;
