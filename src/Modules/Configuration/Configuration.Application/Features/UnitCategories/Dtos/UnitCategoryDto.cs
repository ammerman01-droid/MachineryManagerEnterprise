namespace MachineryManager.Configuration.Application.Features.UnitCategories.Dtos;

/// <summary>Represents the UnitCategoryDto data contract.</summary>
/// <param name="Id">The value supplied for Id.</param>
/// <param name="Name">The value supplied for Name.</param>
public sealed record UnitCategoryDto(Guid Id, string Name);