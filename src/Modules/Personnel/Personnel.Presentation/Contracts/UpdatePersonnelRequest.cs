namespace MachineryManagerEnterprise.Personnel.Presentation.Contracts;

/// <summary>Request body for updating an existing Personnel record's editable details.</summary>
public sealed record UpdatePersonnelRequest(
    string FirstName,
    string LastName,
    string PersonnelCode,
    Guid JobTitleId,
    IReadOnlyList<DrivingLicenseRequest> DrivingLicenses);