using Core.Exceptions;
using Core.Models;
using FluentAssertions;
using Identity.Application.Providers;
using Identity.Application.Usecases.Account.Commands.RefreshToken;
using Identity.Domain;
using Microsoft.AspNetCore.Identity;
using Moq;

namespace Identity.UnitTests.Usecases.Account;

public class RefreshTokenHandlerTests
{
    private readonly Mock<UserManager<User>> _userManagerMock;
    private readonly Mock<IJwtProvider> _jwtProviderMock;
    private readonly RefreshTokenHandler _handler;

    public RefreshTokenHandlerTests()
    {
        var store = new Mock<IUserStore<User>>();
        _userManagerMock = new Mock<UserManager<User>>(store.Object, null, null, null, null, null, null, null, null);
        _jwtProviderMock = new Mock<IJwtProvider>();
        _handler = new RefreshTokenHandler(_userManagerMock.Object, _jwtProviderMock.Object);
    }

    [Fact]
    public async Task Handle_ShouldThrowNotFoundException_WhenUserIsNotFound()
    {
        // Arrange
        var request = new RefreshTokenRequest { Id = "nonexistent-id", RefreshToken = "refresh-token", DeviceType = DeviceType.Mobile };
        _userManagerMock.Setup(m => m.FindByIdAsync(request.Id)).ReturnsAsync((User)null);

        // Act
        var act = async () => await _handler.Handle(request, CancellationToken.None);

        // Assert
        await act.Should().ThrowAsync<NotFoundException>().WithMessage($"Not found entity. User by id = {request.Id} not found.");
    }

    [Fact]
    public async Task Handle_ShouldThrowInvalidAuthException_WhenRefreshTokenIsInvalid()
    {
        // Arrange
        var user = new User { Id = "user-id", Email = "test@example.com" };
        var request = new RefreshTokenRequest { Id = user.Id, RefreshToken = "invalid-token", DeviceType = DeviceType.Mobile };
        _userManagerMock.Setup(m => m.FindByIdAsync(user.Id)).ReturnsAsync(user);
        _jwtProviderMock.Setup(m => m.GetRefreshTokenForDeviceAsync(user.Id, request.DeviceType, CancellationToken.None)).ReturnsAsync((string)null);

        // Act
        var act = async () => await _handler.Handle(request, CancellationToken.None);

        // Assert
        await act.Should().ThrowAsync<InvalidAuthException>().WithMessage("Invalid credentials. Invalid refresh token.");
    }

    [Fact]
    public async Task Handle_ShouldReturnNewJwt_WhenRefreshTokenIsValid()
    {
        // Arrange
        var user = new User { Id = "user-id", Email = "test@example.com" };
        var request = new RefreshTokenRequest { Id = user.Id, RefreshToken = "valid-refresh-token", DeviceType = DeviceType.Mobile };
        var newJwt = "new-jwt-token";
        
        _userManagerMock.Setup(m => m.FindByIdAsync(user.Id)).ReturnsAsync(user);
        _jwtProviderMock.Setup(m => m.GetRefreshTokenForDeviceAsync(user.Id, request.DeviceType, CancellationToken.None)).ReturnsAsync("valid-refresh-token");
        _jwtProviderMock.Setup(m => m.GenerateJwtAsync(user, CancellationToken.None)).ReturnsAsync(newJwt);

        // Act
        var response = await _handler.Handle(request, CancellationToken.None);

        // Assert
        response.Should().NotBeNull();
        response.Email.Should().Be(user.Email);
        response.JwtToken.Should().Be(newJwt);
    }
}