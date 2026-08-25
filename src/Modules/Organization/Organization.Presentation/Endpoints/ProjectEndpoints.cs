using MachineryManager.Organization.Application.Features.Projects.Commands.RegisterProject;
using MachineryManager.Organization.Application.Features.Projects.Queries.GetProjectById;
using MachineryManager.Organization.Application.Features.Projects.Queries.SearchProjects;
using MachineryManager.Organization.Presentation.Contracts;
using MediatR;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Routing;
using OpenIddict.Validation.AspNetCore;
using MachineryManager.Organization.Application.Features.Projects.Commands.RenameProject;

namespace MachineryManager.Organization.Presentation.Endpoints;

/// <summary>
/// Maps the Project module's REST endpoints per 07-api conventions.
/// </summary>
public static class ProjectEndpoints
{
    /// <summary>
    /// Registers the Project module's endpoints on the application's route builder.
    /// </summary>
    /// <param name="endpoints">The endpoint route builder.</param>
    /// <returns>The same <see cref="IEndpointRouteBuilder"/> for chaining.</returns>
    public static IEndpointRouteBuilder MapProjectEndpoints(this IEndpointRouteBuilder endpoints)
    {
        var group = endpoints
            .MapGroup("/api/v1/projects")
            .WithTags("Projects")
            .RequireAuthorization(policy => policy
                .AddAuthenticationSchemes(OpenIddictValidationAspNetCoreDefaults.AuthenticationScheme)
                .RequireAuthenticatedUser());

        group.MapPost("/", RegisterProjectAsync)
            .WithName("RegisterProject")
            .WithSummary("Registers a new Project under an Organization.");

                group.MapPut("/{projectId:guid}", RenameProjectAsync)
            .WithName("RenameProject")
            .WithSummary("Renames an existing Project.");

        group.MapGet("/{projectId:guid}", GetProjectByIdAsync)
            .WithName("GetProjectById")
            .WithSummary("Retrieves a single Project by its identifier.");

        group.MapGet("/", SearchProjectsAsync)
            .WithName("SearchProjects")
            .WithSummary("Searches Projects with optional text filtering and pagination.");

        return endpoints;
    }

    private static async Task<IResult> RegisterProjectAsync(
        RegisterProjectRequest request,
        ISender sender,
        HttpContext httpContext,
        CancellationToken cancellationToken)
    {
        var result = await sender.Send(
            new RegisterProjectCommand(request.OrganizationId, request.Name),
            cancellationToken);

        return result.IsSuccess
            ? Results.Created($"/api/v1/projects/{result.Value}", new { id = result.Value })
            : result.ToProblemResult(httpContext);
    }

    private static async Task<IResult> GetProjectByIdAsync(
        Guid projectId,
        ISender sender,
        HttpContext httpContext,
        CancellationToken cancellationToken)
    {
        var result = await sender.Send(new GetProjectByIdQuery(projectId), cancellationToken);

        return result.IsSuccess
            ? Results.Ok(result.Value)
            : result.ToProblemResult(httpContext);
    }

    private static async Task<IResult> SearchProjectsAsync(
        ISender sender,
        HttpContext httpContext,
        CancellationToken cancellationToken,
        string? search = null,
        int page = 1,
        int pageSize = 25)
    {
        var result = await sender.Send(
            new SearchProjectsQuery(search, page, pageSize),
            cancellationToken);

        return result.IsSuccess
            ? Results.Ok(result.Value)
            : result.ToProblemResult(httpContext);
    }

        private static async Task<IResult> RenameProjectAsync(
        Guid projectId,
        RenameProjectRequest request,
        ISender sender,
        HttpContext httpContext,
        CancellationToken cancellationToken)
    {
        var result = await sender.Send(
            new RenameProjectCommand(projectId, request.Name),
            cancellationToken);

        return result.IsSuccess
            ? Results.NoContent()
            : result.ToProblemResult(httpContext);
    }
}