using FluentValidation;

namespace MachineryManagerEnterprise.Consumption.Application.Features.FuelConsumptions.Queries.GetFuelConsumptionById;

/// <summary>Validates <see cref="GetFuelConsumptionByIdQuery"/> shape-level rules.</summary>
public sealed class GetFuelConsumptionByIdQueryValidator : AbstractValidator<GetFuelConsumptionByIdQuery>
{
    /// <summary>Initializes the validation rules.</summary>
    public GetFuelConsumptionByIdQueryValidator()
    {
        RuleFor(x => x.Id).NotEmpty();
    }
}
