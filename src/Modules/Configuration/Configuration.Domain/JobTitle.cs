using Configuration.Domain.Events;
using MachineryManagerEnterprise.SharedKernel;
using MachineryManagerEnterprise.SharedKernel.Abstractions;

namespace Configuration.Domain;

/// <summary>
/// Aggregate Root representing a manageable Job Title option (e.g.
/// "Site Supervisor", "Heavy Equipment Operator"), used by the
/// Personnel module. Lives in the Configuration module as
/// reference/master data, Holding-scoped — same pattern as
/// <see cref="DrivingLicenseType"/> and <see cref="Color"/>.
/// </summary>
public sealed class JobTitle : AggregateRoot<JobTitleId>
{
    /// <summary>Gets the maximum allowed length of <see cref="Name"/>.</summary>
    public const int MaxNameLength = 100;

    /// <summary>Gets the identifier of the owning Holding.</summary>
    public Guid HoldingId { get; private set; }

    /// <summary>Gets the name of the Job Title.</summary>
    public string Name { get; private set; } = string.Empty;

    private JobTitle()
    {
    }

    private JobTitle(JobTitleId id, Guid holdingId, string name)
        : base(id)
    {
        HoldingId = holdingId;
        Name = name;
    }

    /// <summary>Registers a new Job Title option within a Holding.</summary>
    public static Result<JobTitle> Register(Guid holdingId, string name, IDateTimeProvider dateTimeProvider)
    {
        if (string.IsNullOrWhiteSpace(name))
        {
            return Result.Failure<JobTitle>(JobTitleErrors.NameRequired());
        }

        if (name.Length > MaxNameLength)
        {
            return Result.Failure<JobTitle>(JobTitleErrors.NameTooLong(MaxNameLength));
        }

        var jobTitle = new JobTitle(JobTitleId.New(), holdingId, name.Trim());

        jobTitle.RaiseDomainEvent(new JobTitleRegistered(jobTitle.Id, holdingId, jobTitle.Name, dateTimeProvider.UtcNow));

        return jobTitle;
    }

    /// <summary>Renames this Job Title.</summary>
    public Result Rename(string name, IDateTimeProvider dateTimeProvider)
    {
        if (string.IsNullOrWhiteSpace(name))
        {
            return Result.Failure(JobTitleErrors.NameRequired());
        }

        if (name.Length > MaxNameLength)
        {
            return Result.Failure(JobTitleErrors.NameTooLong(MaxNameLength));
        }

        Name = name.Trim();
        RaiseDomainEvent(new Events.JobTitleRenamed(Id, Name, dateTimeProvider.UtcNow));

        return Result.Success();
    }
}