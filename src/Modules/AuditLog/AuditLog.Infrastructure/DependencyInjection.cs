using MachineryManager.AuditLog.Application.Abstractions;
using MachineryManager.AuditLog.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace MachineryManager.AuditLog.Infrastructure;

/// <summary>Registers the AuditLog module's Infrastructure-layer services (chat, 2026-09-05, gam 4).</summary>
public static class DependencyInjection
{
    /// <summary>
    /// Registers the read-only <see cref="AuditLogDbContext"/> and the
    /// audit read repository.
    /// </summary>
    /// <remarks>
    /// Deliberately uses the plain <c>options =&gt;</c> overload (NOT the
    /// (IServiceProvider, DbContextOptionsBuilder) one) and attaches no
    /// <c>AuditSaveChangesInterceptor</c>: this module is read-only and
    /// must never produce audit rows.
    /// </remarks>
    /// <param name="services">The service collection to configure.</param>
    /// <param name="configuration">The application configuration, used to resolve the connection string.</param>
    /// <returns>The same <see cref="IServiceCollection"/> for chaining.</returns>
    public static IServiceCollection AddAuditLogInfrastructure(
        this IServiceCollection services,
        IConfiguration configuration)
    {
        services.AddDbContext<AuditLogDbContext>(options => options
            .UseSqlServer(configuration.GetConnectionString("MachineryManagerDatabase")));

        services.AddScoped<IAuditEntryReadRepository, AuditEntryReadRepository>();

        return services;
    }
}