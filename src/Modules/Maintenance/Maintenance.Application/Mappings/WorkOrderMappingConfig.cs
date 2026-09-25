using Mapster;
using MachineryManagerEnterprise.Maintenance.Application.Features.WorkOrders.Dtos;
using MachineryManagerEnterprise.Maintenance.Domain;

namespace MachineryManagerEnterprise.Maintenance.Application.Mappings;

/// <summary>
/// Registers Mapster mapping configuration for the Maintenance module,
/// following the project's standard Mapster convention.
/// </summary>
public sealed class WorkOrderMappingConfig : IRegister
{
    /// <summary>Configures the type-mapping rules used by this module's Query handlers and repositories.</summary>
    /// <param name="config">The Mapster configuration to register mappings into.</param>
    public void Register(TypeAdapterConfig config)
    {
        config.NewConfig<WorkOrder, WorkOrderDto>()
            .Map(dest => dest.Id, src => src.Id.Value)
            .Map(dest => dest.ResultingAssetStatus, src => src.ResultingAssetStatus.ToString())
            .Map(dest => dest.Priority, src => src.Priority.ToString())
            .Map(dest => dest.RepairType, src => src.RepairType.ToString())
            .Map(dest => dest.PredictedRepairLocation, src => src.PredictedRepairLocation.ToString())
            .Map(dest => dest.PartNeedingRepair, src => src.PartNeedingRepair.ToString())
            .Map(dest => dest.Status, src => src.Status.ToString());
    }
}
