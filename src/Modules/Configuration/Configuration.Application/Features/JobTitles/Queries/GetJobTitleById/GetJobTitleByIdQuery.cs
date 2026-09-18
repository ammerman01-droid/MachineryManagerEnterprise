using MachineryManagerEnterprise.Configuration.Application.Features.JobTitles.Dtos;
using MachineryManagerEnterprise.SharedKernel;
using MediatR;

namespace MachineryManagerEnterprise.Configuration.Application.Features.JobTitles.Queries.GetJobTitleById;

/// <summary>Retrieves a single Job Title by its identifier.</summary>
public sealed record GetJobTitleByIdQuery(Guid JobTitleId) : IRequest<Result<JobTitleDto>>;