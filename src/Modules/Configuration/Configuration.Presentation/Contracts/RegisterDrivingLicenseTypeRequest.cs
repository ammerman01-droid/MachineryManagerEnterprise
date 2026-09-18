using MachineryManagerEnterprise.SharedKernel;

namespace MachineryManagerEnterprise.Configuration.Presentation.Contracts;

/// <summary>Request body for registering a new Driving License Type within a Holding.</summary>
/// <param name="HoldingId">The identifier of the Holding that will own this Driving License Type.</param>
/// <param name="Name">The display name of the license type (e.g. "Class B"), required, max 50 characters.</param>
public sealed record RegisterDrivingLicenseTypeRequest(Guid HoldingId, string Name);