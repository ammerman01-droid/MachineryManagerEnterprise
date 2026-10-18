using FluentValidation;

namespace MachineryManagerEnterprise.Usage.Application.Features.DailyUsage.Commands.FreezeOrganizationUsage;

/// <summary>Validates <see cref="FreezeOrganizationUsageCommand"/> per ADR-0036.</summary>
public sealed class FreezeOrganizationUsageCommandValidator : AbstractValidator<FreezeOrganizationUsageCommand>
{
    /// <summary>Initializes validation rules for the bulk freeze command.</summary>
    public FreezeOrganizationUsageCommandValidator()
    {
        RuleFor(x => x.OrganizationId).NotEmpty();
        RuleFor(x => x.FrozenUpToDate).NotEqual(default(DateOnly));
        RuleFor(x => x.ProjectId).NotEqual(Guid.Empty).When(x => x.ProjectId.HasValue);
    }
}
