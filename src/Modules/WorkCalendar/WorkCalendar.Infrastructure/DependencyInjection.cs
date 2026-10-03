using MachineryManagerEnterprise.SharedKernel.Abstractions;
using MachineryManagerEnterprise.WorkCalendar.Application.Abstractions;
using MachineryManagerEnterprise.WorkCalendar.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace MachineryManagerEnterprise.WorkCalendar.Infrastructure;

/// <summary>Registers the WorkCalendar module's Infrastructure-layer services.</summary>
public static class DependencyInjection
{
    /// <summary>
    /// Registers the WorkCalendar module's DbContext, repository,
    /// Unit of Work, and cross-module lookup services.
    /// </summary>
    /// <param name="services">The service collection to configure.</param>
    /// <param name="configuration">The application configuration, used to resolve the connection string.</param>
    /// <returns>The same <see cref="IServiceCollection"/> for chaining.</returns>
    public static IServiceCollection AddWorkCalendarInfrastructure(
        this IServiceCollection services,
        IConfiguration configuration)
    {
        services.AddDbContext<WorkCalendarDbContext>(options => options
            .UseSqlServer(
                configuration.GetConnectionString("MachineryManagerDatabase"),
                sqlServerOptions => sqlServerOptions.MigrationsHistoryTable(
                    "__EFMigrationsHistory",
                    schema: "workcalendar")));

        services.AddScoped<IWorkCalendarRepository, WorkCalendarRepository>();
        services.AddScoped<IWorkCalendarUnitOfWork>(
            sp => sp.GetRequiredService<WorkCalendarDbContext>());

        services.AddScoped<IWorkCalendarLookupService, WorkCalendarLookupService>();

        return services;
    }
}