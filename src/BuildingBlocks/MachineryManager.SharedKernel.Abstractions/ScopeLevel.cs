namespace MachineryManager.SharedKernel.Abstractions;

/// <summary>
/// The four authorization levels of the tenant hierarchy
/// (05-application, Section 5.8): Platform → Holding → Organization → Project.
/// </summary>
public enum ScopeLevel
{
    /// <summary>Unrestricted access across the entire platform.</summary>
    Platform = 0,

    /// <summary>Scoped to a single Holding and everything beneath it.</summary>
    Holding = 1,

    /// <summary>Scoped to a single Organization and everything beneath it.</summary>
    Organization = 2,

    /// <summary>Scoped to a single Project.</summary>
    Project = 3,
}