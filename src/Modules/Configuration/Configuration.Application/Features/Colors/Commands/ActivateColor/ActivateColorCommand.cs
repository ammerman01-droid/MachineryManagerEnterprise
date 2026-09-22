using MachineryManagerEnterprise.SharedKernel;
using MediatR;

namespace MachineryManagerEnterprise.Configuration.Application.Features.Colors.Commands.ActivateColor;

/// <summary>Reactivates a previously deactivated Color.</summary>
/// <param name="Id">The identifier of the Color to reactivate.</param>
public sealed record ActivateColorCommand(Guid Id) : IRequest<Result>;
