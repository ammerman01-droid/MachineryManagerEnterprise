using FluentValidation;

namespace MachineryManagerEnterprise.Usage.Application.Features.DailyUsage.Queries.GetAccumulatedOperationalUsage;

/// <summary>Validates <see cref="GetAccumulatedOperationalUsageQuery"/> per ADR-0036.</summary>
public sealed class GetAccumulatedOperationalUsageQueryValidator : AbstractValidator<GetAccumulatedOperationalUsageQuery>
{
    /// <summary>Initializes validation rules for the accumulated operational usage query.</summary>
    public GetAccumulatedOperationalUsageQueryValidator()
    {
        RuleFor(x => x.OwnerId).NotEmpty();

        RuleFor(x => x.ThroughDate)
            .GreaterThanOrEqualTo(x => x.SinceDate)
            .When(x => x.ThroughDate is not null)
            .WithMessage("ThroughDate must not be earlier than SinceDate.");
    }
}
