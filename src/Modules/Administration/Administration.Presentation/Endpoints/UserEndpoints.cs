using Administration.Domain;
using MachineryManagerEnterprise.Administration.Application.Features.Access.Queries.GetMyAccess;
using MachineryManagerEnterprise.Administration.Application.Features.Users.Commands.CreateUser;
using MachineryManagerEnterprise.Administration.Application.Features.Users.Commands.SetUserActive;
using MachineryManagerEnterprise.Administration.Application.Features.Users.Queries.GetUserById;
using MachineryManagerEnterprise.Administration.Application.Features.Users.Queries.GetUserRoles;
using MachineryManagerEnterprise.Administration.Application.Features.Users.Queries.SearchUsers;
using MachineryManagerEnterprise.Administration.Presentation.Contracts;
using MediatR;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Routing;
using OpenIddict.Validation.AspNetCore;

namespace MachineryManagerEnterprise.Administration.Presentation.Endpoints;

/// <summary>
/// User management endpoints. They replace the former Identity-module endpoints at the same
/// routes: authorization is no longer an Identity role but the caller's profile permissions
/// (<c>User.*</c>) on the user's scope, plus the hierarchy rules of Section 5.8.
/// </summary>
public static class UserEndpoints
{
    /// <summary>Maps the <c>/api/v1/users</c> endpoints.</summary>
    /// <param name="endpoints">The endpoint route builder.</param>
    /// <returns>The same <see cref="IEndpointRouteBuilder"/> for chaining.</returns>
    public static IEndpointRouteBuilder MapAdministrationUserEndpoints(this IEndpointRouteBuilder endpoints)
    {
        var group = endpoints
            .MapGroup("/api/v1/users")
            .WithTags("Users")
            .RequireAuthorization(policy => policy
                .AddAuthenticationSchemes(OpenIddictValidationAspNetCoreDefaults.AuthenticationScheme)
                .RequireAuthenticatedUser());

        // Registered before "/{userId:guid}" for clarity; "me" is not a Guid, so there is no ambiguity.
        group.MapGet("/me/access", GetMyAccessAsync)
            .WithName("GetMyAccess")
            .WithSummary("Returns the signed-in user's level, position and effective permissions (used by the UI).");

        group.MapGet("/", SearchUsersAsync)
            .WithName("SearchUsers")
            .WithSummary("Searches the users the caller may see, with optional text filtering and pagination.");

        group.MapPost("/", CreateUserAsync)
            .WithName("CreateUser")
            .WithSummary("Creates a user and, when a scope is given, gives it a home and its first profile assignment atomically.");

        group.MapGet("/{userId:guid}", GetUserByIdAsync)
            .WithName("GetUserById")
            .WithSummary("Retrieves a single user by identifier.");

        group.MapGet("/{userId:guid}/roles", GetUserRolesAsync)
            .WithName("GetUserRoles")
            .WithSummary("Retrieves roles assigned to a user.");

        group.MapPost("/{userId:guid}/deactivate", DeactivateUserAsync)
            .WithName("DeactivateUser")
            .WithSummary("Locks out a user account and ends its sessions.");

        group.MapPost("/{userId:guid}/activate", ActivateUserAsync)
            .WithName("ActivateUser")
            .WithSummary("Reactivates a locked-out user account.");

        return endpoints;
    }

    private static async Task<IResult> GetMyAccessAsync(
        ISender sender,
        HttpContext httpContext,
        CancellationToken cancellationToken)
    {
        var result = await sender.Send(new GetMyAccessQuery(), cancellationToken);

        return result.IsSuccess ? Results.Ok(result.Value) : result.ToProblemResult(httpContext);
    }

    private static async Task<IResult> SearchUsersAsync(
        ISender sender,
        HttpContext httpContext,
        CancellationToken cancellationToken,
        string? search = null,
        int page = 1,
        int pageSize = 25)
    {
        var result = await sender.Send(new SearchUsersQuery(search, page, pageSize), cancellationToken);

        return result.IsSuccess ? Results.Ok(result.Value) : result.ToProblemResult(httpContext);
    }

    private static async Task<IResult> CreateUserAsync(
        CreateUserRequest request,
        ISender sender,
        HttpContext httpContext,
        CancellationToken cancellationToken)
    {
        AuthorizationScope? scope = null;

        if (request.ScopeLevel is { } level)
        {
            scope = level switch
            {
                AuthorizationScopeLevel.Platform => AuthorizationScope.Platform(),
                AuthorizationScopeLevel.Holding when request.ScopeHoldingId is { } holdingId =>
                    AuthorizationScope.ForHolding(holdingId),
                AuthorizationScopeLevel.Organization when request.ScopeOrganizationId is { } organizationId =>
                    AuthorizationScope.ForOrganization(organizationId),
                AuthorizationScopeLevel.Project when request.ScopeProjectId is { } projectId =>
                    AuthorizationScope.ForProject(projectId),
                _ => null,
            };

            if (scope is null)
            {
                return Results.BadRequest(new
                {
                    error = "User.ScopeInvalid",
                    message = $"The id of the {level} node is required for scope level {level}.",
                });
            }
        }

        var result = await sender.Send(
            new CreateUserCommand(request.UserName, request.Password, scope, request.ProfileId, request.Roles),
            cancellationToken);

        return result.IsSuccess
            ? Results.Created($"/api/v1/users/{result.Value}", new { id = result.Value, userName = request.UserName })
            : result.ToProblemResult(httpContext);
    }

    private static async Task<IResult> GetUserByIdAsync(
        Guid userId,
        ISender sender,
        HttpContext httpContext,
        CancellationToken cancellationToken)
    {
        var result = await sender.Send(new GetUserByIdQuery(userId), cancellationToken);

        return result.IsSuccess ? Results.Ok(result.Value) : result.ToProblemResult(httpContext);
    }

    private static async Task<IResult> GetUserRolesAsync(
        Guid userId,
        ISender sender,
        HttpContext httpContext,
        CancellationToken cancellationToken)
    {
        var result = await sender.Send(new GetUserRolesQuery(userId), cancellationToken);

        return result.IsSuccess ? Results.Ok(result.Value) : result.ToProblemResult(httpContext);
    }

    private static async Task<IResult> DeactivateUserAsync(
        Guid userId,
        ISender sender,
        HttpContext httpContext,
        CancellationToken cancellationToken)
    {
        var result = await sender.Send(new SetUserActiveCommand(userId, false), cancellationToken);

        return result.IsSuccess ? Results.NoContent() : result.ToProblemResult(httpContext);
    }

    private static async Task<IResult> ActivateUserAsync(
        Guid userId,
        ISender sender,
        HttpContext httpContext,
        CancellationToken cancellationToken)
    {
        var result = await sender.Send(new SetUserActiveCommand(userId, true), cancellationToken);

        return result.IsSuccess ? Results.NoContent() : result.ToProblemResult(httpContext);
    }
}
