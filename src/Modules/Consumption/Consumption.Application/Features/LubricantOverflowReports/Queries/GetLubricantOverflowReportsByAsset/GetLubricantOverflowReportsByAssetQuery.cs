using MachineryManagerEnterprise.Consumption.Application.Features.LubricantOverflowReports.Dtos;
using MachineryManagerEnterprise.SharedKernel;
using MediatR;

namespace MachineryManagerEnterprise.Consumption.Application.Features.LubricantOverflowReports.Queries.GetLubricantOverflowReportsByAsset;

/// <summary>Retrieves the Lubricant Overflow Reports recorded for a given Asset, most recent first.</summary>
public sealed record GetLubricantOverflowReportsByAssetQuery(Guid AssetId) : IRequest<Result<IReadOnlyList<LubricantOverflowReportDto>>>;
