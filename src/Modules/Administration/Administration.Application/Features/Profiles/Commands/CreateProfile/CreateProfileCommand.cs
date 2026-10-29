using MachineryManagerEnterprise.SharedKernel;
using MediatR;

namespace MachineryManagerEnterprise.Administration.Application.Features.Profiles.Commands.CreateProfile;

/// <summary>Creates a new permission Profile.</summary>
/// <param name="Name">The profile's display name.</param>
/// <param name="Permissions">The permissions the profile grants.</param>
/// <param name="HoldingId">
/// The owning Holding, or <see langword="null"/> for a Platform-owned profile
/// (Platform users only).
/// </param>
public sealed record CreateProfileCommand(
    string Name,
    IReadOnlyList<string> Permissions,
    Guid? HoldingId = null)
    : IRequest<Result<Guid>>;
