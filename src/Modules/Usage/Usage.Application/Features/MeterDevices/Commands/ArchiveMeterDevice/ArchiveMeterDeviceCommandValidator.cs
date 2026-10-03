using FluentValidation;

namespace MachineryManagerEnterprise.Usage.Application.Features.MeterDevices.Commands.ArchiveMeterDevice;

/// <summary>Validates <see cref="ArchiveMeterDeviceCommand"/> per ADR-0036.</summary>
public sealed class ArchiveMeterDeviceCommandValidator : AbstractValidator<ArchiveMeterDeviceCommand>
{
    /// <summary>Initializes validation rules for the archive meter device command.</summary>
    public ArchiveMeterDeviceCommandValidator()
    {
        RuleFor(x => x.MeterDeviceId).NotEmpty();
    }
}
