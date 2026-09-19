using Configuration.Domain.Events;
using MachineryManagerEnterprise.SharedKernel;
using MachineryManagerEnterprise.SharedKernel.Abstractions;

namespace Configuration.Domain;

/// <summary>
/// Aggregate Root representing a user-managed operational status option
/// for Assets (e.g. "در حال کار", "منتظر تصمیم‌گیری", "آماده به کار",
/// "خارج از ناوگان" — chat, 2026-09-18). Holding-scoped, mirroring
/// <see cref="Color"/> exactly: every Organization under the same
/// Holding shares one operational-status catalog.
/// </summary>
public sealed class AssetOperationalStatus : AggregateRoot<AssetOperationalStatusId>
{
    /// <summary>The maximum allowed length for the name field.</summary>
    public const int MaxNameLength = 50;

    /// <summary>Gets the identifier of the owning Holding.</summary>
    public Guid HoldingId { get; private set; }

    /// <summary>Gets the display name of this operational status.</summary>
    public string Name { get; private set; } = string.Empty;

    private AssetOperationalStatus()
    {
    }

    private AssetOperationalStatus(AssetOperationalStatusId id, Guid holdingId, string name)
        : base(id)
    {
        HoldingId = holdingId;
        Name = name;
    }

    /// <summary>Registers a new operational status option within a Holding.</summary>
    public static Result<AssetOperationalStatus> Register(
        Guid holdingId, string name, IDateTimeProvider dateTimeProvider)
    {
        if (string.IsNullOrWhiteSpace(name))
        {
            return Result.Failure<AssetOperationalStatus>(AssetOperationalStatusErrors.NameRequired());
        }

        if (name.Length > MaxNameLength)
        {
            return Result.Failure<AssetOperationalStatus>(AssetOperationalStatusErrors.NameTooLong(MaxNameLength));
        }

        var status = new AssetOperationalStatus(AssetOperationalStatusId.New(), holdingId, name.Trim());

        status.RaiseDomainEvent(new AssetOperationalStatusRegistered(status.Id, holdingId, status.Name, dateTimeProvider.UtcNow));

        return status;
    }
}