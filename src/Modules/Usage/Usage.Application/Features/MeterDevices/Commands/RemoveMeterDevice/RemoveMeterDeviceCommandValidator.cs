using FluentValidation;

namespace MachineryManagerEnterprise.Usage.Application.Features.MeterDevices.Commands.RemoveMeterDevice;

/// <summary>Validates <see cref="RemoveMeterDeviceCommand"/> per ADR-0036.</summary>
public sealed class RemoveMeterDeviceCommandValidator : AbstractValidator<RemoveMeterDeviceCommand>
{
    /// <summary>Initializes validation rules for the remove meter device command.</summary>
    public RemoveMeterDeviceCommandValidator()
    {
        RuleFor(x => x.MeterDeviceId).NotEmpty();
    }
}
