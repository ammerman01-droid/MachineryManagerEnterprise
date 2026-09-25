using MachineryManagerEnterprise.SharedKernel;
using MediatR;

namespace MachineryManagerEnterprise.Configuration.Application.Features.LubricantTypes.Commands.DeleteLubricantType;

/// <summary>Deletes a Lubricant Type, provided it is not referenced by any Lubricant Overflow Report line.</summary>
public sealed record DeleteLubricantTypeCommand(Guid LubricantTypeId) : IRequest<Result>;
