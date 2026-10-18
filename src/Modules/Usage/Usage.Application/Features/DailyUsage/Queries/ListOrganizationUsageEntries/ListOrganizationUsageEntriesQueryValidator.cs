using FluentValidation;

namespace MachineryManagerEnterprise.Usage.Application.Features.DailyUsage.Queries.ListOrganizationUsageEntries;

/// <summary>
/// Validates <see cref="ListOrganizationUsageEntriesQuery"/> per ADR-0036 — in particular caps the
/// page size, because this list can grow by many rows every day (chat, 2026-10-13).
/// </summary>
public sealed class ListOrganizationUsageEntriesQueryValidator : AbstractValidator<ListOrganizationUsageEntriesQuery>
{
    /// <summary>Initializes validation rules for the organization usage list query.</summary>
    public ListOrganizationUsageEntriesQueryValidator()
    {
        RuleFor(x => x.OrganizationId).NotEmpty();
        RuleFor(x => x.Page).GreaterThanOrEqualTo(1);
        RuleFor(x => x.PageSize).InclusiveBetween(1, 200);
    }
}
