using Core.Exceptions;
using Core.Models;
using FluentAssertions;
using Identity.Application.Providers;
using Identity.Application.Usecases.Account.Commands.Login;
using Identity.Domain;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Moq;

namespace Identity.UnitTests.Usecases.Account;

public class LoginHandlerTests
{
    private readonly Mock<IJwtProvider> _jwtProviderMock;
    private readonly Mock<UserManager<User>> _userManagerMock;
    private readonly Mock<SignInManager<User>> _signInManagerMock;
    private readonly LoginHandler _handler;

    public LoginHandlerTests()
    {
        var store = new Mock<IUserStore<User>>();
        var contextAccessor = new Mock<IHttpContextAccessor>();
        var claimsFactory = new Mock<IUserClaimsPrincipalFactory<User>>();
        _userManagerMock = new Mock<UserManager<User>>(store.Object, null, null, null, null, null, null, null, null);
        _signInManagerMock = new Mock<SignInManager<User>>(
            _userManagerMock.Object, contextAccessor.Object, claimsFactory.Object, null, null, null, null);
        _jwtProviderMock = new Mock<IJwtProvider>();
        _handler = new LoginHandler(_jwtProviderMock.Object, _userManagerMock.Object, _signInManagerMock.Object);
    }

    [Fact]
    public async Task Handle_ShouldThrowNotFoundException_WhenUserDoesNotExist()
    {
        // Arrange
        var request = new LoginRequest
        {
            UniqueProperty = "notfound@example.com", 
            Password = "password", 
            DeviceType = DeviceType.Web
        };
        
        _userManagerMock.Setup(m => m.FindByEmailAsync(request.UniqueProperty)).ReturnsAsync((User)null);

        // Act
        var act = async () => await _handler.Handle(request, CancellationToken.None);

        // Assert
        await act.Should().ThrowAsync<NotFoundException>()
            .WithMessage("Not found entity. User does not exist.");
    }

    [Fact]
    public async Task Handle_ShouldThrowInvalidAuthException_WhenPasswordIsIncorrect()
    {
        // Arrange
        var user = new User
        {
            Email = "test@example.com", 
            EmailConfirmed = true
        };
        
        var request = new LoginRequest
        {
            UniqueProperty = "test@example.com", 
            Password = "wrongpassword", 
            DeviceType = DeviceType.Web
        };
        
        _userManagerMock.Setup(m => m.FindByEmailAsync(request.UniqueProperty)).ReturnsAsync(user);
        _signInManagerMock
            .Setup(m => m.PasswordSignInAsync(user, request.Password, false, false))
            .ReturnsAsync(SignInResult.Failed);

        // Act
        var act = async () => await _handler.Handle(request, CancellationToken.None);

        // Assert
        await act.Should().ThrowAsync<InvalidAuthException>()
            .WithMessage("Invalid credentials. Username/email or password is incorrect.");
    }

    [Fact]
    public async Task Handle_ShouldReturnJwtAndRefreshToken_WhenLoginIsSuccessful()
    {
        // Arrange
        var user = new User { Id = "123", Email = "test@example.com", EmailConfirmed = true };
        
        var request = new LoginRequest 
            { UniqueProperty = "test@example.com", 
                Password = "password", DeviceType = DeviceType.Web };
        _userManagerMock.Setup(m => m.FindByEmailAsync(request.UniqueProperty)).ReturnsAsync(user);
        _signInManagerMock.Setup(m => m.PasswordSignInAsync(user, request.Password, false, false)).ReturnsAsync(SignInResult.Success);
        _jwtProviderMock
            .Setup(m => m.GenerateJwtAsync(user, It.IsAny<CancellationToken>()))
            .ReturnsAsync("jwt-token");
        _jwtProviderMock
            .Setup(m => m.GetRefreshTokenForDeviceAsync(user.Id, request.DeviceType, It.IsAny<CancellationToken>()))
            .ReturnsAsync((string)null);
        _jwtProviderMock.Setup(m => m.GenerateRefreshToken(user.Id, request.DeviceType, It.IsAny<CancellationToken>())).ReturnsAsync("refresh-token");

        // Act
        var response = await _handler.Handle(request, CancellationToken.None);

        // Assert
        response.JwtToken.Should().Be("jwt-token");
        response.RefreshToken.Should().Be("refresh-token");
        response.Email.Should().Be(user.Email);
    }
}