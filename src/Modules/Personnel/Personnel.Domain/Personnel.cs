using MachineryManagerEnterprise.Personnel.Domain.Events;
using MachineryManagerEnterprise.SharedKernel;
using MachineryManagerEnterprise.SharedKernel.Abstractions;

namespace MachineryManagerEnterprise.Personnel.Domain;

/// <summary>
/// Aggregate Root representing a Personnel record. Per BR-017's
/// ownership model (the same pattern used for Asset and Warehouse
/// Inventory): <see cref="OrganizationId"/> is the permanent owner and
/// never changes after registration; <see cref="CurrentProjectId"/> is
/// the operational assignment and MAY be reassigned over time by future
/// commands (out of this MVP's scope). A Personnel may hold zero, one,
/// or multiple driving licenses (<see cref="DrivingLicenses"/>), and its
/// Job Title is a reference into the Configuration module's manageable
/// Job Title catalog rather than free text.
/// </summary>
public sealed class Personnel : AggregateRoot<PersonnelId>
{
    /// <summary>Gets the maximum allowed length of <see cref="FirstName"/>.</summary>
    public const int MaxFirstNameLength = 100;

    /// <summary>Gets the maximum allowed length of <see cref="LastName"/>.</summary>
    public const int MaxLastNameLength = 100;

    /// <summary>Gets the maximum allowed length of <see cref="PersonnelCode"/>.</summary>
    public const int MaxPersonnelCodeLength = 30;

    private readonly List<PersonnelDrivingLicense> _drivingLicenses = new();

    /// <summary>Gets the Organization that permanently owns this Personnel record (BR-017). Immutable after registration.</summary>
    public Guid OrganizationId { get; private set; }

    /// <summary>
    /// Gets the Project this Personnel is currently operationally
    /// assigned to. Reassignment/leave commands (which would change this
    /// value while preserving historical scoping, per BR-017) are
    /// explicitly out of scope for this registration-only MVP.
    /// </summary>
    public Guid CurrentProjectId { get; private set; }

    /// <summary>Gets the first name.</summary>
    public string FirstName { get; private set; } = string.Empty;

    /// <summary>Gets the last name.</summary>
    public string LastName { get; private set; } = string.Empty;

    /// <summary>Gets the personnel code, unique within the owning Organization.</summary>
    public string PersonnelCode { get; private set; } = string.Empty;

    /// <summary>Gets the identifier of the Job Title, managed as a separate Configuration catalog entry.</summary>
    public Guid JobTitleId { get; private set; }

    /// <summary>Gets the read-only list of driving licenses held by this Personnel. May be empty.</summary>
    public IReadOnlyList<PersonnelDrivingLicense> DrivingLicenses => _drivingLicenses.AsReadOnly();

    private Personnel()
    {
    }

    private Personnel(
        PersonnelId id,
        Guid organizationId,
        Guid currentProjectId,
        string firstName,
        string lastName,
        string personnelCode,
        Guid jobTitleId)
        : base(id)
    {
        OrganizationId = organizationId;
        CurrentProjectId = currentProjectId;
        FirstName = firstName;
        LastName = lastName;
        PersonnelCode = personnelCode;
        JobTitleId = jobTitleId;
    }

    /// <summary>
    /// Registers a new Personnel record within an Organization, currently
    /// assigned to a Project, with zero or more initial driving licenses.
    /// </summary>
    public static Result<Personnel> Register(
        Guid organizationId,
        Guid currentProjectId,
        string firstName,
        string lastName,
        string personnelCode,
        Guid jobTitleId,
        IReadOnlyList<(Guid DrivingLicenseTypeId, DateOnly ExpiryDate)> drivingLicenses,
        IDateTimeProvider dateTimeProvider)
    {
        if (string.IsNullOrWhiteSpace(firstName))
        {
            return Result.Failure<Personnel>(PersonnelErrors.FirstNameRequired());
        }

        if (firstName.Length > MaxFirstNameLength)
        {
            return Result.Failure<Personnel>(PersonnelErrors.FirstNameTooLong(MaxFirstNameLength));
        }

        if (string.IsNullOrWhiteSpace(lastName))
        {
            return Result.Failure<Personnel>(PersonnelErrors.LastNameRequired());
        }

        if (lastName.Length > MaxLastNameLength)
        {
            return Result.Failure<Personnel>(PersonnelErrors.LastNameTooLong(MaxLastNameLength));
        }

        if (string.IsNullOrWhiteSpace(personnelCode))
        {
            return Result.Failure<Personnel>(PersonnelErrors.PersonnelCodeRequired());
        }

        if (personnelCode.Length > MaxPersonnelCodeLength)
        {
            return Result.Failure<Personnel>(PersonnelErrors.PersonnelCodeTooLong(MaxPersonnelCodeLength));
        }

        if (jobTitleId == Guid.Empty)
        {
            return Result.Failure<Personnel>(PersonnelErrors.JobTitleIdRequired());
        }

        var personnel = new Personnel(
            PersonnelId.New(),
            organizationId,
            currentProjectId,
            firstName.Trim(),
            lastName.Trim(),
            personnelCode.Trim(),
            jobTitleId);

        foreach (var license in drivingLicenses ?? Array.Empty<(Guid DrivingLicenseTypeId, DateOnly ExpiryDate)>())
        {
            var addResult = personnel.AddDrivingLicense(license.DrivingLicenseTypeId, license.ExpiryDate);

            if (addResult.IsFailure)
            {
                return Result.Failure<Personnel>(addResult.Error);
            }
        }

        personnel.RaiseDomainEvent(new PersonnelRegistered(
            personnel.Id, organizationId, currentProjectId, personnel.PersonnelCode, dateTimeProvider.UtcNow));

        return personnel;
    }

