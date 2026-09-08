using FluentValidation;
using MachineryManager.SharedKernel;
using MediatR;

namespace MachineryManager.Configuration.Application.Behaviors;

/// <summary>
/// MediatR pipeline behavior that executes FluentValidation validators
/// before the request handler. Per ADR-0036, validation runs automatically
/// and handlers shall NOT perform ad hoc input validation.
/// </summary>
/// <typeparam name="TRequest">The request type.</typeparam>
/// <typeparam name="TResponse">The response type (expected to be Result or Result&lt;T&gt;).</typeparam>
public sealed class ValidationBehavior<TRequest, TResponse>
    : IPipelineBehavior<TRequest, TResponse>
    where TRequest : notnull
{
    private readonly IEnumerable<IValidator<TRequest>> _validators;

    /// <summary>
    /// Initializes a new instance of the <see cref="ValidationBehavior{TRequest, TResponse}"/> class.
    /// </summary>
    /// <param name="validators">All registered validators for <typeparamref name="TRequest"/>.</param>
    public ValidationBehavior(IEnumerable<IValidator<TRequest>> validators)
    {
        _validators = validators;
    }

    /// <summary>
    /// Validates the request before delegating to the handler. If validation fails,
    /// returns a <see cref="Result"/> failure without invoking the handler.
    /// </summary>
    /// <param name="request">The incoming request.</param>
    /// <param name="next">The delegate to execute the handler.</param>
    /// <param name="cancellationToken">Token to cancel the asynchronous operation.</param>
    /// <returns>The handler response, or a validation failure result.</returns>
    /// <exception cref="InvalidOperationException">
    /// Thrown only when validation fails but the response type does not support
    /// the Result pattern (indicates a programming error, not a validation error).
    /// </exception>
    public async Task<TResponse> Handle(
        TRequest request,
        RequestHandlerDelegate<TResponse> next,
        CancellationToken cancellationToken)
    {
        if (!_validators.Any())
        {
            return await next();
        }

        var context = new ValidationContext<TRequest>(request);
        var validationResults = await Task.WhenAll(
            _validators.Select(v => v.ValidateAsync(context, cancellationToken)));

        var failures = validationResults
            .SelectMany(r => r.Errors)
            .Where(f => f != null)
            .ToList();

        if (failures.Count == 0)
        {
            return await next();
        }

        var error = Error.Validation(
            "Validation.General",
            string.Join("; ", failures.Select(f => f.ErrorMessage)));

        var responseType = typeof(TResponse);

        if (responseType == typeof(Result))
        {
            return (TResponse)(object)Result.Failure(error);
        }

        if (responseType.IsGenericType &&
            responseType.GetGenericTypeDefinition() == typeof(Result<>))
        {
            var valueType = responseType.GetGenericArguments()[0];
            var method = typeof(Result)
                .GetMethod(nameof(Result.Failure), 1, new[] { typeof(Error) })
                ?.MakeGenericMethod(valueType);

            if (method != null)
            {
                var result = method.Invoke(null, new object[] { error });
                return (TResponse)result!;
            }
        }

        throw new InvalidOperationException(
            $"Validation failed but response type {responseType.Name} does not support Result pattern.");
    }
}