using MachineryManager.Identity.Infrastructure;
using MachineryManager.Identity.Presentation.Endpoints;
using MachineryManager.Organization.Application;
using MachineryManager.Organization.Infrastructure;
using MachineryManager.Organization.Presentation.Endpoints;
using MachineryManager.SharedKernel.Infrastructure;
using MachineryManager.Web.Components;
using MudBlazor.Services;
using OpenTelemetry;
using OpenTelemetry.Metrics;
using OpenTelemetry.Trace;
using Scalar.AspNetCore;
using Serilog;

// Two-stage initialization (per ADR-0009 / ADR-0033): a bootstrap
// logger captures any failure that happens before the host itself has
// finished configuring the real logging pipeline.
Log.Logger = new LoggerConfiguration()
    .WriteTo.Console()
    .CreateBootstrapLogger();

try
{
    var builder = WebApplication.CreateBuilder(args);

    builder.Host.UseSerilog((context, services, configuration) => configuration
        .ReadFrom.Configuration(context.Configuration)
        .ReadFrom.Services(services)
        .Enrich.FromLogContext());

    builder.Services
        .AddOpenTelemetry()
        .WithTracing(tracing => tracing
            .AddAspNetCoreInstrumentation())
        .WithMetrics(metrics => metrics
            .AddAspNetCoreInstrumentation());

    // Add services to the container.
    builder.Services.AddRazorComponents()
        .AddInteractiveServerComponents();

    // UI [TE-0003 (MudBlazor / ADR-0005)]
    builder.Services.AddMudServices();

    // API Documentation [TE-0021 (Scalar & NSwag / ADR-0035)]
    builder.Services.AddOpenApi();

    // Cross-cutting SharedKernel infrastructure (IDateTimeProvider).
    builder.Services.AddSharedKernelInfrastructure();

    // Organization module (Application + Infrastructure).
    builder.Services.AddOrganizationApplication();
    builder.Services.AddOrganizationInfrastructure(builder.Configuration);

    // Identity platform module (ADR-0030).
    builder.Services.AddIdentityInfrastructure(builder.Configuration);
    builder.Services.AddIdentityOpenIddictServer(builder.Environment);

    var app = builder.Build();

    // Configure the HTTP request pipeline.
    if (!app.Environment.IsDevelopment())
    {
        app.UseExceptionHandler("/Error", createScopeForErrors: true);
        // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
        app.UseHsts();
    }
    else
    {
        // API Documentation [TE-0021 (Scalar / ADR-0035)] - interactive docs in Development only.
        app.MapOpenApi();
        app.MapScalarApiReference();
    }

    app.UseSerilogRequestLogging();

    app.UseStatusCodePagesWithReExecute("/not-found", createScopeForStatusCodePages: true);
    app.UseHttpsRedirection();

    app.UseAuthentication();
    app.UseAuthorization();

    app.UseAntiforgery();

    app.MapStaticAssets();
    app.MapRazorComponents<App>()
        .AddInteractiveServerRenderMode();

    // Identity platform module: OpenIddict protocol endpoints (ADR-0030).
    app.MapIdentityConnectEndpoints();

    // Organization module REST endpoints (07-api conventions, Section 8).
    app.MapOrganizationEndpoints();

    app.Run();
}
catch (Exception ex) when (ex is not HostAbortedException)
{
    Log.Fatal(ex, "MachineryManager.Web terminated unexpectedly during startup");
}
finally
{
    Log.CloseAndFlush();
}