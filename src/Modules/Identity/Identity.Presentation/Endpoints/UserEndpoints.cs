using MachineryManager.Identity.Domain;
using MachineryManager.Identity.Presentation.Contracts;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Routing;
using OpenIddict.Validation.AspNetCore;

using static OpenIddict.Abstractions.OpenIddictConstants;

namespace MachineryManager.Identity.Presentation.Endpoints;

/// <summary>Maps user management endpoints for the Identity module.</summary>
public static class UserEndpoints
{
    /// <summary>Registers the Identity module's user management endpoints.</summary>
    /// <param name="endpoints">The endpoint route builder.</param>
    /// <returns>The same <see cref="IEndpointRouteBuilder"/> for chaining.</returns>
    public static IEndpointRouteBuilder MapIdentityUserEndpoints(this IEndpointRouteBuilder endpoints)
    {
        var group = endpoints
            .MapGroup("/api/v1/users")
            .WithTags("Users")
            .RequireAuthorization(policy => policy
                .AddAuthenticationSchemes(OpenIddictValidationAspNetCoreDefaults.AuthenticationScheme)
                .RequireAuthenticatedUser());

        group.MapGet("/", SearchUsersAsync)
            .WithName("SearchUsers")
            .WithSummary("Searches users with optional text filtering and pagination.");

        group.MapPost("/", CreateUserAsync)
            .WithName("CreateUser")
            .WithSummary("Creates a new user account.")
            .RequireAuthorization(policy => policy
                .AddAuthenticationSchemes(OpenIddictValidationAspNetCoreDefaults.AuthenticationScheme)
                .RequireClaim(Claims.Role, "System Administrator"));

        group.MapGet("/{userId:guid}", GetUserByIdAsync)
            .WithName("GetUserById")
            .WithSummary("Retrieves a single user by identifier.");

        group.MapGet("/{userId:guid}/roles", GetUserRolesAsync)
            .WithName("GetUserRoles")
            .WithSummary("Retrieves roles assigned to a user.");

        group.MapPost("/{userId:guid}/deactivate", DeactivateUserAsync)
            .WithName("DeactivateUser")
            .WithSummary("Locks out a user account.")
            .RequireAuthorization(policy => policy
                .AddAuthenticationSchemes(OpenIddictValidationAspNetCoreDefaults.AuthenticationScheme)
                .RequireClaim(Claims.Role, "System Administrator"));

        return endpoints;
    }

    private static IResult SearchUsersAsync(
        UserManager<ApplicationUser> userManager,
        string? search = null,
        int page = 1,
        int pageSize = 25)
    {
        var allUsers = userManager.Users.ToList();

        if (!string.IsNullOrWhiteSpace(search))
        {
            allUsers = allUsers.Where(u => u.UserName != null && u.UserName.Contains(search, StringComparison.OrdinalIgnoreCase)).ToList();
        }

        var totalItems = allUsers.Count;
        var items = allUsers
            .Skip((page - 1) * pageSize)
            .Take(pageSize)
            .Select(u => new UserDto(u.Id, u.UserName ?? string.Empty))
            .ToList();

        var totalPages = totalItems == 0 ? 0 : (int)Math.Ceiling(totalItems / (double)pageSize);

        return Results.Ok(new
        {
            items,
            page,
            pageSize,
            totalItems,
            totalPages,
            hasNextPage = page < totalPages,
            hasPreviousPage = page > 1
        });
    }

    private static async Task<IResult> CreateUserAsync(
        CreateUserRequest request,
        UserManager<ApplicationUser> userManager)
    {
        var user = new ApplicationUser { UserName = request.UserName };
        var result = await userManager.CreateAsync(user, request.Password);

        if (!result.Succeeded)
        {
            var errors = string.Join("; ", result.Errors.Select(e => e.Description));
            return Results.BadRequest(new { error = "User.CreateFailed", message = errors });
        }

        if (request.Roles?.Any() == true)
        {
            var validRoles = request.Roles.Where(r => StandardRoles.All.Contains(r)).ToArray();
            if (validRoles.Any())
            {
                await userManager.AddToRolesAsync(user, validRoles);
            }
        }

        return Results.Created($"/api/v1/users/{user.Id}", new UserDto(user.Id, user.UserName ?? string.Empty));
    }

    private static async Task<IResult> GetUserByIdAsync(
        Guid userId,
        UserManager<ApplicationUser> userManager)
    {
        var user = await userManager.FindByIdAsync(userId.ToString());
        if (user is null)
        {
            return Results.NotFound(new { error = "User.NotFound", message = $"User with id {userId} was not found." });
        }

        return Results.Ok(new UserDto(user.Id, user.UserName ?? string.Empty));
    }

    private static async Task<IResult> GetUserRolesAsync(
        Guid userId,
        UserManager<ApplicationUser> userManager)
    {
        var user = await userManager.FindByIdAsync(userId.ToString());
        if (user is null)
        {
            return Results.NotFound(new { error = "User.NotFound", message = $"User with id {userId} was not found." });
        }

        var roles = await userManager.GetRolesAsync(user);
        return Results.Ok(roles);
    }

    private static async Task<IResult> DeactivateUserAsync(
        Guid userId,
        UserManager<ApplicationUser> userManager)
    {
        var user = await userManager.FindByIdAsync(userId.ToString());
        if (user is null)
        {
            return Results.NotFound(new { error = "User.NotFound", message = $"User with id {userId} was not found." });
        }

        var result = await userManager.SetLockoutEndDateAsync(user, DateTimeOffset.UtcNow.AddYears(100));
        if (!result.Succeeded)
        {
            var errors = string.Join("; ", result.Errors.Select(e => e.Description));
            return Results.BadRequest(new { error = "User.DeactivateFailed", message = errors });
        }

        return Results.NoContent();
    }
}