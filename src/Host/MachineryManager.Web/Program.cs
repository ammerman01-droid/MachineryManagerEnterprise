using MachineryManager.Identity.Infrastructure;
using MachineryManager.Identity.Presentation.Endpoints;
using MachineryManager.Identity.Infrastructure.Persistence;
using MachineryManager.Administration.Application;
using MachineryManager.Administration.Infrastructure;
using MachineryManager.Administration.Presentation.Endpoints;
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
using MachineryManager.Asset.Application;
using MachineryManager.Asset.Infrastructure;
using MachineryManager.Asset.Presentation.Endpoints;

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

    builder.Services.AddHttpContextAccessor();
    builder.Services.AddScoped<MachineryManager.SharedKernel.Abstractions.ICurrentUserService, MachineryManager.SharedKernel.Infrastructure.CurrentUserService>();

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
            typeof(MachineryManager.Identity.Presentation.Components.Pages.Login).Assembly,
            typeof(MachineryManager.Administration.Presentation.Components.Pages.ProfilesList).Assembly,
            typeof(MachineryManager.Organization.Presentation.Components.Pages.OrganizationsList).Assembly,
            typeof(MachineryManager.Asset.Presentation.Components.Pages.AssetModelsList).Assembly);

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

    //General endpoints
    app.MapColorEndpoints();
    app.MapUnitOfMeasurementEndpoints();

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