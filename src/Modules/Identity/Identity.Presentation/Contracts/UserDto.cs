namespace MachineryManager.Identity.Presentation.Contracts;

/// <summary>Read-only view of a User.</summary>
public sealed record UserDto(Guid Id, string UserName);