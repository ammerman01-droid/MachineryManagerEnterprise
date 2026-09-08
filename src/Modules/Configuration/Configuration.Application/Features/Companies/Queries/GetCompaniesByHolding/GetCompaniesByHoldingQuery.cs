using MachineryManagerEnterprise.Configuration.Application.Features.Companies.Dtos;
using MachineryManagerEnterprise.SharedKernel;
using MediatR;

namespace MachineryManagerEnterprise.Configuration.Application.Features.Companies.Queries.GetCompaniesByHolding;

/// <summary>
/// Query to retrieve every Company registered for a Holding (used to
/// populate manufacturer-selection lists in AssetModel, EngineModel,
/// and Asset forms).
/// </summary>
/// <param name="HoldingId">The Holding whose Company catalog should be returned.</param>
public sealed record GetCompaniesByHoldingQuery(Guid HoldingId) : IRequest<Result<IReadOnlyList<CompanyDto>>>;