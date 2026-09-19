using FluentAssertions;
using MachineryManagerEnterprise.Configuration.Application.Abstractions;
using MachineryManagerEnterprise.Configuration.Application.Features.Colors.Dtos;
using MachineryManagerEnterprise.Configuration.Application.Features.Colors.Queries.GetColorsByHolding;
using MachineryManagerEnterprise.SharedKernel.Abstractions;
using NSubstitute;
using Xunit;

namespace MachineryManagerEnterprise.Configuration.Application.Tests.Features.Colors.Queries.GetColorsByHolding;

/// <summary>Tests for <see cref="GetColorsByHoldingQueryHandler"/>.</summary>
public sealed class GetColorsByHoldingQueryHandlerTests
{
    private const string RequiredPermission = "Color.View";

    private readonly IColorRepository _colorRepository = Substitute.For<IColorRepository>();
    private readonly ICurrentUserService _currentUserService = Substitute.For<ICurrentUserService>();
    private readonly IPermissionEvaluator _permissionEvaluator = Substitute.For<IPermissionEvaluator>();

    private readonly GetColorsByHoldingQueryHandler _sut;

    public GetColorsByHoldingQueryHandlerTests()
    {
        _sut = new GetColorsByHoldingQueryHandler(_colorRepository, _currentUserService, _permissionEvaluator);
    }

    private void ArrangeCurrentUser(Guid userId, bool isAuthorized = true)
    {
        _currentUserService.UserId.Returns((Guid?)userId);
        _permissionEvaluator
            .HasPermissionAsync(userId, RequiredPermission, Arg.Any<ResourceScope>(), Arg.Any<CancellationToken>())
            .Returns(isAuthorized);
    }

    [Fact]
    public async Task Handle_WhenNoUserIsAuthenticated_ReturnsNotAuthorized()
    {
        _currentUserService.UserId.Returns((Guid?)null);

        var result = await _sut.Handle(new GetColorsByHoldingQuery(Guid.NewGuid()), CancellationToken.None);

        result.IsFailure.Should().BeTrue();
        result.Error.Code.Should().Be("Color.NotAuthorized");
    }

    [Fact]
    public async Task Handle_WhenUserLacksPermission_ReturnsNotAuthorized()
    {
        var holdingId = Guid.NewGuid();
        ArrangeCurrentUser(Guid.NewGuid(), isAuthorized: false);

        var result = await _sut.Handle(new GetColorsByHoldingQuery(holdingId), CancellationToken.None);

        result.IsFailure.Should().BeTrue();
        result.Error.Code.Should().Be("Color.NotAuthorized");
    }

    [Fact]
    public async Task Handle_WhenAuthorized_ReturnsTheColorsFromTheRepository()
    {
        var holdingId = Guid.NewGuid();
        ArrangeCurrentUser(Guid.NewGuid());

        var expected = new List<ColorDto> { new(Guid.NewGuid(), "Blue"), new(Guid.NewGuid(), "Red") };
        _colorRepository.GetByHoldingAsync(holdingId, Arg.Any<CancellationToken>()).Returns(expected);

        var result = await _sut.Handle(new GetColorsByHoldingQuery(holdingId), CancellationToken.None);

        result.IsSuccess.Should().BeTrue();
        result.Value.Should().BeEquivalentTo(expected);
    }

    [Fact]
    public async Task Handle_WhenHoldingHasNoColors_ReturnsAnEmptyList()
    {
        var holdingId = Guid.NewGuid();
        ArrangeCurrentUser(Guid.NewGuid());
        _colorRepository.GetByHoldingAsync(holdingId, Arg.Any<CancellationToken>())
            .Returns(new List<ColorDto>());

        var result = await _sut.Handle(new GetColorsByHoldingQuery(holdingId), CancellationToken.None);

        result.IsSuccess.Should().BeTrue();
        result.Value.Should().BeEmpty();
    }
}
