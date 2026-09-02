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
    }
}