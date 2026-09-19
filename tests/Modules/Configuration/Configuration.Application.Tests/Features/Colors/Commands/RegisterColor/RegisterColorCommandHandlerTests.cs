using FluentAssertions;
using MachineryManagerEnterprise.Configuration.Application.Abstractions;
using MachineryManagerEnterprise.Configuration.Application.Features.Colors.Commands.RegisterColor;
using MachineryManagerEnterprise.SharedKernel.Abstractions;
using NSubstitute;
using Xunit;

namespace MachineryManagerEnterprise.Configuration.Application.Tests.Features.Colors.Commands.RegisterColor;

/// <summary>Tests for <see cref="RegisterColorCommandHandler"/>.</summary>
public sealed class RegisterColorCommandHandlerTests
{
    private const string RequiredPermission = "Color.Create";

    private readonly IColorRepository _colorRepository = Substitute.For<IColorRepository>();
    private readonly IHoldingLookupService _holdingLookupService = Substitute.For<IHoldingLookupService>();
    private readonly IConfigurationUnitOfWork _unitOfWork = Substitute.For<IConfigurationUnitOfWork>();
    private readonly IDateTimeProvider _dateTimeProvider = Substitute.For<IDateTimeProvider>();
    private readonly ICurrentUserService _currentUserService = Substitute.For<ICurrentUserService>();
    private readonly IPermissionEvaluator _permissionEvaluator = Substitute.For<IPermissionEvaluator>();

    private readonly RegisterColorCommandHandler _sut;

    public RegisterColorCommandHandlerTests()
    {
        _dateTimeProvider.UtcNow.Returns(DateTimeOffset.UtcNow);

        _sut = new RegisterColorCommandHandler(
            _colorRepository,
            _holdingLookupService,
            _unitOfWork,
            _dateTimeProvider,
            _currentUserService,
            _permissionEvaluator);
    }

    /// <summary>Sets up the current user as authenticated and (un)authorized for <c>Color.Create</c>.</summary>
    private void ArrangeCurrentUser(Guid userId, bool isAuthorized = true)
    {
        _currentUserService.UserId.Returns((Guid?)userId);
        _permissionEvaluator
            .HasPermissionAsync(userId, RequiredPermission, Arg.Any<ResourceScope>(), Arg.Any<CancellationToken>())
            .Returns(isAuthorized);
    }

    [Fact]
    public async Task Handle_WhenNoUserIsAuthenticated_ReturnsNotAuthorizedAndDoesNotTouchTheRepository()
    {
        _currentUserService.UserId.Returns((Guid?)null);

        var result = await _sut.Handle(new RegisterColorCommand(Guid.NewGuid(), "Red"), CancellationToken.None);

        result.IsFailure.Should().BeTrue();
        result.Error.Code.Should().Be("Color.NotAuthorized");
        _colorRepository.DidNotReceiveWithAnyArgs().Add(default!);
    }

    [Fact]
    public async Task Handle_WhenHoldingDoesNotExist_ReturnsHoldingNotFound()
    {
        var holdingId = Guid.NewGuid();
        ArrangeCurrentUser(Guid.NewGuid());
        _holdingLookupService.ExistsAsync(holdingId, Arg.Any<CancellationToken>()).Returns(false);

        var result = await _sut.Handle(new RegisterColorCommand(holdingId, "Red"), CancellationToken.None);

        result.IsFailure.Should().BeTrue();
        result.Error.Code.Should().Be("Holding.NotFound");
    }

    [Fact]
    public async Task Handle_WhenUserLacksPermission_ReturnsNotAuthorized()
    {
        var holdingId = Guid.NewGuid();
        ArrangeCurrentUser(Guid.NewGuid(), isAuthorized: false);
        _holdingLookupService.ExistsAsync(holdingId, Arg.Any<CancellationToken>()).Returns(true);

        var result = await _sut.Handle(new RegisterColorCommand(holdingId, "Red"), CancellationToken.None);

        result.IsFailure.Should().BeTrue();
        result.Error.Code.Should().Be("Color.NotAuthorized");
    }

    [Fact]
    public async Task Handle_WhenDomainRegistrationFails_ReturnsTheDomainErrorAndDoesNotPersist()
    {
        var holdingId = Guid.NewGuid();
        ArrangeCurrentUser(Guid.NewGuid());
        _holdingLookupService.ExistsAsync(holdingId, Arg.Any<CancellationToken>()).Returns(true);

        var result = await _sut.Handle(new RegisterColorCommand(holdingId, "   "), CancellationToken.None);

        result.IsFailure.Should().BeTrue();
        result.Error.Code.Should().Be("Color.NameRequired");
        _colorRepository.DidNotReceiveWithAnyArgs().Add(default!);
        await _unitOfWork.DidNotReceive().SaveChangesAsync(Arg.Any<CancellationToken>());
    }

    [Fact]
    public async Task Handle_WithAnAuthorizedValidRequest_AddsTheColorAndSavesChanges()
    {
        var holdingId = Guid.NewGuid();
        ArrangeCurrentUser(Guid.NewGuid());
        _holdingLookupService.ExistsAsync(holdingId, Arg.Any<CancellationToken>()).Returns(true);

        var result = await _sut.Handle(new RegisterColorCommand(holdingId, "Red"), CancellationToken.None);

        result.IsSuccess.Should().BeTrue();
        _colorRepository.Received(1).Add(Arg.Is<global::Configuration.Domain.Color>(
            c => c.Name == "Red" && c.HoldingId == holdingId));
        await _unitOfWork.Received(1).SaveChangesAsync(Arg.Any<CancellationToken>());
    }
}
