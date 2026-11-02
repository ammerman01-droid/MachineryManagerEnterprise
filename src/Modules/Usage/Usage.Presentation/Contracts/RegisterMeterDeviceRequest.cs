namespace MachineryManagerEnterprise.Usage.Presentation.Contracts;

/// <summary>HTTP request body for registering a new Meter Device (BR-041).</summary>
/// <param name="OrganizationId">The Organization this device belongs to.</param>
/// <param name="Name">The mandatory, Organization-unique, human-readable device name (chat, 2026-10-28).</param>
/// <param name="Unit">The unit this device reads in; fixed for its lifetime (chat, 2026-09-29).</param>
/// <param name="DailyCapOverride">This device's own override of the system-wide daily cap (BR-045), or <see langword="null"/> to use the default.</param>
public sealed record RegisterMeterDeviceRequest(
    Guid OrganizationId,
    string Name,
    string Unit,
    decimal? DailyCapOverride);
