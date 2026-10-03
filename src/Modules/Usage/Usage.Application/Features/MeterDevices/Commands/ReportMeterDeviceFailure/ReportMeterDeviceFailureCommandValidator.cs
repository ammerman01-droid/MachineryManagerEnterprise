using FluentValidation;

namespace MachineryManagerEnterprise.Usage.Application.Features.MeterDevices.Commands.ReportMeterDeviceFailure;

/// <summary>Validates <see cref="ReportMeterDeviceFailureCommand"/> per ADR-0036.</summary>
public sealed class ReportMeterDeviceFailureCommandValidator : AbstractValidator<ReportMeterDeviceFailureCommand>
{
    /// <summary>Initializes validation rules for the report meter device failure command.</summary>
    public ReportMeterDeviceFailureCommandValidator()
    {
        RuleFor(x => x.MeterDeviceId).NotEmpty();
    }
}
