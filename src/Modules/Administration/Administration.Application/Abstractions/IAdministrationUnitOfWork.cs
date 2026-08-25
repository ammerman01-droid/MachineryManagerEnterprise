using MachineryManager.SharedKernel.Abstractions;

namespace MachineryManager.Administration.Application.Abstractions;

/// <summary>
/// Unit of work for the Administration module. Distinct from the
/// Organization module's UoW so that each module commits its own
/// aggregates (ADR-0001, Modular Monolith Rules).
/// </summary>
public interface IAdministrationUnitOfWork : IUnitOfWork
{
}