namespace MachineryManagerEnterprise.Usage.Presentation.Contracts;

/// <summary>HTTP request body for correcting an already-registered shift reading's raw value (BR-051).</summary>
/// <param name="NewRawReadingValue">The corrected raw reading value.</param>
public sealed record CorrectEntryReadingRequest(decimal NewRawReadingValue);
