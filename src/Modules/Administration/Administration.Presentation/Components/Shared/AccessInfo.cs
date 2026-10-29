namespace MachineryManagerEnterprise.Administration.Presentation.Components.Shared;

/// <summary>
/// The signed-in user's level, position and permissions as reported by
/// <c>GET /api/v1/users/me/access</c>. The UI uses it only to show or hide controls; the server
/// enforces every rule again on each request.
/// </summary>
public sealed class AccessInfo
{
    /// <summary>Gets or sets the level: Platform, Holding, Organization or Project.</summary>
    public string Level { get; set; } = string.Empty;

    /// <summary>Gets or sets the caller's Holding (null for Platform).</summary>
    public Guid? HoldingId { get; set; }

    /// <summary>Gets or sets the caller's Organization.</summary>
    public Guid? OrganizationId { get; set; }

    /// <summary>Gets or sets the caller's Project.</summary>
    public Guid? ProjectId { get; set; }

    /// <summary>Gets or sets whether the caller holds every permission within their scope.</summary>
    public bool HasWildcard { get; set; }

    /// <summary>Gets or sets the explicitly granted permissions.</summary>
    public List<string> Permissions { get; set; } = new();

    /// <summary>Gets whether the caller is a Platform user.</summary>
    public bool IsPlatform => Level == "Platform";

    /// <summary>Gets whether the caller may define and change profiles (Platform and Holding levels only).</summary>
    public bool CanManageProfiles => Level is "Platform" or "Holding";

    /// <summary>Gets the levels at which the caller may create users (own level for Platform/Holding, strictly below for Organization).</summary>
    public IReadOnlyList<string> ManageableLevels => Level switch
    {
        "Platform" => new[] { "Platform", "Holding", "Organization", "Project" },
        "Holding" => new[] { "Holding", "Organization", "Project" },
        "Organization" => new[] { "Project" },
        _ => Array.Empty<string>(),
    };

    /// <summary>Returns whether the caller holds <paramref name="permission"/>.</summary>
    /// <param name="permission">The permission string, e.g. "Profile.View".</param>
    /// <returns><see langword="true"/> when held.</returns>
    public bool Has(string permission) => HasWildcard || Permissions.Contains(permission);
}
