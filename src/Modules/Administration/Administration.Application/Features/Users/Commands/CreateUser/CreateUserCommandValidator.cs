using FluentValidation;

namespace MachineryManagerEnterprise.Administration.Application.Features.Users.Commands.CreateUser;

/// <summary>Validates <see cref="CreateUserCommand"/> input shape (business rules live in the handler).</summary>
public sealed class CreateUserCommandValidator : AbstractValidator<CreateUserCommand>
{
    /// <summary>Initializes a new instance of the <see cref="CreateUserCommandValidator"/> class.</summary>
    public CreateUserCommandValidator()
    {
        RuleFor(x => x.UserName).NotEmpty().WithMessage("User name is required.");
        RuleFor(x => x.Password).NotEmpty().WithMessage("Password is required.");
        RuleFor(x => x.ProfileId)
            .NotEqual(Guid.Empty)
            .When(x => x.ProfileId.HasValue)
            .WithMessage("A valid profile identifier is required.");
    }
}
