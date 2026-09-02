using MachineryManager.SharedKernel;

namespace MachineryManager.Configuration.Presentation.Contracts;

/// <summary>
/// Represents a request to register a Unit of Measurement within a Holding.
/// </summary>
/// <param name="HoldingId">The identifier of the owning Holding.</param>
/// <param name="Name">The display name of the Unit of Measurement.</param>
/// <param name="Kind">The physical quantity kind represented by the Unit of Measurement.</param>
public sealed record RegisterUnitOfMeasurementRequest(
    Guid HoldingId,
    string Name,
    PhysicalQuantityKind Kind);