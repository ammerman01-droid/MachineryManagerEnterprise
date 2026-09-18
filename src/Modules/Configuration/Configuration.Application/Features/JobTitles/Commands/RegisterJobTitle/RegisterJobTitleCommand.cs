using MachineryManagerEnterprise.SharedKernel;
using MediatR;

namespace MachineryManagerEnterprise.Configuration.Application.Features.JobTitles.Commands.RegisterJobTitle;

/// <summary>Registers a new Job Title option within a Holding.</summary>
public sealed record RegisterJobTitleCommand(Guid HoldingId, string Name) : IRequest<Result<Guid>>;