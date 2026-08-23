namespace MachineryManager.Identity.Domain;

/// <summary>
/// Reads and caches the current user's access token once (typically
/// during a Razor component's OnInitializedAsync, while a real
/// HttpContext is still reliably available), so later Blazor Server
/// interactive event handlers — which no longer have a real
/// HttpContext — can still access it (chat, 2026-08-22).
/// </summary>
/// <remarks>
/// Defined here (Domain) rather than Infrastructure so Presentation
/// can depend on the abstraction without referencing Infrastructure
/// directly — the same pattern already used for SignInManager/UserManager.
/// </remarks>
public interface ICurrentAccessTokenAccessor
{
    /// <summary>Gets the current user's stored access token, if any.</summary>
    /// <returns>The access token, or <c>null</c> if unavailable.</returns>
    Task<string?> GetAccessTokenAsync();
}