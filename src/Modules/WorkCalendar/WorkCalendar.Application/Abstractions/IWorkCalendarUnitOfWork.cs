using MachineryManagerEnterprise.SharedKernel.Abstractions;

namespace MachineryManagerEnterprise.WorkCalendar.Application.Abstractions;

/// <summary>
/// Unit of work for the WorkCalendar module. Distinct from every other
/// module's UoW so that each module commits its own aggregates
/// (ADR-0001, Modular Monolith Rules) — mirrors <c>IAssetUnitOfWork</c>.
/// </summary>
public interface IWorkCalendarUnitOfWork : IUnitOfWork
{
}