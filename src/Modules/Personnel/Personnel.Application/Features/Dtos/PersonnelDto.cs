namespace MachineryManagerEnterprise.Personnel.Application.Features.Dtos;

/// <summary>Represents a single driving license within a <see cref="PersonnelDto"/>.</summary>
public sealed record PersonnelDrivingLicenseDto(Guid DrivingLicenseTypeId, DateOnly ExpiryDate);

/// <summary>Represents the PersonnelDto data contract.</summary>
public sealed record PersonnelDto(
    Guid Id,
    Guid OrganizationId,
    Guid CurrentProjectId,
    string FirstName,
    string LastName,
    string PersonnelCode,
    Guid JobTitleId,
    IReadOnlyList<PersonnelDrivingLicenseDto> DrivingLicenses);