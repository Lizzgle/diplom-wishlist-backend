using Core.Exceptions;
using FluentAssertions;
using Identity.Application.Usecases.Account.Commands.ResetPassword;
using Identity.Domain;
using Microsoft.AspNetCore.Identity;
using Moq;
using ArgumentException = Core.Exceptions.ArgumentException;

namespace Identity.UnitTests.Usecases.Account;

public class ResetPasswordHandlerTests
{
    private readonly Mock<UserManager<User>> _userManagerMock;
    private readonly ResetPasswordHandler _handler;

    public ResetPasswordHandlerTests()
    {
        var store = new Mock<IUserStore<User>>();
        _userManagerMock = new Mock<UserManager<User>>(store.Object, null, null, null, null, null, null, null, null);
        _handler = new ResetPasswordHandler(_userManagerMock.Object);
    }

    [Fact]
    public async Task Handle_ShouldThrowNotFoundException_WhenUserDoesNotExist()
    {
        // Arrange
        var request = new ResetPasswordRequest { Email = "notfound@example.com", Password = "password", ConfirmPassword = "password", Code = "code" };
        _userManagerMock.Setup(m => m.FindByEmailAsync(request.Email)).ReturnsAsync((User)null);

        // Act
        var act = async () => await _handler.Handle(request, CancellationToken.None);

        // Assert
        await act.Should().ThrowAsync<NotFoundException>().WithMessage("Not found entity. User not found");
    }

    [Fact]
    public async Task Handle_ShouldThrowArgumentException_WhenPasswordsDoNotMatch()
    {
        // Arrange
        var user = new User { Email = "test@example.com" };
        var request = new ResetPasswordRequest
        {
            Email = "test@example.com", 
            Password = "password", 
            ConfirmPassword = "different", 
            Code = "code"
        };
        _userManagerMock.Setup(m => m.FindByEmailAsync(request.Email)).ReturnsAsync(user);

        // Act
        var act = async () => await _handler.Handle(request, CancellationToken.None);

        // Assert
        await act.Should().ThrowAsync<ArgumentException>().WithMessage("Argument {0} is invalid. Passwords do not match");
    }
    
    [Fact]
    public async Task Handle_ShouldThrowInvalidTokenException_WhenPasswordResetFails()
    {
        // Arrange
        var user = new User { Email = "test@example.com" };
        var request = new ResetPasswordRequest
        {
            Email = user.Email,
            Code = "reset-code",
            Password = "newPassword1!",
            ConfirmPassword = "newPassword1!"
        };

        _userManagerMock.Setup(m => m.FindByEmailAsync(request.Email)).ReturnsAsync(user);
        
        _userManagerMock.Setup(m => m.ResetPasswordAsync(user, request.Code, request.Password))
            .ReturnsAsync(IdentityResult.Failed(new IdentityError { Description = "Password reset failed." }));

        var act = async () => await _handler.Handle(request, CancellationToken.None);

        // Assert
        await act.Should().ThrowAsync<InvalidTokenException>().WithMessage("Invalid token.");
    }

    [Fact]
    public async Task Handle_ShouldCallResetPasswordAsync_WhenUserExists()
    {
        // Arrange
        var user = new User { Email = "test@example.com", PasswordHash = "password" };
        var request = new ResetPasswordRequest { Email = "test@example.com", Password = "password", ConfirmPassword = "password", Code = "code" };
        _userManagerMock.Setup(m => m.FindByEmailAsync(request.Email)).ReturnsAsync(user);
        _userManagerMock.Setup(m => m.ResetPasswordAsync(user, request.Code, request.Password)).ReturnsAsync(IdentityResult.Success);

        // Act
        await _handler.Handle(request, CancellationToken.None);

        // Assert
        _userManagerMock.Verify(m => m.ResetPasswordAsync(user, request.Code, request.Password), Times.Once);
    }
}