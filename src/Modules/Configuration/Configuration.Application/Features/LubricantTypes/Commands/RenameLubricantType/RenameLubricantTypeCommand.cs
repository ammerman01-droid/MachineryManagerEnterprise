using MachineryManagerEnterprise.SharedKernel;
using MediatR;

namespace MachineryManagerEnterprise.Configuration.Application.Features.LubricantTypes.Commands.RenameLubricantType;

/// <summary>Renames an existing Lubricant Type.</summary>
public sealed record RenameLubricantTypeCommand(Guid LubricantTypeId, string Name) : IRequest<Result>;
