using MachineryManagerEnterprise.SharedKernel;

namespace MachineryManagerEnterprise.Configuration.Presentation.Contracts;

/// <summary>Request body for registering a new Job Title within a Holding.</summary>
/// <param name="HoldingId">The identifier of the Holding that will own this Job Title.</param>
/// <param name="Name">The display name of the job title, required, max 100 characters.</param>
public sealed record RegisterJobTitleRequest(Guid HoldingId, string Name);