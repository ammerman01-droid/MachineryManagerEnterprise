using MachineryManager.Asset.Application.Features.Assets.Dtos;
using MachineryManager.SharedKernel;
using MediatR;

namespace MachineryManager.Asset.Application.Features.Assets.Queries.SearchAssets;

/// <summary>
/// Query to search Assets within an Organization. <see cref="SearchTerm"/>,
/// when provided, matches against Code, Name, SerialNumber, or
/// LicensePlate (chat, 2026-08-28).
/// </summary>
public sealed record SearchAssetsQuery(Guid OrganizationId, string? SearchTerm, int Page = 1, int PageSize = 20)
    : IRequest<Result<SearchAssetsResponse>>;

/// <summary>Paginated response for <see cref="SearchAssetsQuery"/>.</summary>
public sealed record SearchAssetsResponse(
    IReadOnlyCollection<AssetDto> Items,
    int Page,
    int PageSize,
    int TotalItems,
    int TotalPages,
    bool HasNextPage,
    bool HasPreviousPage);