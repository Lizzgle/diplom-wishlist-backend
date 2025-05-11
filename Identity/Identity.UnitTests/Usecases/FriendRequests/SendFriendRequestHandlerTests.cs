using Core.Exceptions;
using FluentAssertions;
using Identity.Application.Usecases.FriendRequests.Commands.SendFriendRequest;
using Identity.Contracts.Repositories;
using Identity.Domain;
using Microsoft.AspNetCore.Identity;
using Moq;
using ArgumentException = Core.Exceptions.ArgumentException;

namespace Identity.UnitTests.Usecases.FriendRequests;

public class SendFriendRequestHandlerTests
{
    private readonly Mock<UserManager<User>> _userManagerMock;
    private readonly Mock<IFriendRequestRepository> _friendRequestRepositoryMock;
    private readonly SendFriendRequestHandler _handler;

    public SendFriendRequestHandlerTests()
    {
        var store = new Mock<IUserStore<User>>();
        _userManagerMock = new Mock<UserManager<User>>(store.Object, null, null, null, null, null, null, null, null);
        _friendRequestRepositoryMock = new Mock<IFriendRequestRepository>();
        _handler = new SendFriendRequestHandler(_friendRequestRepositoryMock.Object, _userManagerMock.Object);
    }

    [Fact]
    public async Task Handle_ShouldThrowArgumentException_WhenSenderAndReceiverAreTheSame()
    {
        // Arrange
        var request = new SendFriendRequest { SenderId = "user-id", ReceiverId = "user-id" };

        // Act
        var act = async () => await _handler.Handle(request, CancellationToken.None);

        // Assert
        await act.Should().ThrowAsync<ArgumentException>().WithMessage("Argument {0} is invalid. You cannot send a friend request to yourself");
    }

    [Fact]
    public async Task Handle_ShouldThrowAlreadyExistException_WhenFriendRequestAlreadyExists()
    {
        // Arrange
        var request = new SendFriendRequest { SenderId = "sender-id", ReceiverId = "receiver-id" };
        _friendRequestRepositoryMock.Setup(m => m.IsFriendRequestExistsAsync(request.SenderId, request.ReceiverId, CancellationToken.None)).ReturnsAsync(true);

        // Act
        var act = async () => await _handler.Handle(request, CancellationToken.None);

        // Assert
        await act.Should().ThrowAsync<AlreadyExistException>().WithMessage("Already exist entity. Friend request already exists");
    }

    [Fact]
    public async Task Handle_ShouldThrowNotFoundException_WhenSenderNotFound()
    {
        // Arrange
        var request = new SendFriendRequest { SenderId = "sender-id", ReceiverId = "receiver-id" };
        _userManagerMock.Setup(m => m.FindByIdAsync(request.SenderId)).ReturnsAsync((User)null);

        // Act
        var act = async () => await _handler.Handle(request, CancellationToken.None);

        // Assert
        await act.Should().ThrowAsync<NotFoundException>().WithMessage($"Not found entity. User with id = {request.SenderId} not found");
    }

    [Fact]
    public async Task Handle_ShouldThrowNotFoundException_WhenReceiverNotFound()
    {
        // Arrange
        var request = new SendFriendRequest { SenderId = "sender-id", ReceiverId = "receiver-id" };
        var sender = new User { Id = "sender-id" };
        _userManagerMock.Setup(m => m.FindByIdAsync(request.SenderId)).ReturnsAsync(sender);
        _userManagerMock.Setup(m => m.FindByIdAsync(request.ReceiverId)).ReturnsAsync((User)null);

        // Act
        var act = async () => await _handler.Handle(request, CancellationToken.None);

        // Assert
        await act.Should().ThrowAsync<NotFoundException>().WithMessage($"Not found entity. User with id = {request.ReceiverId} not found");
    }

    [Fact]
    public async Task Handle_ShouldSuccessfullySendFriendRequest_WhenValid()
    {
        // Arrange
        var request = new SendFriendRequest { SenderId = "sender-id", ReceiverId = "receiver-id" };
        var sender = new User { Id = "sender-id" };
        var receiver = new User { Id = "receiver-id" };

        _userManagerMock.Setup(m => m.FindByIdAsync(request.SenderId)).ReturnsAsync(sender);
        _userManagerMock.Setup(m => m.FindByIdAsync(request.ReceiverId)).ReturnsAsync(receiver);
        _friendRequestRepositoryMock.Setup(m => m.IsFriendRequestExistsAsync(request.SenderId, request.ReceiverId, CancellationToken.None)).ReturnsAsync(false);

        // Act
        await _handler.Handle(request, CancellationToken.None);

        // Assert
        _friendRequestRepositoryMock.Verify(m => m.AddAsync(It.Is<FriendRequest>(fr => fr.SenderId == request.SenderId && fr.ReceiverId == request.ReceiverId), CancellationToken.None), Times.Once);
    }
}