using MachineryManagerEnterprise.SharedKernel;
using MediatR;

namespace MachineryManagerEnterprise.Configuration.Application.Features.Colors.Commands.UpdateColor;

/// <summary>Updates an existing Color's display name.</summary>
/// <param name="Id">The identifier of the Color to update.</param>
/// <param name="Name">The new display name of the color.</param>
public sealed record UpdateColorCommand(Guid Id, string Name) : IRequest<Result>;
