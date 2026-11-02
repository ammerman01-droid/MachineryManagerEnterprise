namespace MachineryManagerEnterprise.Usage.Presentation.Contracts;

/// <summary>HTTP request body for renaming a Meter Device.</summary>
/// <param name="Name">The new human-readable name; must be unique within the device's Organization.</param>
public sealed record RenameMeterDeviceRequest(string Name);
