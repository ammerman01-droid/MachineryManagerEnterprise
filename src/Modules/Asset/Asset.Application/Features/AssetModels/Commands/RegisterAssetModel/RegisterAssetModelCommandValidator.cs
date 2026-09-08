using FluentValidation;

namespace MachineryManager.Asset.Application.Features.AssetModels.Commands.RegisterAssetModel;

/// <summary>Validates <see cref="RegisterAssetModelCommand"/> per ADR-0036.</summary>
public sealed class RegisterAssetModelCommandValidator : AbstractValidator<RegisterAssetModelCommand>
{
    /// <summary>Initializes validation rules for the register asset model command.</summary>
    public RegisterAssetModelCommandValidator()
    {
        RuleFor(x => x.HoldingId).NotEmpty();

        RuleFor(x => x.Name)
            .NotEmpty()
            .MaximumLength(global::Asset.Domain.AssetModel.MaxNameLength);

        RuleFor(x => x.CompanyId).NotEmpty();

        RuleFor(x => x.LengthValue).GreaterThan(0).When(x => x.LengthValue.HasValue);
        RuleFor(x => x.WidthValue).GreaterThan(0).When(x => x.WidthValue.HasValue);
        RuleFor(x => x.HeightValue).GreaterThan(0).When(x => x.HeightValue.HasValue);
        RuleFor(x => x.WeightValue).GreaterThan(0).When(x => x.WeightValue.HasValue);
        RuleFor(x => x.WorkingCapacityVolumeValue).GreaterThan(0).When(x => x.WorkingCapacityVolumeValue.HasValue);
        RuleFor(x => x.WorkingCapacityWeightValue).GreaterThan(0).When(x => x.WorkingCapacityWeightValue.HasValue);
    }
}