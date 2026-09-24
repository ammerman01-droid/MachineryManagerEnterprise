using MachineryManagerEnterprise.SharedKernel.Abstractions;

namespace MachineryManagerEnterprise.Consumption.Application.Abstractions;

/// <summary>
/// Unit of work for the Consumption module. Distinct from every other
/// module's UoW so that each module commits its own aggregates
/// (ADR-0001, Modular Monolith Rules) — mirrors
/// <c>IAssetUnitOfWork</c>/<c>IPersonnelUnitOfWork</c>.
/// </summary>
public interface IConsumptionUnitOfWork : IUnitOfWork
{
}
