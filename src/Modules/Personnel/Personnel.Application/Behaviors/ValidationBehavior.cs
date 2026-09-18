using FluentValidation;
using MachineryManagerEnterprise.SharedKernel;
using MediatR;

namespace MachineryManagerEnterprise.Personnel.Application.Behaviors;

/// <summary>MediatR pipeline behavior that executes FluentValidation validators before the request handler.</summary>
public sealed class ValidationBehavior<TRequest, TResponse> : IPipelineBehavior<TRequest, TResponse>
    where TRequest : notnull
{
    private readonly IEnumerable<IValidator<TRequest>> _validators;

    /// <summary>Initializes a new instance of the <see cref="ValidationBehavior{TRequest, TResponse}"/> class.</summary>
    public ValidationBehavior(IEnumerable<IValidator<TRequest>> validators)
    {
        _validators = validators;
    }

    /// <summary>Validates <paramref name="request"/> and short-circuits the pipeline with a <see cref="Result"/> failure if validation fails.</summary>
    public async Task<TResponse> Handle(TRequest request, RequestHandlerDelegate<TResponse> next, CancellationToken cancellationToken)
    {
        if (!_validators.Any())
        {
            return await next();
        }

        var context = new ValidationContext<TRequest>(request);
        var validationResults = await Task.WhenAll(_validators.Select(v => v.ValidateAsync(context, cancellationToken)));
        var failures = validationResults.SelectMany(r => r.Errors).Where(f => f != null).ToList();

        if (failures.Count == 0)
        {
            return await next();
        }

        var error = Error.Validation("Validation.General", string.Join("; ", failures.Select(f => f.ErrorMessage)));
        var responseType = typeof(TResponse);

        if (responseType == typeof(Result))
        {
            return (TResponse)(object)Result.Failure(error);
        }

        if (responseType.IsGenericType && responseType.GetGenericTypeDefinition() == typeof(Result<>))
        {
            var valueType = responseType.GetGenericArguments()[0];
            var method = typeof(Result).GetMethod(nameof(Result.Failure), 1, new[] { typeof(Error) })?.MakeGenericMethod(valueType);

            if (method != null)
            {
                var result = method.Invoke(null, new object[] { error });
                return (TResponse)result!;
            }
        }

        throw new InvalidOperationException($"Validation failed but response type {responseType.Name} does not support Result pattern.");
    }
}