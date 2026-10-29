using Administration.Domain.Events;
using MachineryManagerEnterprise.SharedKernel;
using MachineryManagerEnterprise.SharedKernel.Abstractions;

namespace Administration.Domain;

/// <summary>
/// Profile: a named, reusable bundle of Permissions (Section 5.8).
/// A Profile does NOT carry a scope — scope is assigned separately
/// per User via <see cref="UserProfileAssignment"/>.
/// </summary>
public sealed class Profile : AggregateRoot<ProfileId>
{
    /// <summary>The maximum allowed length for a profile name.</summary>
    public const int MaxNameLength = 200;

    /// <summary>The wildcard permission that grants every permission within an assignment's scope.</summary>
    public const string WildcardPermission = "*";

    /// <summary>The display name of the system-managed full-access profile of a Holding.</summary>
    public const string SystemFullAccessName = "دسترسی کامل هلدینگ";

    /// <summary>Gets the name of the profile.</summary>
    public string Name { get; private set; }

    private readonly List<string> _permissions = [];

    /// <summary>Gets the permissions contained in this profile.</summary>
    public IReadOnlyCollection<string> Permissions => _permissions.AsReadOnly();

    /// <summary>Gets whether this profile is currently active.</summary>
    public bool IsActive { get; private set; }

    /// <summary>Gets the UTC timestamp when the profile was created.</summary>
    public DateTimeOffset CreatedAt { get; private set; }

    /// <summary>
    /// Gets the Holding that owns this profile, or <see langword="null"/> for a
    /// Platform-owned profile. Ownership is fixed at creation and never changes;
    /// a Holding sees and manages only its own profiles.
    /// </summary>
    public Guid? HoldingId { get; private set; }

    /// <summary>
    /// Gets whether this is a system-managed profile (the full-access profile created
    /// for a Holding's first user). System profiles can never be edited, deactivated
    /// or deleted.
    /// </summary>
    public bool IsSystem { get; private set; }

    // Reserved for EF Core materialization only.
    private Profile()
    {
        Name = string.Empty;
    }

    private Profile(ProfileId id, string name, DateTimeOffset createdAt, Guid? holdingId, bool isSystem)
        : base(id)
    {
        Name = name;
        IsActive = true;
        CreatedAt = createdAt;
        HoldingId = holdingId;
        IsSystem = isSystem;
    }

    /// <summary>
    /// Creates a new Profile. This is the only way a user-defined Profile comes into existence.
    /// </summary>
    /// <param name="name">The profile's display name.</param>
    /// <param name="permissions">The initial set of permissions.</param>
    /// <param name="dateTimeProvider">Provider for deterministic UTC timestamps.</param>
    /// <param name="holdingId">
    /// The owning Holding, or <see langword="null"/> for a Platform-owned profile.
    /// A Holding-owned profile may not contain the wildcard permission.
    /// </param>
    /// <returns>A <see cref="Result{Profile}"/> containing the new profile, or a validation error.</returns>
    public static Result<Profile> Create(
        string name,
        IEnumerable<string> permissions,
        IDateTimeProvider dateTimeProvider,
        Guid? holdingId = null)
    {
        if (string.IsNullOrWhiteSpace(name))
        {
            return Result.Failure<Profile>(ProfileErrors.NameRequired());
        }

        if (name.Length > MaxNameLength)
        {
            return Result.Failure<Profile>(ProfileErrors.NameTooLong(MaxNameLength));
        }

        if (holdingId == Guid.Empty)
        {
            return Result.Failure<Profile>(ProfileErrors.HoldingIdInvalid());
        }

        var normalized = Normalize(permissions);

        if (holdingId is not null && normalized.Contains(WildcardPermission, StringComparer.Ordinal))
        {
            return Result.Failure<Profile>(ProfileErrors.WildcardNotAllowed());
        }

        var profile = new Profile(ProfileId.New(), name.Trim(), dateTimeProvider.UtcNow, holdingId, isSystem: false);
        profile._permissions.AddRange(normalized);

        profile.RaiseDomainEvent(
            new ProfileCreated(profile.Id, profile.Name, profile.CreatedAt));

        return profile;
    }

