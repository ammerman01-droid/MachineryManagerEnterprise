using Administration.Domain.Events;
using MachineryManager.SharedKernel;
using MachineryManager.SharedKernel.Abstractions;

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

    /// <summary>Gets the name of the profile.</summary>
    public string Name { get; private set; }

    private readonly List<string> _permissions = [];

    /// <summary>Gets the permissions contained in this profile.</summary>
    public IReadOnlyCollection<string> Permissions => _permissions.AsReadOnly();

    /// <summary>Gets whether this profile is currently active.</summary>
    public bool IsActive { get; private set; }

    /// <summary>Gets the UTC timestamp when the profile was created.</summary>
    public DateTimeOffset CreatedAt { get; private set; }

    // Reserved for EF Core materialization only.
    private Profile()
    {
        Name = string.Empty;
    }

    private Profile(ProfileId id, string name, DateTimeOffset createdAt)
        : base(id)
    {
        Name = name;
        IsActive = true;
        CreatedAt = createdAt;
    }

    /// <summary>
    /// Creates a new Profile. This is the only way a Profile comes into existence.
    /// </summary>
    /// <param name="name">The profile's display name.</param>
    /// <param name="permissions">The initial set of permissions.</param>
    /// <param name="dateTimeProvider">Provider for deterministic UTC timestamps.</param>
    /// <returns>A <see cref="Result{Profile}"/> containing the new profile, or a validation error.</returns>
    public static Result<Profile> Create(
        string name,
        IEnumerable<string> permissions,
        IDateTimeProvider dateTimeProvider)
    {
        if (string.IsNullOrWhiteSpace(name))
        {
            return Result.Failure<Profile>(ProfileErrors.NameRequired());
        }

        if (name.Length > MaxNameLength)
        {
            return Result.Failure<Profile>(ProfileErrors.NameTooLong(MaxNameLength));
        }

        var profile = new Profile(ProfileId.New(), name.Trim(), dateTimeProvider.UtcNow);

        foreach (var permission in permissions.Distinct(StringComparer.Ordinal))
        {
            if (!string.IsNullOrWhiteSpace(permission))
            {
                profile._permissions.Add(permission);
            }
        }

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
        if (string.IsNullOrWhiteSpace(permission))
        {
            return Result.Failure(ProfileErrors.PermissionRequired());
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
        if (!_permissions.Remove(permission))
        {
            return Result.Failure(ProfileErrors.PermissionNotFound(permission));
        }

        return Result.Success();
    }

    /// <summary>Deactivates this profile, preventing new assignments.</summary>
    public void Deactivate()
    {
        IsActive = false;
    }

    /// <summary>Reactivates this profile, allowing new assignments.</summary>
    public void Activate()
    {
        IsActive = true;
    }
}