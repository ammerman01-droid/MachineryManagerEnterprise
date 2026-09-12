using MachineryManagerEnterprise.Identity.Infrastructure;
using MachineryManagerEnterprise.Identity.Presentation.Endpoints;
using MachineryManagerEnterprise.Identity.Infrastructure.Persistence;
using MachineryManagerEnterprise.Administration.Application;
using MachineryManagerEnterprise.Administration.Infrastructure;
using MachineryManagerEnterprise.Administration.Presentation.Endpoints;
using MachineryManagerEnterprise.Organization.Application;
using MachineryManagerEnterprise.Organization.Infrastructure;
using MachineryManagerEnterprise.Organization.Presentation.Endpoints;
using MachineryManagerEnterprise.SharedKernel.Infrastructure;
using MachineryManagerEnterprise.Web.Components;
using MudBlazor.Services;
using OpenTelemetry;
using OpenTelemetry.Metrics;
using OpenTelemetry.Trace;
using Scalar.AspNetCore;
using Serilog;
using MachineryManagerEnterprise.Asset.Application;
using MachineryManagerEnterprise.Asset.Infrastructure;
using MachineryManagerEnterprise.Asset.Presentation.Endpoints;
using MachineryManagerEnterprise.Configuration.Infrastructure;
using MachineryManagerEnterprise.Configuration.Presentation.Endpoints;
using MachineryManagerEnterprise.Configuration.Application;
using MachineryManagerEnterprise.AuditLog.Application;
using MachineryManagerEnterprise.AuditLog.Infrastructure;
using MachineryManagerEnterprise.AuditLog.Presentation.Endpoints;
using MachineryManagerEnterprise.UI;
//using MachineryManagerEnterprise.WorkCalendar.Application;

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

    builder.Services.AddRazorComponents()
        .AddInteractiveServerComponents();

    builder.Services.AddMudServices();
    builder.Services.AddOpenApi();
    builder.Services.AddSharedKernelInfrastructure();
    builder.Services.AddMachineryManagerUiTheming();

    builder.Services.AddHttpContextAccessor();
    builder.Services.AddScoped<MachineryManagerEnterprise.SharedKernel.Abstractions.ICurrentUserService, MachineryManagerEnterprise.SharedKernel.Infrastructure.CurrentUserService>();

    // Organization module
    builder.Services.AddOrganizationApplication();
    builder.Services.AddOrganizationInfrastructure(builder.Configuration);

    // Administration module
    builder.Services.AddAdministrationApplication();
    builder.Services.AddAdministrationInfrastructure(builder.Configuration);

    // Asset module
    builder.Services.AddAssetApplication();
    builder.Services.AddAssetInfrastructure(builder.Configuration);

    // Identity platform module
    builder.Services.AddIdentityInfrastructure(builder.Configuration);
    builder.Services.AddIdentityOpenIddictServer(builder.Environment);
    builder.Services.AddIdentityOpenIddictClient(builder.Configuration, builder.Environment);
    builder.Services.AddIdentityInternalApiClient(builder.Configuration);

    // Configuration module
    builder.Services.AddConfigurationApplication();
    builder.Services.AddConfigurationInfrastructure(builder.Configuration);

    // AuditLog module (read-only)
    builder.Services.AddAuditLogApplication();
    builder.Services.AddAuditLogInfrastructure(builder.Configuration);

    // WorkCalendar module
    //builder.Services.AddWorkCalendarApplication();


    var app = builder.Build();

    if (!app.Environment.IsDevelopment())
    {
        app.UseExceptionHandler("/Error", createScopeForErrors: true);
        app.UseHsts();
    }
    else
    {
        app.MapOpenApi();
        app.MapScalarApiReference();
    }

    using (var scope = app.Services.CreateScope())
    {
        await IdentityDataSeeder.SeedAsync(scope.ServiceProvider, app.Environment);
    }

    app.UseSerilogRequestLogging();
    app.UseWhen(
    context => !context.Request.Path.StartsWithSegments("/api"),
    branch => branch.UseStatusCodePagesWithReExecute("/not-found", createScopeForStatusCodePages: true));
    app.UseHttpsRedirection();
    app.UseAuthentication();
    app.UseAuthorization();
    app.UseAntiforgery();

    app.MapStaticAssets();
        app.MapRazorComponents<App>()
    .AddInteractiveServerRenderMode()
    .AddAdditionalAssemblies(
        typeof(MachineryManagerEnterprise.Identity.Presentation.Components.Pages.Login).Assembly,
        typeof(MachineryManagerEnterprise.Administration.Presentation.Components.Pages.ProfilesList).Assembly,
        typeof(MachineryManagerEnterprise.Organization.Presentation.Components.Pages.OrganizationsList).Assembly,
        typeof(MachineryManagerEnterprise.Asset.Presentation.Components.Pages.AssetModelsList).Assembly,
        typeof(MachineryManagerEnterprise.Configuration.Presentation.Components.Pages.ColorsList).Assembly,
        typeof(MachineryManagerEnterprise.AuditLog.Presentation.Components.Pages.AuditLogList).Assembly);

    // Identity endpoints
    app.MapIdentityConnectEndpoints();
    app.MapIdentitySigninCallbackEndpoints();
    app.MapIdentityDevTokenEndpoints(app.Environment);
    app.MapIdentityUserEndpoints();

    // Organization endpoints
    app.MapOrganizationEndpoints();
    app.MapHoldingEndpoints();
    app.MapProjectEndpoints();

    // Administration endpoints
    app.MapProfileEndpoints();
    app.MapUserProfileAssignmentEndpoints();

    // Asset endpoints
    app.MapAssetModelEndpoints();
    app.MapEngineModelEndpoints();
    app.MapAssetEndpoints();

    //Configuration endpoints
    app.MapColorEndpoints();
    app.MapUnitOfMeasurementEndpoints();
    app.MapCompanyEndpoints();
    app.MapFuelTypeEndpoints();

    // AuditLog endpoints
    app.MapAuditLogEndpoints();
    

    app.Run();
}
catch (Exception ex) when (ex is not HostAbortedException)
{
    Log.Fatal(ex, "MachineryManagerEnterprise.Web terminated unexpectedly during startup");
}
finally
{
    Log.CloseAndFlush();
}