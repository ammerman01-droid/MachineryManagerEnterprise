using FluentValidation;

namespace MachineryManagerEnterprise.Personnel.Application.Features.Commands.RegisterPersonnel;

/// <summary>Validates <see cref="RegisterPersonnelCommand"/>.</summary>
public sealed class RegisterPersonnelCommandValidator : AbstractValidator<RegisterPersonnelCommand>
{
    /// <summary>Initializes a new instance of the <see cref="RegisterPersonnelCommandValidator"/> class.</summary>
    public RegisterPersonnelCommandValidator()
    {
        RuleFor(x => x.OrganizationId).NotEmpty();
        RuleFor(x => x.ProjectId).NotEmpty();
        RuleFor(x => x.FirstName).NotEmpty().MaximumLength(global::MachineryManagerEnterprise.Personnel.Domain.Personnel.MaxFirstNameLength);
        RuleFor(x => x.LastName).NotEmpty().MaximumLength(global::MachineryManagerEnterprise.Personnel.Domain.Personnel.MaxLastNameLength);
        RuleFor(x => x.PersonnelCode).NotEmpty().MaximumLength(global::MachineryManagerEnterprise.Personnel.Domain.Personnel.MaxPersonnelCodeLength);
        RuleFor(x => x.JobTitleId).NotEmpty();

        RuleForEach(x => x.DrivingLicenses).ChildRules(license =>
        {
            license.RuleFor(l => l.DrivingLicenseTypeId).NotEmpty();
            license.RuleFor(l => l.ExpiryDate).NotEqual(default(DateOnly));
        });
    }
}