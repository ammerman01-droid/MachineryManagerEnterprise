using MachineryManagerEnterprise.Consumption.Application.Features.FuelConsumptions.Dtos;
using MachineryManagerEnterprise.Consumption.Domain;
using Mapster;

namespace MachineryManagerEnterprise.Consumption.Application.Mappings;

/// <summary>Registers Mapster mappings for the Consumption module.</summary>
public sealed class ConsumptionMappingConfig : IRegister
{
    /// <inheritdoc />
    public void Register(TypeAdapterConfig config)
    {
        config.NewConfig<FuelConsumption, FuelConsumptionDto>()
            .Map(dest => dest.Id, src => src.Id.Value)
            .Map(dest => dest.FuelSlot, src => src.FuelSlot.ToString())
            .Map(dest => dest.FuelKind, src => src.FuelKind.ToString())
            .Map(dest => dest.FuelUnit, src => src.FuelUnit.ToString())
            .Map(dest => dest.MeterReadingUnit, src => src.MeterReadingUnit.ToString());
    }
}
