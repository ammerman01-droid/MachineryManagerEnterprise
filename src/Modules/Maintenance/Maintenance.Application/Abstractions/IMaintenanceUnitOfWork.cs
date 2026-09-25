using MachineryManagerEnterprise.SharedKernel.Abstractions;

namespace MachineryManagerEnterprise.Maintenance.Application.Abstractions;

/// <summary>
/// Marker Unit of Work for the Maintenance module, registered instead of
/// the shared <see cref="IUnitOfWork"/> directly — mirrors
/// IAssetUnitOfWork/IPersonnelUnitOfWork (chat, 2026-08-27 fix) so this
/// module's DbContext does not collide with other modules' DI
/// registrations for the shared interface.
/// </summary>
public interface IMaintenanceUnitOfWork : IUnitOfWork
{
}
