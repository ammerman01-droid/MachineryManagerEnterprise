using FluentValidation;

namespace MachineryManager.Configuration.Application.Features.Companies.Commands.RegisterCompany;

/// <summary>
/// Validates <see cref="RegisterCompanyCommand"/>.
/// </summary>
public sealed class RegisterCompanyCommandValidator
    : AbstractValidator<RegisterCompanyCommand>
{
    /// <summary>
    /// Initializes the Company registration validation rules.
    /// </summary>
    public RegisterCompanyCommandValidator()
    {
        RuleFor(x => x.HoldingId)
            .NotEmpty();

        RuleFor(x => x.Name)
            .NotEmpty()
            .MaximumLength(global::Configuration.Domain.Company.MaxNameLength);
    }
}