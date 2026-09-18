using MachineryManagerEnterprise.SharedKernel;
using MediatR;

namespace MachineryManagerEnterprise.Configuration.Application.Features.JobTitles.Commands.RenameJobTitle;

/// <summary>Renames an existing Job Title.</summary>
public sealed record RenameJobTitleCommand(Guid JobTitleId, string Name) : IRequest<Result>;