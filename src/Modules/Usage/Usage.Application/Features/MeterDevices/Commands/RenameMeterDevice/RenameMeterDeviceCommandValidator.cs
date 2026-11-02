using FluentValidation;

namespace MachineryManagerEnterprise.Usage.Application.Features.MeterDevices.Commands.RenameMeterDevice;

/// <summary>Validates <see cref="RenameMeterDeviceCommand"/> per ADR-0036.</summary>
public sealed class RenameMeterDeviceCommandValidator : AbstractValidator<RenameMeterDeviceCommand>
{
    /// <summary>Initializes validation rules for the rename meter device command.</summary>
    public RenameMeterDeviceCommandValidator()
    {
        RuleFor(x => x.MeterDeviceId).NotEmpty();
        RuleFor(x => x.Name).NotEmpty().MaximumLength(global::Usage.Domain.MeterDevice.MaxNameLength);
    }
}
