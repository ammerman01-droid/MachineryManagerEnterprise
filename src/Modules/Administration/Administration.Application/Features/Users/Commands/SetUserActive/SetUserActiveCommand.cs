using MachineryManagerEnterprise.SharedKernel;
using MediatR;

namespace MachineryManagerEnterprise.Administration.Application.Features.Users.Commands.SetUserActive;

/// <summary>Activates or deactivates (locks out) a user account.</summary>
/// <param name="UserId">The user.</param>
/// <param name="Active"><see langword="true"/> to activate, <see langword="false"/> to deactivate.</param>
public sealed record SetUserActiveCommand(Guid UserId, bool Active) : IRequest<Result>;
