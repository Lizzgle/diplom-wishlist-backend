using Core.Exceptions;
using FluentAssertions;
using Identity.Application.Usecases.Account.Commands.ForgotPassword;
using Identity.Domain;
using Microsoft.AspNetCore.Identity;
using Moq;

namespace Identity.UnitTests.Usecases.Account;

public class ForgotPasswordHandlerTests
{
        private readonly Mock<UserManager<User>> _userManagerMock;
    private readonly ForgotPasswordHandler _handler;

    public ForgotPasswordHandlerTests()
    {
        var store = new Mock<IUserStore<User>>();
        _userManagerMock = new Mock<UserManager<User>>(store.Object, null, null, null, null, null, null, null, null);
        _handler = new ForgotPasswordHandler(_userManagerMock.Object);
    }

    [Fact]
    public async Task Handle_ShouldThrowNotFoundException_WhenUserDoesNotExist()
    {
        // Arrange
        var request = new ForgotPasswordRequest { Email = "nonexistent@example.com" };
        _userManagerMock.Setup(m => m.FindByEmailAsync(request.Email)).ReturnsAsync((User)null);

        // Act
        var act = async () => await _handler.Handle(request, CancellationToken.None);

        // Assert
        await act.Should().ThrowAsync<NotFoundException>().WithMessage("Not found entity. User not found");
    }

    [Fact]
    public async Task Handle_ShouldThrowNotFoundException_WhenUserEmailNotConfirmed()
    {
        // Arrange
        var user = new User { Email = "test@example.com", EmailConfirmed = false };
        var request = new ForgotPasswordRequest { Email = user.Email };
        _userManagerMock.Setup(m => m.FindByEmailAsync(request.Email)).ReturnsAsync(user);

        // Act
        var act = async () => await _handler.Handle(request, CancellationToken.None);

        // Assert
        await act.Should().ThrowAsync<NotFoundException>().WithMessage("Not found entity. User not found");
    }

    [Fact]
    public async Task Handle_ShouldReturnPasswordResetCode_WhenUserIsFoundAndEmailConfirmed()
    {
        // Arrange
        var user = new User { Email = "test@example.com", EmailConfirmed = true };
        var request = new ForgotPasswordRequest { Email = user.Email };
        var expectedCode = "reset-code";
        _userManagerMock.Setup(m => m.FindByEmailAsync(request.Email)).ReturnsAsync(user);
        _userManagerMock.Setup(m => m.GeneratePasswordResetTokenAsync(user)).ReturnsAsync(expectedCode);

        // Act
        var response = await _handler.Handle(request, CancellationToken.None);

        // Assert
        response.Code.Should().Be(expectedCode);
    }

}