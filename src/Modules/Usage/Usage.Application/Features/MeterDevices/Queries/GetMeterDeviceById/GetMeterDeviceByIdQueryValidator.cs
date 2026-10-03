using FluentValidation;

namespace MachineryManagerEnterprise.Usage.Application.Features.MeterDevices.Queries.GetMeterDeviceById;

/// <summary>Validates <see cref="GetMeterDeviceByIdQuery"/> per ADR-0036.</summary>
public sealed class GetMeterDeviceByIdQueryValidator : AbstractValidator<GetMeterDeviceByIdQuery>
{
    /// <summary>Initializes validation rules for the get meter device by id query.</summary>
    public GetMeterDeviceByIdQueryValidator()
    {
        RuleFor(x => x.MeterDeviceId).NotEmpty();
    }
}
