namespace MachineryManager.SharedKernel.Abstractions;

/// <summary>
/// The full set of scopes a User holds a given Permission over, used to
/// filter list/search results (as opposed to <see cref="IPermissionEvaluator.HasPermissionAsync"/>,
/// which answers a yes/no question about one specific resource).
/// </summary>
/// <param name="IsUnrestricted">
/// True when the user holds this Permission at Platform level — sees
/// everything, no filtering needed.
/// </param>
/// <param name="HoldingIds">Holdings the user has this Permission over (grants visibility of everything beneath them too).</param>
/// <param name="OrganizationIds">Organizations the user has this Permission over directly.</param>
/// <param name="ProjectIds">Projects the user has this Permission over directly.</param>
public sealed record AuthorizedScopeSet(
    bool IsUnrestricted,
    IReadOnlyCollection<Guid> HoldingIds,
    IReadOnlyCollection<Guid> OrganizationIds,
    IReadOnlyCollection<Guid> ProjectIds)
{
    /// <summary>A scope set representing full, unfiltered access (Platform-level).</summary>
    public static AuthorizedScopeSet Unrestricted { get; } =
        new(true, Array.Empty<Guid>(), Array.Empty<Guid>(), Array.Empty<Guid>());

    /// <summary>A scope set representing no access at all.</summary>
    public static AuthorizedScopeSet None { get; } =
        new(false, Array.Empty<Guid>(), Array.Empty<Guid>(), Array.Empty<Guid>());
}