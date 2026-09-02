namespace MachineryManager.SharedKernel;

/// <summary>
/// A fixed, cross-module classification of the fundamental kind of
/// fuel a Configuration-module FuelType represents. Lives in
/// SharedKernel — mirroring <see cref="PhysicalQuantityKind"/> — so
/// other modules' Domain layers can reference it without violating
/// the Modular Monolith rule that a module's Domain layer may depend
/// only on SharedKernel (chat, 2026-09-02).
/// </summary>
/// <remarks>
/// Extend this deliberately — adding a member here is a cross-module
/// contract change, not a Configuration-module-only decision.
/// </remarks>
public enum FuelKind
{
    /// <summary>Diesel fuel (گازوئیل).</summary>
    Diesel = 0,

    /// <summary>Gasoline/petrol (بنزین).</summary>
    Gasoline = 1,

    /// <summary>Gaseous fuel, e.g. CNG/LPG (گاز).</summary>
    Gas = 2,
}