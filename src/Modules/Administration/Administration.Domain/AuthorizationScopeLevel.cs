namespace Administration.Domain;

/// <summary>
/// Defines the hierarchy levels at which a scoped permission or
/// profile assignment may be granted (Section 5.8, Authorization Model).
/// </summary>
public enum AuthorizationScopeLevel
{
    /// <summary>Unrestricted platform-level access.</summary>
    Platform,

    /// <summary>Scoped to all Organizations within a Holding.</summary>
    Holding,

    /// <summary>Scoped to a single Organization.</summary>
    Organization,

    /// <summary>Scoped to a single Project.</summary>
    Project,
}