using FluentAssertions;
using FluentValidation;
using FluentValidation.Results;
using MachineryManagerEnterprise.Configuration.Application.Behaviors;
using MachineryManagerEnterprise.SharedKernel;
using MediatR;
using NSubstitute;
using Xunit;

namespace MachineryManagerEnterprise.Configuration.Application.Tests.Behaviors;

/// <summary>
/// Tests for <see cref="ValidationBehavior{TRequest, TResponse}"/> using
/// fake requests rather than any real Command/Query, since the behavior
/// is generic infrastructure shared by every feature in the module.
/// </summary>
public sealed class ValidationBehaviorTests
{
    public sealed record FakeResultRequest : IRequest<Result<int>>;

    public sealed record FakeNonResultRequest : IRequest<string>;

    [Fact]
    public async Task Handle_WithNoRegisteredValidators_CallsNextDirectly()
    {
        var behavior = new ValidationBehavior<FakeResultRequest, Result<int>>(Array.Empty<IValidator<FakeResultRequest>>());
        RequestHandlerDelegate<Result<int>> next = _ => Task.FromResult(Result.Success(1));

        var result = await behavior.Handle(new FakeResultRequest(), next, CancellationToken.None);

        result.IsSuccess.Should().BeTrue();
        result.Value.Should().Be(1);
    }

    [Fact]
    public async Task Handle_WhenValidationFails_ReturnsAValidationFailureResultWithoutCallingNext()
    {
        var validator = Substitute.For<IValidator<FakeResultRequest>>();
        validator
            .ValidateAsync(Arg.Any<ValidationContext<FakeResultRequest>>(), Arg.Any<CancellationToken>())
            .Returns(new ValidationResult([new ValidationFailure("Name", "Name is required")]));

        var behavior = new ValidationBehavior<FakeResultRequest, Result<int>>([validator]);

        // If the behavior incorrectly called `next`, the result would
        // come back successful with value 999 instead of a failure.
        RequestHandlerDelegate<Result<int>> next = _ => Task.FromResult(Result.Success(999));

        var result = await behavior.Handle(new FakeResultRequest(), next, CancellationToken.None);

        result.IsFailure.Should().BeTrue();
        result.Error.Code.Should().Be("Validation.General");
        result.Error.Message.Should().Contain("Name is required");
    }

    [Fact]
    public async Task Handle_WhenValidationPasses_CallsNextAndReturnsItsResult()
    {
        var validator = Substitute.For<IValidator<FakeResultRequest>>();
        validator
            .ValidateAsync(Arg.Any<ValidationContext<FakeResultRequest>>(), Arg.Any<CancellationToken>())
            .Returns(new ValidationResult());

        var behavior = new ValidationBehavior<FakeResultRequest, Result<int>>([validator]);
        RequestHandlerDelegate<Result<int>> next = _ => Task.FromResult(Result.Success(42));

        var result = await behavior.Handle(new FakeResultRequest(), next, CancellationToken.None);

        result.IsSuccess.Should().BeTrue();
        result.Value.Should().Be(42);
    }

    [Fact]
    public async Task Handle_WhenMultipleValidatorsFail_CombinesAllFailureMessages()
    {
        var firstValidator = Substitute.For<IValidator<FakeResultRequest>>();
        firstValidator
            .ValidateAsync(Arg.Any<ValidationContext<FakeResultRequest>>(), Arg.Any<CancellationToken>())
            .Returns(new ValidationResult([new ValidationFailure("A", "A is invalid")]));

        var secondValidator = Substitute.For<IValidator<FakeResultRequest>>();
        secondValidator
            .ValidateAsync(Arg.Any<ValidationContext<FakeResultRequest>>(), Arg.Any<CancellationToken>())
            .Returns(new ValidationResult([new ValidationFailure("B", "B is invalid")]));

        var behavior = new ValidationBehavior<FakeResultRequest, Result<int>>([firstValidator, secondValidator]);
        RequestHandlerDelegate<Result<int>> next = _ => Task.FromResult(Result.Success(0));

        var result = await behavior.Handle(new FakeResultRequest(), next, CancellationToken.None);

        result.Error.Message.Should().Contain("A is invalid").And.Contain("B is invalid");
    }

    [Fact]
    public async Task Handle_WhenValidationFailsForAResponseTypeThatIsNotAResult_Throws()
    {
        // Documents the guard in ValidationBehavior for response types
        // that don't support the Result pattern — this indicates a
        // programming error (a Command/Query not returning Result/Result<T>),
        // not a validation error, so it must surface loudly.
        var validator = Substitute.For<IValidator<FakeNonResultRequest>>();
        validator
            .ValidateAsync(Arg.Any<ValidationContext<FakeNonResultRequest>>(), Arg.Any<CancellationToken>())
            .Returns(new ValidationResult([new ValidationFailure("X", "bad")]));

        var behavior = new ValidationBehavior<FakeNonResultRequest, string>([validator]);
        RequestHandlerDelegate<string> next = _ => Task.FromResult("unused");

        var act = () => behavior.Handle(new FakeNonResultRequest(), next, CancellationToken.None);

        await act.Should().ThrowAsync<InvalidOperationException>();
    }
}
