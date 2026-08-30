using MachineryManager.SharedKernel;
using MediatR;

namespace MachineryManager.Configuration.Application.Features.UnitCategories.Commands.RegisterUnitCategory;

/// <summary>Registers a new Unit Category (e.g. "Power", "Volume") within a Holding.</summary>
/// <param name="HoldingId">The identifier of the owning Holding.</param>
/// <param name="Name">The display name of the category.</param>
public sealed record RegisterUnitCategoryCommand(Guid HoldingId, string Name) : IRequest<Result<Guid>>;