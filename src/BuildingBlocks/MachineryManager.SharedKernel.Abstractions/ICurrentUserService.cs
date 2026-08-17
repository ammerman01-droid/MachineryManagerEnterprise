namespace MachineryManager.SharedKernel.Abstractions;

/// <summary>
/// Provides the Application User Context for the current request.
/// Business logic shall depend only on this abstraction, never on raw
/// JWT claims (07-api/07-AuthenticationAuthorization.md, Section 8).
/// </summary>
public interface ICurrentUserService
{
    /// <summary>The authenticated user's identifier, or null if unauthenticated.</summary>
    Guid? UserId { get; }

    /// <summary>
    /// The Organization (tenant) boundary that every authenticated
    /// request executes within (Section 9, Multi-Tenant Context).
    /// Null only for unauthenticated or platform-level requests.
    /// </summary>
    Guid? OrganizationId { get; }

    /// <summary>Whether the current request is authenticated.</summary>
    bool IsAuthenticated { get; }
}