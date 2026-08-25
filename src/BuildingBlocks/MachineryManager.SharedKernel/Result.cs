namespace MachineryManager.SharedKernel;

/// <summary>
/// Represents the outcome of an operation that can fail with a
/// Business Error (05-development/07-ErrorHandling.md), without using
/// exceptions for expected, non-exceptional business outcomes.
/// </summary>
public class Result
{
    /// <summary>
    /// Initializes a new instance of the result.
    /// </summary>
    /// <param name="isSuccess">Indicates whether the operation succeeded.</param>
    /// <param name="error">The error associated with the result.</param>
    protected Result(bool isSuccess, Error error)
    {
        if (isSuccess && error != Error.None)
        {
            throw new InvalidOperationException("A successful result cannot carry an error.");
        }

        if (!isSuccess && error == Error.None)
        {
            throw new InvalidOperationException("A failed result must carry an error.");
        }

        IsSuccess = isSuccess;
        Error = error;
    }

    /// <summary>Whether the operation succeeded.</summary>
    public bool IsSuccess { get; }

    /// <summary>Whether the operation failed.</summary>
    public bool IsFailure => !IsSuccess;

    /// <summary>The error describing why the operation failed, or <see cref="SharedKernel.Error.None"/> on success.</summary>
    public Error Error { get; }

    /// <summary>Creates a successful result with no value.</summary>
    public static Result Success() => new(true, Error.None);

    /// <summary>Creates a failed result carrying the given error.</summary>
    public static Result Failure(Error error) => new(false, error);

    /// <summary>Creates a successful result carrying the given value.</summary>
    public static Result<TValue> Success<TValue>(TValue value) => new(value, true, Error.None);

    /// <summary>Creates a failed result of the given value type, carrying the given error.</summary>
    public static Result<TValue> Failure<TValue>(Error error) => new(default, false, error);
}

/// <summary>
/// A <see cref="Result"/> that carries a value on success.
/// </summary>
/// <typeparam name="TValue">The type of the value returned on success.</typeparam>
public sealed class Result<TValue> : Result
{
    private readonly TValue? _value;

    internal Result(TValue? value, bool isSuccess, Error error)
        : base(isSuccess, error)
    {
        _value = value;
    }

    /// <summary>
    /// The success value. Accessing this on a failed Result throws,
    /// because callers must check <see cref="Result.IsSuccess"/> first.
    /// </summary>
    public TValue Value => IsSuccess
        ? _value!
        : throw new InvalidOperationException("The value of a failed result cannot be accessed.");

    /// <summary>Implicitly wraps a value into a successful <see cref="Result{TValue}"/>.</summary>
    public static implicit operator Result<TValue>(TValue value) => Success(value);
}
