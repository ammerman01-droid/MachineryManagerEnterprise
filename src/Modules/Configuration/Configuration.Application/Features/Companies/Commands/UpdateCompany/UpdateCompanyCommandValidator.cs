using FluentValidation;

namespace MachineryManagerEnterprise.Configuration.Application.Features.Companies.Commands.UpdateCompany;

/// <summary>Validates <see cref="UpdateCompanyCommand"/>.</summary>
public sealed class UpdateCompanyCommandValidator : AbstractValidator<UpdateCompanyCommand>
{
    /// <summary>Initializes a new instance of the <see cref="UpdateCompanyCommandValidator"/> class.</summary>
    public UpdateCompanyCommandValidator()
    {
        RuleFor(x => x.Id).NotEmpty();
        RuleFor(x => x.Name).NotEmpty().MaximumLength(global::Configuration.Domain.Company.MaxNameLength);
    }
}
