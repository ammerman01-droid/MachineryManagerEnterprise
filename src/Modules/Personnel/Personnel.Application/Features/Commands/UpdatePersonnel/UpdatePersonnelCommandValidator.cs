using FluentValidation;

namespace MachineryManagerEnterprise.Personnel.Application.Features.Commands.UpdatePersonnel;

/// <summary>Validates <see cref="UpdatePersonnelCommand"/>.</summary>
public sealed class UpdatePersonnelCommandValidator : AbstractValidator<UpdatePersonnelCommand>
{
    /// <summary>Initializes a new instance of the <see cref="UpdatePersonnelCommandValidator"/> class.</summary>
    public UpdatePersonnelCommandValidator()
    {
        RuleFor(x => x.PersonnelId).NotEmpty();
        RuleFor(x => x.FirstName).NotEmpty().MaximumLength(global::MachineryManagerEnterprise.Personnel.Domain.Personnel.MaxFirstNameLength);
        RuleFor(x => x.LastName).NotEmpty().MaximumLength(global::MachineryManagerEnterprise.Personnel.Domain.Personnel.MaxLastNameLength);
        RuleFor(x => x.PersonnelCode).NotEmpty().MaximumLength(global::MachineryManagerEnterprise.Personnel.Domain.Personnel.MaxPersonnelCodeLength);
        RuleFor(x => x.JobTitleId).NotEmpty();
    }
}