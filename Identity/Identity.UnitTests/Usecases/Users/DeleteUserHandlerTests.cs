using Core.Exceptions;
using FluentAssertions;
using Identity.Application.Usecases.Users.Commands.DeleteUser;
using Identity.Domain;
using Microsoft.AspNetCore.Identity;
using Moq;

namespace Identity.UnitTests.Usecases.Users;

public class DeleteUserHandlerTests
{
    private readonly Mock<UserManager<User>> _userManagerMock;
    private readonly DeleteUserHandler _handler;

    public DeleteUserHandlerTests()
    {
        var store = new Mock<IUserStore<User>>();
        _userManagerMock = new Mock<UserManager<User>>(store.Object, null, null, null, null, null, null, null, null);
        _handler = new DeleteUserHandler(_userManagerMock.Object);
    }

    [Fact]
    public async Task Handle_ShouldThrowNotFoundException_WhenUserNotFound()
    {
        // Arrange
        var request = new DeleteUserRequest { Id = "non-existing-id" };
        _userManagerMock.Setup(m => m.FindByIdAsync(request.Id)).ReturnsAsync((User)null);

        // Act
        var act = async () => await _handler.Handle(request, CancellationToken.None);

        // Assert
        await act.Should().ThrowAsync<NotFoundException>()
            .WithMessage($"Not found entity. User with id = {request.Id} not found");
    }

    [Fact]
    public async Task Handle_ShouldDeleteUser_WhenUserExists()
    {
        // Arrange
        var request = new DeleteUserRequest { Id = "existing-id" };
        var user = new User { Id = "existing-id" };
        _userManagerMock.Setup(m => m.FindByIdAsync(request.Id)).ReturnsAsync(user);
        _userManagerMock.Setup(m => m.DeleteAsync(user)).ReturnsAsync(IdentityResult.Success);

        // Act
        await _handler.Handle(request, CancellationToken.None);

        // Assert
        _userManagerMock.Verify(m => m.DeleteAsync(user), Times.Once);
    }
}