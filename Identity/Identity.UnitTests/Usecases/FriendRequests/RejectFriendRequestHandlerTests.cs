using Core.Exceptions;
using FluentAssertions;
using Identity.Application.Usecases.FriendRequests.Commands.RejectFriendRequest;
using Identity.Contracts.Repositories;
using Identity.Domain;
using Identity.Domain.Enums;
using Microsoft.AspNetCore.Identity;
using Moq;

namespace Identity.UnitTests.Usecases.FriendRequests;

public class RejectFriendRequestHandlerTests
{
    private readonly Mock<UserManager<User>> _userManagerMock;
    private readonly Mock<IFriendRequestRepository> _friendRequestRepositoryMock;
    private readonly RejectFriendRequestHandler _handler;

    public RejectFriendRequestHandlerTests()
    {
        var store = new Mock<IUserStore<User>>();
        _userManagerMock = new Mock<UserManager<User>>(store.Object, null, null, null, null, null, null, null, null);
        _friendRequestRepositoryMock = new Mock<IFriendRequestRepository>();
        _handler = new RejectFriendRequestHandler(_friendRequestRepositoryMock.Object, _userManagerMock.Object);
    }

    [Fact]
    public async Task Handle_ShouldThrowNotFoundException_WhenUserIsNotFound()
    {
        // Arrange
        var request = new RejectFriendRequest { Id = Guid.NewGuid(), ReceiverId = "nonexistent-user-id" };
        _userManagerMock.Setup(m => m.FindByIdAsync(request.ReceiverId)).ReturnsAsync((User)null);

        // Act
        var act = async () => await _handler.Handle(request, CancellationToken.None);

        // Assert
        await act.Should().ThrowAsync<NotFoundException>().WithMessage($"Not found entity. {request.ReceiverId} not found");
    }

    [Fact]
    public async Task Handle_ShouldThrowNotFoundException_WhenFriendRequestIsNotFound()
    {
        // Arrange
        var user = new User { Id = "receiver-id" };
        var request = new RejectFriendRequest { Id = Guid.NewGuid(), ReceiverId = user.Id };
        _userManagerMock.Setup(m => m.FindByIdAsync(request.ReceiverId)).ReturnsAsync(user);
        _friendRequestRepositoryMock.Setup(m => m.GetByIdAsync(request.Id, CancellationToken.None)).ReturnsAsync((FriendRequest)null);

        // Act
        var act = async () => await _handler.Handle(request, CancellationToken.None);

        // Assert
        await act.Should().ThrowAsync<NotFoundException>().WithMessage("Not found entity. Friend request not found");
    }

    [Fact]
    public async Task Handle_ShouldThrowForbiddenException_WhenReceiverIdDoesNotMatch()
    {
        // Arrange
        var user = new User { Id = "receiver-id" };
        var request = new RejectFriendRequest { Id = Guid.NewGuid(), ReceiverId = user.Id };
        var friendRequest = new FriendRequest
        {
            ReceiverId = "other-receiver-id",
            SenderId = "sender-id",
        };
        _userManagerMock.Setup(m => m.FindByIdAsync(request.ReceiverId)).ReturnsAsync(user);
        _friendRequestRepositoryMock.Setup(m => m.GetByIdAsync(request.Id, CancellationToken.None)).ReturnsAsync(friendRequest);

        // Act
        var act = async () => await _handler.Handle(request, CancellationToken.None);

        // Assert
        await act.Should().ThrowAsync<ForbiddenException>().WithMessage("You have not access to this action. You cannot accept this request");
    }

    [Fact]
    public async Task Handle_ShouldSuccessfullyRejectFriendRequest_WhenValid()
    {
        // Arrange
        var user = new User { Id = "receiver-id", Email = "receiver@example.com" };
        var sender = new User { Id = "sender-id", Email = "sender@example.com" };
        var request = new RejectFriendRequest { Id = Guid.NewGuid(), ReceiverId = user.Id };
        var friendRequest = new FriendRequest
        {
            Id = request.Id,
            SenderId = sender.Id,
            ReceiverId = user.Id,
            Status = FriendRequestStatus.Pending,
            RespondedAt = null
        };

        _userManagerMock.Setup(m => m.FindByIdAsync(user.Id)).ReturnsAsync(user);
        _friendRequestRepositoryMock.Setup(m => m.GetByIdAsync(request.Id, CancellationToken.None)).ReturnsAsync(friendRequest);

        // Act
        await _handler.Handle(request, CancellationToken.None);

        // Assert
        _friendRequestRepositoryMock.Verify(m => m.UpdateAsync(It.Is<FriendRequest>(fr => fr.Status == FriendRequestStatus.Rejected), CancellationToken.None), Times.Once);
    }
}