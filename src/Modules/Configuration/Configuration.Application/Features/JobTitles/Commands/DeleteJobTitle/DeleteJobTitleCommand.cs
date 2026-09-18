using MachineryManagerEnterprise.SharedKernel;
using MediatR;

namespace MachineryManagerEnterprise.Configuration.Application.Features.JobTitles.Commands.DeleteJobTitle;

/// <summary>Deletes a Job Title, provided it is not referenced by any Personnel record.</summary>
public sealed record DeleteJobTitleCommand(Guid JobTitleId) : IRequest<Result>;