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

        RuleFor(x => x.Manufacturer)
            .NotEmpty()
            .MaximumLength(global::Asset.Domain.EngineModel.MaxManufacturerLength);
    }
}