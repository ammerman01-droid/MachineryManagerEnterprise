using MachineryManager.SharedKernel.Abstractions;

namespace MachineryManager.Organization.Application.Abstractions;

/// <summary>
/// Unit of work for the Organization module. Distinct from every other
/// module's UoW so that each module commits its own aggregates
/// (ADR-0001, Modular Monolith Rules) — mirrors
/// <c>IAdministrationUnitOfWork</c>. Introduced (chat, 2026-08-27) to
/// fix a DI collision: registering the shared <see cref="IUnitOfWork"/>
/// directly from more than one module causes the last module
/// registered in Program.cs to silently win for the entire
/// application, so other modules' SaveChangesAsync calls were
/// resolving to the wrong DbContext and discarding their changes.
/// </summary>
public interface IOrganizationUnitOfWork : IUnitOfWork
{
}