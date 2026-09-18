using FluentValidation;

namespace MachineryManagerEnterprise.Configuration.Application.Features.DrivingLicenseTypes.Commands.RenameDrivingLicenseType;

/// <summary>Validates <see cref="RenameDrivingLicenseTypeCommand"/>.</summary>
public sealed class RenameDrivingLicenseTypeCommandValidator : AbstractValidator<RenameDrivingLicenseTypeCommand>
{
    /// <summary>Initializes a new instance of the <see cref="RenameDrivingLicenseTypeCommandValidator"/> class.</summary>
    public RenameDrivingLicenseTypeCommandValidator()
    {
        RuleFor(x => x.DrivingLicenseTypeId).NotEmpty();
        RuleFor(x => x.Name).NotEmpty().MaximumLength(global::Configuration.Domain.DrivingLicenseType.MaxNameLength);
    }
}