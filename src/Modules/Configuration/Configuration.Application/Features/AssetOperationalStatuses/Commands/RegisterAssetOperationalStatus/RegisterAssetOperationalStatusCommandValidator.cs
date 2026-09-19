using FluentValidation;

namespace MachineryManagerEnterprise.Configuration.Application.Features.AssetOperationalStatuses.Commands.RegisterAssetOperationalStatus;

/// <summary>Validates <see cref="RegisterAssetOperationalStatusCommand"/>.</summary>
public sealed class RegisterAssetOperationalStatusCommandValidator : AbstractValidator<RegisterAssetOperationalStatusCommand>
{
    /// <summary>Initializes a new instance of the <see cref="RegisterAssetOperationalStatusCommandValidator"/> class.</summary>
    public RegisterAssetOperationalStatusCommandValidator()
    {
        RuleFor(x => x.HoldingId).NotEmpty();
        RuleFor(x => x.Name).NotEmpty().MaximumLength(global::Configuration.Domain.AssetOperationalStatus.MaxNameLength);
    }
}