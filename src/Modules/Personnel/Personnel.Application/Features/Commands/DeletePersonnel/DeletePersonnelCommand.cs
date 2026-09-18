using MachineryManagerEnterprise.SharedKernel;
using MediatR;

namespace MachineryManagerEnterprise.Personnel.Application.Features.Commands.DeletePersonnel;

/// <summary>Deletes a Personnel record.</summary>
public sealed record DeletePersonnelCommand(Guid PersonnelId) : IRequest<Result>;