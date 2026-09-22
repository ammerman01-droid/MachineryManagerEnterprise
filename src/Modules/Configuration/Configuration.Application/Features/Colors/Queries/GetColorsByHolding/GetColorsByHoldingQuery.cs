using MachineryManagerEnterprise.Configuration.Application.Features.Colors.Dtos;
using MachineryManagerEnterprise.SharedKernel;
using MediatR;

namespace MachineryManagerEnterprise.Configuration.Application.Features.Colors.Queries.GetColorsByHolding;

/// <summary>Retrieves the list of Color options defined for a Holding.</summary>
/// <param name="HoldingId">The identifier of the Holding.</param>
/// <param name="IncludeInactive">
/// When <see langword="true"/>, deactivated (soft-deleted) colors are
/// included in the result (used by the admin management page).
/// Defaults to <see langword="false"/>.
/// </param>
public sealed record GetColorsByHoldingQuery(Guid HoldingId, bool IncludeInactive = false)
    : IRequest<Result<IReadOnlyList<ColorDto>>>;
