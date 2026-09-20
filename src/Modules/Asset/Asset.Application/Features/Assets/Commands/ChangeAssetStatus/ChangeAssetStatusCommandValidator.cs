using FluentValidation;

namespace MachineryManagerEnterprise.Asset.Application.Features.Assets.Commands.ChangeAssetStatus;

/// <summary>Validates <see cref="ChangeAssetStatusCommand"/> per ADR-0036.</summary>
public sealed class ChangeAssetStatusCommandValidator : AbstractValidator<ChangeAssetStatusCommand>
{
    /// <summary>Initializes validation rules for the change asset status command.</summary>
    public ChangeAssetStatusCommandValidator()
    {
        RuleFor(x => x.AssetId).NotEmpty();
        RuleFor(x => x.NewStatus).IsInEnum();
    }
}
