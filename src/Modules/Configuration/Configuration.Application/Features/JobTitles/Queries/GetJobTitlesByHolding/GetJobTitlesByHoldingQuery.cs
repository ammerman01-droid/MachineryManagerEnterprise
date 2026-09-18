using MachineryManagerEnterprise.Configuration.Application.Features.JobTitles.Dtos;
using MachineryManagerEnterprise.SharedKernel;
using MediatR;

namespace MachineryManagerEnterprise.Configuration.Application.Features.JobTitles.Queries.GetJobTitlesByHolding;

/// <summary>Retrieves the list of Job Title options defined for a Holding.</summary>
public sealed record GetJobTitlesByHoldingQuery(Guid HoldingId) : IRequest<Result<IReadOnlyList<JobTitleDto>>>;