using System.Security.Claims;
using MachineryManager.SharedKernel.Abstractions;
using Microsoft.AspNetCore.Http;

namespace MachineryManager.SharedKernel.Infrastructure;

/// <summary>
/// Provides the current user context from the ASP.NET Core HttpContext,
/// implementing the <see cref="ICurrentUserService"/> abstraction.
/// </summary>
public sealed class CurrentUserService : ICurrentUserService
{
    private readonly IHttpContextAccessor _httpContextAccessor;

    /// <summary>
    /// Initializes a new instance of the <see cref="CurrentUserService"/> class.
    /// </summary>
    /// <param name="httpContextAccessor">The HTTP context accessor.</param>
    public CurrentUserService(IHttpContextAccessor httpContextAccessor)
    {
        _httpContextAccessor = httpContextAccessor;
    }

    /// <inheritdoc />
    public Guid? UserId
    {
        get
        {
            var userIdClaim = _httpContextAccessor.HttpContext?.User?.FindFirstValue(ClaimTypes.NameIdentifier)
                ?? _httpContextAccessor.HttpContext?.User?.FindFirstValue("sub");
            return Guid.TryParse(userIdClaim, out var userId) ? userId : null;
        }
    }

    /// <inheritdoc />
    public Guid? OrganizationId
    {
        get
        {
            // The Organization claim is not yet issued by the Identity module
            // (ADR-0030: Identity NEVER owns Organization data). Tenant
            // resolution will be added when the Administration module
            // implements scoped assignment. Returns null until then.
            var orgClaim = _httpContextAccessor.HttpContext?.User?.FindFirstValue("organization_id");
            return Guid.TryParse(orgClaim, out var orgId) ? orgId : null;
        }
    }

    /// <inheritdoc />
    public bool IsAuthenticated => _httpContextAccessor.HttpContext?.User?.Identity?.IsAuthenticated ?? false;
}