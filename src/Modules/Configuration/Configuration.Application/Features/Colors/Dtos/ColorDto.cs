namespace MachineryManagerEnterprise.Configuration.Application.Features.Colors.Dtos;

/// <summary>Represents the ColorDto data contract.</summary>
/// <param name="Id">The value supplied for Id.</param>
/// <param name="Name">The value supplied for Name.</param>
/// <param name="IsActive">Whether the Color is currently active (false when soft-deleted).</param>
public sealed record ColorDto(Guid Id, string Name, bool IsActive);
