using FluentValidation;

namespace MachineryManager.Asset.Application.Features.AssetModels.Commands.UpdateAssetModelSpecifications;

/// <summary>Validates <see cref="UpdateAssetModelSpecificationsCommand"/> per ADR-0036.</summary>
public sealed class UpdateAssetModelSpecificationsCommandValidator
    : AbstractValidator<UpdateAssetModelSpecificationsCommand>
{
    /// <summary>Initializes validation rules for the update asset model specifications command.</summary>
    public UpdateAssetModelSpecificationsCommandValidator()
    {
        RuleFor(x => x.AssetModelId).NotEmpty();
        RuleFor(x => x.CompanyId).NotEmpty();

        RuleFor(x => x.LengthValue).GreaterThan(0).When(x => x.LengthValue.HasValue);
        RuleFor(x => x.WidthValue).GreaterThan(0).When(x => x.WidthValue.HasValue);
        RuleFor(x => x.HeightValue).GreaterThan(0).When(x => x.HeightValue.HasValue);
        RuleFor(x => x.WeightValue).GreaterThan(0).When(x => x.WeightValue.HasValue);
        RuleFor(x => x.WorkingCapacityVolumeValue).GreaterThan(0).When(x => x.WorkingCapacityVolumeValue.HasValue);
        RuleFor(x => x.WorkingCapacityWeightValue).GreaterThan(0).When(x => x.WorkingCapacityWeightValue.HasValue);
    }
}