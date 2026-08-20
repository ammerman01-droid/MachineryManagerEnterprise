using FluentValidation;

namespace MachineryManager.Organization.Application.Features.Holdings.Queries.GetHoldingById;

/// <summary>
/// Validates <see cref="GetHoldingByIdQuery"/> per ADR-0036.
/// </summary>
public sealed class GetHoldingByIdQueryValidator : AbstractValidator<GetHoldingByIdQuery>
{
    /// <summary>
    /// Initializes validation rules for the get-by-id query.
    /// </summary>
    public GetHoldingByIdQueryValidator()
    {
        RuleFor(x => x.HoldingId).NotEmpty();
    }
}