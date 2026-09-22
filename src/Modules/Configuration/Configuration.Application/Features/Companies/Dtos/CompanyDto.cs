namespace MachineryManagerEnterprise.Configuration.Application.Features.Companies.Dtos;

/// <summary>
/// Represents a Company option returned by the Configuration module.
/// </summary>
/// <param name="Id">The Company's identifier.</param>
/// <param name="Name">The Company's display name.</param>
/// <param name="IsActive">Whether the Company is currently active (false when soft-deleted).</param>
public sealed record CompanyDto(
    Guid Id,
    string Name,
    bool IsActive);
