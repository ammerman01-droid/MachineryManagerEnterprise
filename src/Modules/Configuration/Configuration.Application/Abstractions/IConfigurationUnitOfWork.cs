using MachineryManager.SharedKernel.Abstractions;

namespace MachineryManager.Configuration.Application.Abstractions;

/// <summary>
/// Module-specific Unit of Work for Configuration (per the mandatory
/// pattern established after the DI-collision bug with a shared
/// IUnitOfWork registration — chat, 2026-08-27).
/// </summary>
public interface IConfigurationUnitOfWork : IUnitOfWork
{
}