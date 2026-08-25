using Administration.Domain;
using MachineryManager.Administration.Application.Features.UserProfileAssignments.Commands.ActivateUserProfileAssignment;
using MachineryManager.Administration.Application.Features.UserProfileAssignments.Commands.AssignUserToProfile;
using MachineryManager.Administration.Application.Features.UserProfileAssignments.Commands.DeactivateUserProfileAssignment;
using MachineryManager.Administration.Application.Features.UserProfileAssignments.Commands.DeleteUserProfileAssignment;
using MachineryManager.Administration.Application.Features.UserProfileAssignments.Queries.GetUserProfileAssignmentsByUserId;
using MachineryManager.Administration.Presentation.Contracts;
using MediatR;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Routing;
using OpenIddict.Validation.AspNetCore;

using static OpenIddict.Abstractions.OpenIddictConstants;

namespace MachineryManager.Administration.Presentation.Endpoints;

/// <summary>Maps the UserProfileAssignment module's REST endpoints.</summary>
public static class UserProfileAssignmentEndpoints
{
    /// <summary>Registers the assignment endpoints.</summary>
    /// <param name="endpoints">The endpoint route builder.</param>
    /// <returns>The same <see cref="IEndpointRouteBuilder"/> for chaining.</returns>
    public static IEndpointRouteBuilder MapUserProfileAssignmentEndpoints(this IEndpointRouteBuilder endpoints)
    {
        var group = endpoints
            .MapGroup("/api/v1/user-profile-assignments")
            .WithTags("User Profile Assignments")
            .RequireAuthorization(policy => policy
                .AddAuthenticationSchemes(OpenIddictValidationAspNetCoreDefaults.AuthenticationScheme)
                .RequireAuthenticatedUser());

        group.MapPost("/", AssignUserToProfileAsync)
            .WithName("AssignUserToProfile")
            .WithSummary("Assigns a User to a Profile at a specific authorization scope. Automatically deactivates the user's previously active assignment, if any.")
            // Bootstrap-phase restriction (chat, 2026-08-20) — see the
            // same note on ProfileEndpoints.CreateProfile.
            .RequireAuthorization(policy => policy
                .AddAuthenticationSchemes(OpenIddictValidationAspNetCoreDefaults.AuthenticationScheme)
                .RequireClaim(Claims.Role, "System Administrator"));

        group.MapGet("/", GetAssignmentsByUserIdAsync)
            .WithName("GetUserProfileAssignments")
            .WithSummary("Retrieves every profile assignment for a user (active and inactive).");

        group.MapPost("/{assignmentId:guid}/deactivate", DeactivateUserProfileAssignmentAsync)
            .WithName("DeactivateUserProfileAssignment")
            .WithSummary("Deactivates a User-Profile assignment. The record stays in the user's list and can be reactivated later.")
            .RequireAuthorization(policy => policy
                .AddAuthenticationSchemes(OpenIddictValidationAspNetCoreDefaults.AuthenticationScheme)
                .RequireClaim(Claims.Role, "System Administrator"));

        group.MapPost("/{assignmentId:guid}/activate", ActivateUserProfileAssignmentAsync)
            .WithName("ActivateUserProfileAssignment")
            .WithSummary("(Re)activates a User-Profile assignment, automatically deactivating whichever other assignment currently holds the user's active slot.")
            .RequireAuthorization(policy => policy
                .AddAuthenticationSchemes(OpenIddictValidationAspNetCoreDefaults.AuthenticationScheme)
                .RequireClaim(Claims.Role, "System Administrator"));

        group.MapDelete("/{assignmentId:guid}", DeleteUserProfileAssignmentAsync)
            .WithName("DeleteUserProfileAssignment")
            .WithSummary("Permanently removes a User-Profile assignment from the user's list.")
            .RequireAuthorization(policy => policy
                .AddAuthenticationSchemes(OpenIddictValidationAspNetCoreDefaults.AuthenticationScheme)
                .RequireClaim(Claims.Role, "System Administrator"));

        return endpoints;
    }

    private static async Task<IResult> AssignUserToProfileAsync(
        AssignUserToProfileRequest request,
        ISender sender,
        HttpContext httpContext,
        CancellationToken cancellationToken)
    {
        var scope = request.ScopeLevel switch
        {
            AuthorizationScopeLevel.Platform => AuthorizationScope.Platform(),
            AuthorizationScopeLevel.Holding => AuthorizationScope.ForHolding(request.ScopeHoldingId!.Value),
            AuthorizationScopeLevel.Organization => AuthorizationScope.ForOrganization(request.ScopeOrganizationId!.Value),
            AuthorizationScopeLevel.Project => AuthorizationScope.ForProject(request.ScopeProjectId!.Value),
            _ => throw new ArgumentOutOfRangeException(nameof(request.ScopeLevel))
        };

        var result = await sender.Send(
            new AssignUserToProfileCommand(request.UserId, request.ProfileId, scope),
            cancellationToken);

        return result.IsSuccess
            ? Results.Created($"/api/v1/user-profile-assignments/{result.Value}", new { id = result.Value })
            : result.ToProblemResult(httpContext);
    }

    private static async Task<IResult> GetAssignmentsByUserIdAsync(
        [FromQuery] Guid userId,
        ISender sender,
        HttpContext httpContext,
        CancellationToken cancellationToken)
    {
        var result = await sender.Send(
            new GetUserProfileAssignmentsByUserIdQuery(userId),
            cancellationToken);

        return result.IsSuccess
            ? Results.Ok(result.Value)
            : result.ToProblemResult(httpContext);
    }

    private static async Task<IResult> DeactivateUserProfileAssignmentAsync(
        Guid assignmentId,
        ISender sender,
        HttpContext httpContext,
        CancellationToken cancellationToken)
    {
        var result = await sender.Send(
            new DeactivateUserProfileAssignmentCommand(assignmentId),
            cancellationToken);

        return result.IsSuccess
            ? Results.NoContent()
            : result.ToProblemResult(httpContext);
    }

    private static async Task<IResult> ActivateUserProfileAssignmentAsync(
        Guid assignmentId,
        ISender sender,
        HttpContext httpContext,
        CancellationToken cancellationToken)
    {
        var result = await sender.Send(
            new ActivateUserProfileAssignmentCommand(assignmentId),
            cancellationToken);

        return result.IsSuccess
            ? Results.NoContent()
            : result.ToProblemResult(httpContext);
    }

    private static async Task<IResult> DeleteUserProfileAssignmentAsync(
        Guid assignmentId,
        ISender sender,
        HttpContext httpContext,
        CancellationToken cancellationToken)
    {
        var result = await sender.Send(
            new DeleteUserProfileAssignmentCommand(assignmentId),
            cancellationToken);

        return result.IsSuccess
            ? Results.NoContent()
            : result.ToProblemResult(httpContext);
    }
}