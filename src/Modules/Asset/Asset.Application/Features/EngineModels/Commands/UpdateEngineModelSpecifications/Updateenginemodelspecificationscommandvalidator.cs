using FluentValidation;

namespace MachineryManager.Asset.Application.Features.EngineModels.Commands.UpdateEngineModelSpecifications;

/// <summary>Validates <see cref="UpdateEngineModelSpecificationsCommand"/> per ADR-0036.</summary>
public sealed class UpdateEngineModelSpecificationsCommandValidator : AbstractValidator<UpdateEngineModelSpecificationsCommand>
{
    /// <summary>Initializes validation rules for the update specifications command.</summary>
    public UpdateEngineModelSpecificationsCommandValidator()
    {
        RuleFor(x => x.EngineModelId).NotEmpty();

        RuleFor(x => x.CompanyId).NotEmpty();

        RuleFor(x => x.FuelKind)
            .IsInEnum()
            .WithMessage("نوع سوخت انتخاب‌شده معتبر نیست.");

        RuleFor(x => x.CylinderCount)
            .GreaterThan(0)
            .When(x => x.CylinderCount.HasValue);

        RuleFor(x => x.EngineDisplacementValue)
            .GreaterThan(0)
            .When(x => x.EngineDisplacementValue.HasValue);

        RuleFor(x => x.EnginePowerValue)
            .GreaterThan(0)
            .When(x => x.EnginePowerValue.HasValue);

        RuleFor(x => x.WeightValue)
            .GreaterThan(0)
            .When(x => x.WeightValue.HasValue);
    }
}