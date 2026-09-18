using MachineryManagerEnterprise.SharedKernel;
using MediatR;

namespace MachineryManagerEnterprise.Personnel.Application.Features.Commands.UpdatePersonnel;

/// <summary>Updates an existing Personnel record's editable details and replaces its driving license list.</summary>
public sealed record UpdatePersonnelCommand(
    Guid PersonnelId,
    string FirstName,
    string LastName,
    string PersonnelCode,
    Guid JobTitleId,
    IReadOnlyList<(Guid DrivingLicenseTypeId, DateOnly ExpiryDate)> DrivingLicenses) : IRequest<Result>;