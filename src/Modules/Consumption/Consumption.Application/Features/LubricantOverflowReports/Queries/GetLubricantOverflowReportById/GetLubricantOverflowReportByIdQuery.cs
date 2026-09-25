using MachineryManagerEnterprise.Consumption.Application.Features.LubricantOverflowReports.Dtos;
using MachineryManagerEnterprise.SharedKernel;
using MediatR;

namespace MachineryManagerEnterprise.Consumption.Application.Features.LubricantOverflowReports.Queries.GetLubricantOverflowReportById;

/// <summary>Retrieves a single Lubricant Overflow Report by its identifier.</summary>
public sealed record GetLubricantOverflowReportByIdQuery(Guid LubricantOverflowReportId) : IRequest<Result<LubricantOverflowReportDto>>;
