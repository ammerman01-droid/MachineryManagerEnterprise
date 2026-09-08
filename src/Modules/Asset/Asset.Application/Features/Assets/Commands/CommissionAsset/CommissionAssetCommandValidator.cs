using FluentValidation;

namespace MachineryManager.Asset.Application.Features.Assets.Commands.CommissionAsset;

/// <summary>Validates <see cref="CommissionAssetCommand"/> per ADR-0036.</summary>
public sealed class CommissionAssetCommandValidator : AbstractValidator<CommissionAssetCommand>
{
    /// <summary>Initializes validation rules for the commission asset command.</summary>
    public CommissionAssetCommandValidator()
    {
        RuleFor(x => x.AssetId).NotEmpty();
    }
}