    /// <summary>
    /// Adds a driving license to this Personnel. A Personnel may hold at
    /// most one license per Driving License Type.
    /// </summary>
    public Result AddDrivingLicense(Guid drivingLicenseTypeId, DateOnly expiryDate)
    {
        if (drivingLicenseTypeId == Guid.Empty)
        {
            return Result.Failure(PersonnelErrors.DrivingLicenseTypeIdRequired());
        }

        if (expiryDate == default)
        {
            return Result.Failure(PersonnelErrors.DrivingLicenseExpiryDateRequired());
        }

        if (_drivingLicenses.Any(l => l.DrivingLicenseTypeId == drivingLicenseTypeId))
        {
            return Result.Failure(PersonnelErrors.DuplicateDrivingLicenseType());
        }

        _drivingLicenses.Add(PersonnelDrivingLicense.Create(drivingLicenseTypeId, expiryDate));

        return Result.Success();
    }

    /// <summary>
    /// Updates this Personnel's editable details. <see cref="OrganizationId"/>
    /// and <see cref="CurrentProjectId"/> are intentionally NOT editable here
    /// (project reassignment is a separate, not-yet-implemented capability
    /// per BR-017).
    /// </summary>
    public Result UpdateDetails(string firstName, string lastName, string personnelCode, Guid jobTitleId, IDateTimeProvider dateTimeProvider)
    {
        if (string.IsNullOrWhiteSpace(firstName))
        {
            return Result.Failure(PersonnelErrors.FirstNameRequired());
        }

        if (firstName.Length > MaxFirstNameLength)
        {
            return Result.Failure(PersonnelErrors.FirstNameTooLong(MaxFirstNameLength));
        }

        if (string.IsNullOrWhiteSpace(lastName))
        {
            return Result.Failure(PersonnelErrors.LastNameRequired());
        }

        if (lastName.Length > MaxLastNameLength)
        {
            return Result.Failure(PersonnelErrors.LastNameTooLong(MaxLastNameLength));
        }

        if (string.IsNullOrWhiteSpace(personnelCode))
        {
            return Result.Failure(PersonnelErrors.PersonnelCodeRequired());
        }

        if (personnelCode.Length > MaxPersonnelCodeLength)
        {
            return Result.Failure(PersonnelErrors.PersonnelCodeTooLong(MaxPersonnelCodeLength));
        }

        if (jobTitleId == Guid.Empty)
        {
            return Result.Failure(PersonnelErrors.JobTitleIdRequired());
        }

        FirstName = firstName.Trim();
        LastName = lastName.Trim();
        PersonnelCode = personnelCode.Trim();
        JobTitleId = jobTitleId;

        RaiseDomainEvent(new Events.PersonnelUpdated(Id, dateTimeProvider.UtcNow));

        return Result.Success();
    }

    /// <summary>
    /// Replaces the entire set of driving licenses held by this Personnel
    /// with the given set (full replace, as submitted by the edit form).
    /// </summary>
    public Result ReplaceDrivingLicenses(IReadOnlyList<(Guid DrivingLicenseTypeId, DateOnly ExpiryDate)> drivingLicenses)
    {
        var distinctTypeCount = drivingLicenses.Select(l => l.DrivingLicenseTypeId).Distinct().Count();

        if (distinctTypeCount != drivingLicenses.Count)
        {
            return Result.Failure(PersonnelErrors.DuplicateDrivingLicenseType());
        }

        foreach (var license in drivingLicenses)
        {
            if (license.DrivingLicenseTypeId == Guid.Empty)
            {
                return Result.Failure(PersonnelErrors.DrivingLicenseTypeIdRequired());
            }

            if (license.ExpiryDate == default)
            {
                return Result.Failure(PersonnelErrors.DrivingLicenseExpiryDateRequired());
            }
        }

        _drivingLicenses.Clear();

        foreach (var license in drivingLicenses)
        {
            _drivingLicenses.Add(PersonnelDrivingLicense.Create(license.DrivingLicenseTypeId, license.ExpiryDate));
        }

        return Result.Success();
    }

}