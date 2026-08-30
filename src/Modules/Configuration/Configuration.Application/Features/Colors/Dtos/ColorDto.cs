namespace MachineryManager.Configuration.Application.Features.Colors.Dtos;

/// <summary>Represents the ColorDto data contract.</summary>
/// <param name="Id">The value supplied for Id.</param>
/// <param name="Name">The value supplied for Name.</param>
public sealed record ColorDto(Guid Id, string Name);