    /// <summary>
    /// Creates the system-managed full-access profile of a Holding (wildcard permission,
    /// immutable). Created for the Holding's first user; at most one exists per Holding.
    /// </summary>
    /// <param name="holdingId">The owning Holding.</param>
    /// <param name="dateTimeProvider">Provider for deterministic UTC timestamps.</param>
    /// <returns>A <see cref="Result{Profile}"/> containing the new profile, or a validation error.</returns>
    public static Result<Profile> CreateSystemFullAccess(Guid holdingId, IDateTimeProvider dateTimeProvider)
    {
        if (holdingId == Guid.Empty)
        {
            return Result.Failure<Profile>(ProfileErrors.HoldingIdInvalid());
        }

        var profile = new Profile(ProfileId.New(), SystemFullAccessName, dateTimeProvider.UtcNow, holdingId, isSystem: true);
        profile._permissions.Add(WildcardPermission);

        profile.RaiseDomainEvent(
            new ProfileCreated(profile.Id, profile.Name, profile.CreatedAt));

        return profile;
    }

    /// <summary>
    /// Adds a permission to this profile.
    /// </summary>
    /// <param name="permission">The permission to add.</param>
    /// <returns>A result indicating success or a business error.</returns>
    public Result AddPermission(string permission)
    {
        if (IsSystem)
        {
            return Result.Failure(ProfileErrors.SystemProfileImmutable());
        }

        if (string.IsNullOrWhiteSpace(permission))
        {
            return Result.Failure(ProfileErrors.PermissionRequired());
        }

        if (HoldingId is not null && permission == WildcardPermission)
        {
            return Result.Failure(ProfileErrors.WildcardNotAllowed());
        }

        if (_permissions.Contains(permission, StringComparer.Ordinal))
        {
            return Result.Failure(ProfileErrors.PermissionAlreadyExists(permission));
        }

        _permissions.Add(permission);
        return Result.Success();
    }

    /// <summary>
    /// Removes a permission from this profile.
    /// </summary>
    /// <param name="permission">The permission to remove.</param>
    /// <returns>A result indicating success or a business error.</returns>
    public Result RemovePermission(string permission)
    {
        if (IsSystem)
        {
            return Result.Failure(ProfileErrors.SystemProfileImmutable());
        }

        if (!_permissions.Remove(permission))
        {
            return Result.Failure(ProfileErrors.PermissionNotFound(permission));
        }

        return Result.Success();
    }

    /// <summary>Deactivates this profile, preventing new assignments.</summary>
    /// <returns>A result indicating success, or a business error for a system profile.</returns>
    public Result Deactivate()
    {
        if (IsSystem)
        {
            return Result.Failure(ProfileErrors.SystemProfileImmutable());
        }

        IsActive = false;
        return Result.Success();
    }

    /// <summary>Reactivates this profile, allowing new assignments.</summary>
    /// <returns>A result indicating success, or a business error for a system profile.</returns>
    public Result Activate()
    {
        if (IsSystem)
        {
            return Result.Failure(ProfileErrors.SystemProfileImmutable());
        }

        IsActive = true;
        return Result.Success();
    }

    /// <summary>
    /// Updates the profile's name and replaces the entire permission set.
    /// </summary>
    /// <param name="name">The new display name.</param>
    /// <param name="permissions">The new, complete permission set.</param>
    /// <returns>A result indicating success or a business error.</returns>
    public Result UpdateInformation(string name, IEnumerable<string> permissions)
    {
        if (IsSystem)
        {
            return Result.Failure(ProfileErrors.SystemProfileImmutable());
        }

        if (string.IsNullOrWhiteSpace(name))
        {
            return Result.Failure(ProfileErrors.NameRequired());
        }

        if (name.Length > MaxNameLength)
        {
            return Result.Failure(ProfileErrors.NameTooLong(MaxNameLength));
        }

        var normalized = Normalize(permissions);

        if (HoldingId is not null && normalized.Contains(WildcardPermission, StringComparer.Ordinal))
        {
            return Result.Failure(ProfileErrors.WildcardNotAllowed());
        }

        Name = name.Trim();
        _permissions.Clear();
        _permissions.AddRange(normalized);

        return Result.Success();
    }

    private static List<string> Normalize(IEnumerable<string> permissions) =>
        permissions
            .Where(permission => !string.IsNullOrWhiteSpace(permission))
            .Distinct(StringComparer.Ordinal)
            .ToList();
}
