namespace MachineryManagerEnterprise.Maintenance.Presentation.Contracts;

/// <summary>HTTP request body for cancelling a Work Order.</summary>
/// <param name="Reason">The required reason for cancellation.</param>
public sealed record CancelWorkOrderRequest(string Reason);
