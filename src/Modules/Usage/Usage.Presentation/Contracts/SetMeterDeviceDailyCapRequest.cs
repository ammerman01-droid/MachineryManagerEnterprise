namespace MachineryManagerEnterprise.Usage.Presentation.Contracts;

/// <summary>HTTP request body for setting or clearing a Meter Device's own daily cap override (BR-045).</summary>
/// <param name="DailyCapOverride">The new override, or <see langword="null"/> to fall back to the system default.</param>
public sealed record SetMeterDeviceDailyCapRequest(decimal? DailyCapOverride);
