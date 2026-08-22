namespace MachineryManager.Identity.Presentation.Contracts;

/// <summary>Request body for creating a new user.</summary>
public sealed record CreateUserRequest(string UserName, string Password, string[]? Roles = null);