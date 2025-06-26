using Core.Exceptions;
using FluentAssertions;
using Identity.Application.Usecases.Account.Commands.ConfirmEmail;
using Identity.Domain;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Logging;
using Moq;
using ArgumentException = Core.Exceptions.ArgumentException;

namespace Identity.UnitTests.Usecases.Account;

public class ConfirmEmailHandlerTests
{
    private readonly Mock<UserManager<User>> _userManagerMock;
    private readonly Mock<ILogger<ConfirmEmailHandler>> _loggerMock;
    private readonly ConfirmEmailHandler _handler;

    public ConfirmEmailHandlerTests()
    {
        var store = new Mock<IUserStore<User>>();
        _userManagerMock = new Mock<UserManager<User>>(store.Object, null, null, null, null, null, null, null, null);
        _loggerMock = new Mock<ILogger<ConfirmEmailHandler>>();
        _handler = new ConfirmEmailHandler(_userManagerMock.Object, _loggerMock.Object);
    }

    [Fact]
    public async Task Handle_ShouldThrowArgumentException_WhenEmailOrCodeIsNull()
    {
        // Arrange
        var request = new ConfirmEmailRequest { Email = null, Code = "code" };

        // Act
        var act = async () => await _handler.Handle(request, CancellationToken.None);

        // Assert
        await act.Should().ThrowAsync<ArgumentException>().WithMessage("Argument {0} is invalid. Invalid email confirmation request.");
    }

    [Fact]
    public async Task Handle_ShouldThrowArgumentException_WhenCodeIsNull()
    {
        // Arrange
        var request = new ConfirmEmailRequest { Email = "test@example.com", Code = null };

        // Act
        var act = async () => await _handler.Handle(request, CancellationToken.None);

        // Assert
        await act.Should().ThrowAsync<ArgumentException>().WithMessage("Argument {0} is invalid. Invalid email confirmation request.");
    }

    [Fact]
    public async Task Handle_ShouldThrowNotFoundException_WhenUserDoesNotExist()
    {
        // Arrange
        var request = new ConfirmEmailRequest { Email = "notfound@example.com", Code = "reset-code" };
        _userManagerMock.Setup(m => m.FindByEmailAsync(request.Email)).ReturnsAsync((User)null);

        // Act
        var act = async () => await _handler.Handle(request, CancellationToken.None);

        // Assert
        await act.Should().ThrowAsync<NotFoundException>().WithMessage("Not found entity. User not found.");
    }

    [Fact]
    public async Task Handle_ShouldThrowInvalidTokenException_WhenEmailConfirmationFails()
    {
        // Arrange
        var user = new User { Email = "test@example.com" };
        var request = new ConfirmEmailRequest { Email = user.Email, Code = "invalid-code" };
        _userManagerMock.Setup(m => m.FindByEmailAsync(request.Email)).ReturnsAsync(user);
        _userManagerMock.Setup(m => m.ConfirmEmailAsync(user, request.Code)).ReturnsAsync(IdentityResult.Failed(new IdentityError { Description = "Invalid token." }));

        // Act
        var act = async () => await _handler.Handle(request, CancellationToken.None);

        // Assert
        await act.Should().ThrowAsync<InvalidTokenException>();
    }

    [Fact]
    public async Task Handle_ShouldSucceed_WhenEmailIsConfirmed()
    {
        // Arrange
        var user = new User { Email = "test@example.com" };
        var request = new ConfirmEmailRequest { Email = user.Email, Code = "valid-code" };
        _userManagerMock.Setup(m => m.FindByEmailAsync(request.Email)).ReturnsAsync(user);
        _userManagerMock.Setup(m => m.ConfirmEmailAsync(user, request.Code)).ReturnsAsync(IdentityResult.Success);

        // Act
        await _handler.Handle(request, CancellationToken.None);

        // Assert
        _userManagerMock.Verify(m => m.ConfirmEmailAsync(user, request.Code), Times.Once);
    }
}