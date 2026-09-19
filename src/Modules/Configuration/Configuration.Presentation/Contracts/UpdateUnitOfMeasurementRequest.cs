using MachineryManagerEnterprise.SharedKernel;

namespace MachineryManagerEnterprise.Configuration.Presentation.Contracts;

/// <summary>
/// Represents a request to update an existing Unit of Measurement.
/// </summary>
/// <param name="Name">The new display name of the Unit of Measurement.</param>
/// <param name="Kind">The new physical quantity kind represented by the Unit of Measurement.</param>
public sealed record UpdateUnitOfMeasurementRequest(
    string Name,
    PhysicalQuantityKind Kind);
