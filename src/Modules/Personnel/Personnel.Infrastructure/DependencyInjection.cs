using MachineryManagerEnterprise.Personnel.Application.Abstractions;
using MachineryManagerEnterprise.Personnel.Infrastructure.Persistence;
using MachineryManagerEnterprise.SharedKernel.Infrastructure;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using MachineryManagerEnterprise.SharedKernel.Abstractions;

namespace MachineryManagerEnterprise.Personnel.Infrastructure;

/// <summary>Registers Personnel Infrastructure layer services into the dependency injection container.</summary>
public static class DependencyInjection
{
    /// <summary>Registers the Personnel <see cref="DbContext"/>, repositories, and Unit of Work.</summary>
    public static IServiceCollection AddPersonnelInfrastructure(
        this IServiceCollection services,
        IConfiguration configuration)
    {
        services.AddDbContext<PersonnelDbContext>((serviceProvider, options) => options
            .UseSqlServer(configuration.GetConnectionString("MachineryManagerDatabase"))
            .AddInterceptors(serviceProvider.GetRequiredService<AuditSaveChangesInterceptor>()));

        services.AddScoped<IPersonnelRepository, PersonnelRepository>();
        services.AddScoped<IPersonnelUnitOfWork>(sp => sp.GetRequiredService<PersonnelDbContext>());
        services.AddScoped<IPersonnelUsageLookupService, PersonnelUsageLookupService>();

        // Cross-module lookup for other modules — currently Consumption
        // (chat, 2026-09-16).
        services.AddScoped<IPersonnelLookupService, PersonnelLookupService>();

        return services;
    }
}
