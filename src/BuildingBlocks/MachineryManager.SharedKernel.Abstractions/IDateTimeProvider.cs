namespace MachineryManager.SharedKernel.Abstractions;

/// <summary>
/// Provides the current time. Domain and Application code shall depend
/// on this abstraction instead of calling <see cref="DateTimeOffset.UtcNow"/>
/// directly, so business logic remains deterministic and testable.
/// </summary>
public interface IDateTimeProvider
{
    /// <summary>The current UTC time.</summary>
    DateTimeOffset UtcNow { get; }
}