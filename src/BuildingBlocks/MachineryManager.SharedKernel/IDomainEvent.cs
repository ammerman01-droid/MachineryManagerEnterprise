namespace MachineryManager.SharedKernel;

/// <summary>
/// Marker interface for Domain Events, catalogued in
/// docs-english/03-domain/07-DomainEvents.md.
/// Domain Events describe business facts that already happened; they
/// are never used to request an action (see 01-DomainPrinciples.md).
/// </summary>
public interface IDomainEvent
{
    /// <summary>
    /// UTC timestamp at which the underlying business fact occurred.
    /// </summary>
    DateTimeOffset OccurredOn { get; }
}