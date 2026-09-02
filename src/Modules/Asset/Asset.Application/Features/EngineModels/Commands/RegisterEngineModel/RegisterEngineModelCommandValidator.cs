using FluentValidation;

namespace MachineryManager.Asset.Application.Features.EngineModels.Commands.RegisterEngineModel;

/// <summary>Validates <see cref="RegisterEngineModelCommand"/> per ADR-0036.</summary>
public sealed class RegisterEngineModelCommandValidator : AbstractValidator<RegisterEngineModelCommand>
{
    /// <summary>Initializes validation rules for the register engine model command.</summary>
    public RegisterEngineModelCommandValidator()
    {
        RuleFor(x => x.HoldingId).NotEmpty();

        RuleFor(x => x.Name)
            .NotEmpty()
            .MaximumLength(global::Asset.Domain.EngineModel.MaxNameLength);

        RuleFor(x => x.CompanyId).NotEmpty();

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