using FluentValidation;
using Administration.Domain;

namespace MachineryManagerEnterprise.Administration.Application.Features.UserProfileAssignments.Commands.AssignUserToProfile;

/// <summary>
/// Validates <see cref="AssignUserToProfileCommand"/> per ADR-0036.
/// </summary>
public sealed class AssignUserToProfileCommandValidator : AbstractValidator<AssignUserToProfileCommand>
{
    /// <summary>
    /// Initializes validation rules for the assign-user-to-profile command.
    /// </summary>
    public AssignUserToProfileCommandValidator()
    {
        RuleFor(x => x.UserId).NotEmpty();
        RuleFor(x => x.ProfileId).NotEmpty();
        RuleFor(x => x.Scope).NotNull();

        RuleFor(x => x.Scope.HoldingId).NotEmpty()
            .When(x => x.Scope.Level == AuthorizationScopeLevel.Holding);
        RuleFor(x => x.Scope.OrganizationId).NotEmpty()
            .When(x => x.Scope.Level == AuthorizationScopeLevel.Organization);
        RuleFor(x => x.Scope.ProjectId).NotEmpty()
            .When(x => x.Scope.Level == AuthorizationScopeLevel.Project);

        RuleFor(x => x.Scope.HoldingId).Null()
            .When(x => x.Scope.Level != AuthorizationScopeLevel.Holding);
        RuleFor(x => x.Scope.OrganizationId).Null()
            .When(x => x.Scope.Level != AuthorizationScopeLevel.Organization);
        RuleFor(x => x.Scope.ProjectId).Null()
            .When(x => x.Scope.Level != AuthorizationScopeLevel.Project);
    }
}