namespace MachineryManager.Organization.Application.Features.Holdings.Dtos;

/// <summary>Read-only view of a Holding.</summary>
public sealed record HoldingDto(Guid Id, string Name);