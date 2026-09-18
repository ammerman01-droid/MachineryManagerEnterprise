using FluentValidation;

namespace MachineryManagerEnterprise.Configuration.Application.Features.DrivingLicenseTypes.Commands.RegisterDrivingLicenseType;

/// <summary>Validates <see cref="RegisterDrivingLicenseTypeCommand"/>.</summary>
public sealed class RegisterDrivingLicenseTypeCommandValidator : AbstractValidator<RegisterDrivingLicenseTypeCommand>
{
    /// <summary>Initializes a new instance of the <see cref="RegisterDrivingLicenseTypeCommandValidator"/> class.</summary>
    public RegisterDrivingLicenseTypeCommandValidator()
    {
        RuleFor(x => x.HoldingId).NotEmpty();
        RuleFor(x => x.Name).NotEmpty().MaximumLength(global::Configuration.Domain.DrivingLicenseType.MaxNameLength);
    }
}