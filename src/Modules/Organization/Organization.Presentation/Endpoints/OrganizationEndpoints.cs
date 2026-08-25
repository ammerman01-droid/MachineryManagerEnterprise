using MachineryManager.Organization.Application.Features.Organizations.Commands.AssignOrganizationToHolding;
using MachineryManager.Organization.Application.Features.Organizations.Commands.ReactivateOrganization;
using MachineryManager.Organization.Application.Features.Organizations.Commands.RegisterOrganization;
using MachineryManager.Organization.Application.Features.Organizations.Commands.SuspendOrganization;
using MachineryManager.Organization.Application.Features.Organizations.Queries.GetOrganizationById;
using MachineryManager.Organization.Application.Features.Organizations.Queries.SearchOrganizations;
using MachineryManager.Organization.Presentation.Contracts;
using MediatR;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Routing;
using OpenIddict.Validation.AspNetCore;
using MachineryManager.Organization.Application.Features.Organizations.Commands.RenameOrganization;

namespace MachineryManager.Organization.Presentation.Endpoints;

/// <summary>
/// Maps the Organization module's REST endpoints per 07-api conventions
/// (Section 8): base path <c>/api/v1/organizations</c>.
/// </summary>
public static class OrganizationEndpoints
{
    /// <summary>Registers the Organization module's endpoints on the application's route builder.</summary>
    /// <param name="endpoints">The endpoint route builder.</param>
    /// <returns>The same <see cref="IEndpointRouteBuilder"/> for chaining.</returns>
    public static IEndpointRouteBuilder MapOrganizationEndpoints(this IEndpointRouteBuilder endpoints)
    {
        var group = endpoints
            .MapGroup("/api/v1/organizations")
            .WithTags("Organizations")
            // ADR-0030: every Organization endpoint requires a valid
            // Bearer access token issued by this app's own OpenIddict
            // server. Role/permission-scoped restrictions per action
            // are enforced inside each command handler via
            // IPermissionEvaluator, not at the endpoint level.
            .RequireAuthorization(policy => policy
                .AddAuthenticationSchemes(OpenIddictValidationAspNetCoreDefaults.AuthenticationScheme)
                .RequireAuthenticatedUser());

        group.MapPost("/", RegisterOrganizationAsync)
            .WithName("RegisterOrganization")
            .WithSummary("Registers a new Organization.");

        group.MapPut("/{organizationId:guid}", RenameOrganizationAsync)
            .WithName("RenameOrganization")
            .WithSummary("Renames an existing Organization.");

        group.MapGet("/{organizationId:guid}", GetOrganizationByIdAsync)
            .WithName("GetOrganizationById")
            .WithSummary("Retrieves a single Organization by its identifier.");

        group.MapGet("/", SearchOrganizationsAsync)
            .WithName("SearchOrganizations")
            .WithSummary("Searches Organizations with optional text filtering and pagination.");

        group.MapPost("/{organizationId:guid}/assign-to-holding", AssignOrganizationToHoldingAsync)
            .WithName("AssignOrganizationToHolding")
            .WithSummary("Assigns an Organization to a Holding.");

        group.MapPost("/{organizationId:guid}/suspend", SuspendOrganizationAsync)
            .WithName("SuspendOrganization")
            .WithSummary("Suspends an Organization (BR-017, Section 10.16). Historical records remain intact.");

        group.MapPost("/{organizationId:guid}/reactivate", ReactivateOrganizationAsync)
            .WithName("ReactivateOrganization")
            .WithSummary("Reactivates a previously suspended Organization.");

        return endpoints;
    }

    private static async Task<IResult> RegisterOrganizationAsync(
        RegisterOrganizationRequest request,
        ISender sender,
        HttpContext httpContext,
        CancellationToken cancellationToken)
    {
        var result = await sender.Send(new RegisterOrganizationCommand(request.Name), cancellationToken);

        return result.IsSuccess
            ? Results.Created($"/api/v1/organizations/{result.Value}", new { id = result.Value })
            : result.ToProblemResult(httpContext);
    }

    private static async Task<IResult> GetOrganizationByIdAsync(
        Guid organizationId,
        ISender sender,
        HttpContext httpContext,
        CancellationToken cancellationToken)
    {
        var result = await sender.Send(new GetOrganizationByIdQuery(organizationId), cancellationToken);

        return result.IsSuccess
            ? Results.Ok(result.Value)
            : result.ToProblemResult(httpContext);
    }

    private static async Task<IResult> SearchOrganizationsAsync(
        ISender sender,
        HttpContext httpContext,
        CancellationToken cancellationToken,
        string? search = null,
        int page = 1,
        int pageSize = 25)
    {
        var result = await sender.Send(
            new SearchOrganizationsQuery(search, page, pageSize),
            cancellationToken);

        return result.IsSuccess
            ? Results.Ok(result.Value)
            : result.ToProblemResult(httpContext);
    }

    private static async Task<IResult> AssignOrganizationToHoldingAsync(
        Guid organizationId,
        AssignOrganizationToHoldingRequest request,
        ISender sender,
        HttpContext httpContext,
        CancellationToken cancellationToken)
    {
        var result = await sender.Send(
            new AssignOrganizationToHoldingCommand(organizationId, request.HoldingId),
            cancellationToken);

        return result.IsSuccess
            ? Results.NoContent()
            : result.ToProblemResult(httpContext);
    }

    private static async Task<IResult> SuspendOrganizationAsync(
        Guid organizationId,
        ISender sender,
        HttpContext httpContext,
        CancellationToken cancellationToken)
    {
        var result = await sender.Send(new SuspendOrganizationCommand(organizationId), cancellationToken);

        return result.IsSuccess
            ? Results.NoContent()
            : result.ToProblemResult(httpContext);
    }

    private static async Task<IResult> ReactivateOrganizationAsync(
        Guid organizationId,
        ISender sender,
        HttpContext httpContext,
        CancellationToken cancellationToken)
    {
        var result = await sender.Send(new ReactivateOrganizationCommand(organizationId), cancellationToken);

        return result.IsSuccess
            ? Results.NoContent()
            : result.ToProblemResult(httpContext);
    }

        private static async Task<IResult> RenameOrganizationAsync(
        Guid organizationId,
        RenameOrganizationRequest request,
        ISender sender,
        HttpContext httpContext,
        CancellationToken cancellationToken)
    {
        var result = await sender.Send(
            new RenameOrganizationCommand(organizationId, request.Name),
            cancellationToken);

        return result.IsSuccess
            ? Results.NoContent()
            : result.ToProblemResult(httpContext);
    }
}