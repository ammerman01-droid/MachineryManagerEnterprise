using FluentValidation;

namespace MachineryManager.AuditLog.Application.Features.AuditEntries.Queries.SearchAuditEntries;

/// <summary>
/// Validates <see cref="SearchAuditEntriesQuery"/> per the module's
/// FluentValidation convention (chat, 2026-09-05, gam 4).
/// </summary>
public sealed class SearchAuditEntriesQueryValidator : AbstractValidator<SearchAuditEntriesQuery>
{
    /// <summary>Initializes a new instance of the <see cref="SearchAuditEntriesQueryValidator"/> class.</summary>
    public SearchAuditEntriesQueryValidator()
    {
        RuleFor(query => query.Page)
            .GreaterThanOrEqualTo(1);

        RuleFor(query => query.PageSize)
            .InclusiveBetween(1, 200);

        RuleFor(query => query.SchemaName)
            .MaximumLength(50)
            .When(query => query.SchemaName is not null);

        RuleFor(query => query.TableName)
            .MaximumLength(100)
            .When(query => query.TableName is not null);

        RuleFor(query => query)
            .Must(query => !query.From.HasValue || !query.To.HasValue || query.From.Value <= query.To.Value)
            .WithMessage("The 'from' timestamp must not be later than the 'to' timestamp.");
    }
}