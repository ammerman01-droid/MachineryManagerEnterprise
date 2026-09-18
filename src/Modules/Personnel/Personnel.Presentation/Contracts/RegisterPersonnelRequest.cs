using MachineryManagerEnterprise.SharedKernel;

namespace MachineryManagerEnterprise.Personnel.Presentation.Contracts;

/// <summary>Represents a single driving license supplied at registration time.</summary>
/// <param name="DrivingLicenseTypeId">The identifier of the Driving License Type (Configuration module).</param>
/// <param name="ExpiryDate">The license's expiry date.</param>
public sealed record DrivingLicenseRequest(Guid DrivingLicenseTypeId, DateOnly ExpiryDate);

/// <summary>Request body for registering a new Personnel record.</summary>
/// <param name="OrganizationId">The identifier of the owning Organization.</param>
/// <param name="ProjectId">The identifier of the Project this Personnel is currently assigned to.</param>
/// <param name="FirstName">The first name.</param>
/// <param name="LastName">The last name.</param>
/// <param name="PersonnelCode">The personnel code, unique within the owning Organization.</param>
/// <param name="JobTitleId">The identifier of the Job Title (Configuration module).</param>
/// <param name="DrivingLicenses">Zero or more driving licenses held by this Personnel.</param>
public sealed record RegisterPersonnelRequest(
    Guid OrganizationId,
    Guid ProjectId,
    string FirstName,
    string LastName,
    string PersonnelCode,
    Guid JobTitleId,
    IReadOnlyList<DrivingLicenseRequest> DrivingLicenses);