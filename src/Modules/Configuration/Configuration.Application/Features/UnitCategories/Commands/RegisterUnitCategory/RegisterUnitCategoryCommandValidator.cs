using FluentValidation;

namespace MachineryManager.Configuration.Application.Features.UnitCategories.Commands.RegisterUnitCategory;

/// <summary>Validates <see cref="RegisterUnitCategoryCommand"/>.</summary>
public sealed class RegisterUnitCategoryCommandValidator : AbstractValidator<RegisterUnitCategoryCommand>
{
    /// <summary>Initializes a new instance of the <see cref="RegisterUnitCategoryCommandValidator"/> class.</summary>
    public RegisterUnitCategoryCommandValidator()
    {
        RuleFor(x => x.HoldingId).NotEmpty();
        RuleFor(x => x.Name).NotEmpty().MaximumLength(global::Configuration.Domain.UnitCategory.MaxNameLength);
    }
}