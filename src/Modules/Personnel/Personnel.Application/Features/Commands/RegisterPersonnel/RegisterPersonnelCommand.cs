using MachineryManagerEnterprise.SharedKernel;
using MediatR;

namespace MachineryManagerEnterprise.Personnel.Application.Features.Commands.RegisterPersonnel;

/// <summary>Represents a single driving license supplied at registration time.</summary>
public sealed record DrivingLicenseInput(Guid DrivingLicenseTypeId, DateOnly ExpiryDate);

/// <summary>
/// Registers a new Personnel record, owned by an Organization and
/// currently assigned to a Project, with zero or more driving licenses.
/// </summary>
public sealed record RegisterPersonnelCommand(
    Guid OrganizationId,
    Guid ProjectId,
    string FirstName,
    string LastName,
    string PersonnelCode,
    Guid JobTitleId,
    IReadOnlyList<DrivingLicenseInput> DrivingLicenses) : IRequest<Result<Guid>>;