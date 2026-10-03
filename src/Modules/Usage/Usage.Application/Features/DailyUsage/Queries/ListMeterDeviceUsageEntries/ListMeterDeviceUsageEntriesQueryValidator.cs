using FluentValidation;

namespace MachineryManagerEnterprise.Usage.Application.Features.DailyUsage.Queries.ListMeterDeviceUsageEntries;

/// <summary>Validates <see cref="ListMeterDeviceUsageEntriesQuery"/> per ADR-0036.</summary>
public sealed class ListMeterDeviceUsageEntriesQueryValidator : AbstractValidator<ListMeterDeviceUsageEntriesQuery>
{
    /// <summary>Initializes validation rules for the list meter device usage entries query.</summary>
    public ListMeterDeviceUsageEntriesQueryValidator()
    {
        RuleFor(x => x.MeterDeviceId).NotEmpty();
        RuleFor(x => x.Page).GreaterThanOrEqualTo(1);
        RuleFor(x => x.PageSize).InclusiveBetween(1, 200);

        RuleFor(x => x.ToDate)
            .GreaterThanOrEqualTo(x => x.FromDate)
            .When(x => x.FromDate is not null && x.ToDate is not null)
            .WithMessage("ToDate must not be earlier than FromDate.");
    }
}
