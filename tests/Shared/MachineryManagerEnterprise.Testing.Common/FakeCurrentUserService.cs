using MachineryManagerEnterprise.SharedKernel.Abstractions;

namespace MachineryManagerEnterprise.Testing.Common;

/// <summary>
/// A mutable <see cref="ICurrentUserService"/> for endpoint tests: tests
/// call <see cref="SetAuthenticated"/> / <see cref="SetAnonymous"/> before
/// sending a request to control who the request appears to come from.
/// </summary>
/// <remarks>
/// Registered as a singleton in a module's test host and shared with
/// <see cref="TestAuthHandler"/>, so the ASP.NET Core authentication
/// middleware's view of "is this request authenticated" always agrees
/// with what application/domain code sees through
/// <see cref="ICurrentUserService"/> — there is exactly one source of
/// truth per test, not two independently-configured fakes that could
/// drift out of sync.
/// </remarks>
public sealed class FakeCurrentUserService : ICurrentUserService
{
    /// <inheritdoc />
    public Guid? UserId { get; private set; }

    /// <inheritdoc />
    public Guid? OrganizationId { get; private set; }

    /// <inheritdoc />
    public bool IsAuthenticated => UserId is not null;

    /// <summary>Makes subsequent requests appear to come from the given authenticated user.</summary>
    /// <param name="userId">The user identifier the request should be attributed to.</param>
    /// <param name="organizationId">The tenant/organization boundary for the request, if any.</param>
    public void SetAuthenticated(Guid userId, Guid? organizationId = null)
    {
        UserId = userId;
        OrganizationId = organizationId;
    }

    /// <summary>Makes subsequent requests appear unauthenticated (no bearer token supplied).</summary>
    public void SetAnonymous()
    {
        UserId = null;
        OrganizationId = null;
    }
}
