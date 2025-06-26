using AutoMapper;
using Core.Exceptions;
using FluentAssertions;
using Identity.Application.Usecases.FriendRequests.Queries.GetSentFriendRequests;
using Identity.Contracts.Repositories;
using Identity.Domain;
using Microsoft.AspNetCore.Identity;
using Moq;

namespace Identity.UnitTests.Usecases.FriendRequests;

public class GetSentFriendRequestsHandlerTests
{
    private readonly Mock<UserManager<User>> _userManagerMock;
    private readonly Mock<IFriendRequestRepository> _friendRequestRepositoryMock;
    private readonly IMapper _mapper;
    private readonly GetSentFriendRequestsHandler _handler;

    public GetSentFriendRequestsHandlerTests()
    {
        var store = new Mock<IUserStore<User>>();
        _userManagerMock = new Mock<UserManager<User>>(store.Object, null, null, null, null, null, null, null, null);
        _friendRequestRepositoryMock = new Mock<IFriendRequestRepository>();

        var config = new MapperConfiguration(cfg =>
        {
            cfg.CreateMap<FriendRequest, FriendRequestDto>()
                .ForMember(dest => dest.Id, opt => opt.MapFrom(src => src.Id))
                .ForMember(dest => dest.ReceiverId, opt => opt.MapFrom(src => src.ReceiverId))
                .ForMember(dest => dest.ReceiverEmail, opt => opt.MapFrom(src => src.Receiver.Email))
                .ForMember(dest => dest.ReceiverName, opt => opt.MapFrom(src => src.Receiver.UserName));
        });
        _mapper = config.CreateMapper();
        
        _handler = new GetSentFriendRequestsHandler(_friendRequestRepositoryMock.Object, _userManagerMock.Object, _mapper);
    }

    [Fact]
    public async Task Handle_ShouldThrowNotFoundException_WhenUserNotFound()
    {
        // Arrange
        var request = new GetSentFriendRequest { UserId = "user-id" };
        _userManagerMock.Setup(m => m.FindByIdAsync(request.UserId)).ReturnsAsync((User)null);

        // Act
        var act = async () => await _handler.Handle(request, CancellationToken.None);

        // Assert
        await act.Should().ThrowAsync<NotFoundException>().WithMessage("Not found entity. User not found");
    }

    [Fact]
    public async Task Handle_ShouldReturnEmptyList_WhenNoSentFriendRequestsFound()
    {
        // Arrange
        var request = new GetSentFriendRequest { UserId = "user-id" };
        var user = new User { Id = "user-id" };
        _userManagerMock.Setup(m => m.FindByIdAsync(request.UserId)).ReturnsAsync(user);
        _friendRequestRepositoryMock.Setup(m => m.GetSentFriendRequestsAsync(request.UserId, CancellationToken.None))
            .ReturnsAsync(new List<FriendRequest>());

        // Act
        var result = await _handler.Handle(request, CancellationToken.None);

        // Assert
        result.FriendRequests.Should().BeEmpty();
    }

    [Fact]
    public async Task Handle_ShouldReturnSentFriendRequests_WhenRequestsExist()
    {
        // Arrange
        var request = new GetSentFriendRequest { UserId = "user-id" };
        var user = new User { Id = "user-id" };
        var receiver = new User { Id = "receiver-id", Email = "receiver@example.com", UserName = "Receiver" };
        
        var friendRequests = new List<FriendRequest>
        {
            new FriendRequest
            {
                Id = Guid.NewGuid(),
                SenderId = user.Id,
                ReceiverId = receiver.Id,
                Receiver = receiver
            }
        };

        _userManagerMock.Setup(m => m.FindByIdAsync(request.UserId)).ReturnsAsync(user);
        _friendRequestRepositoryMock.Setup(m => m.GetSentFriendRequestsAsync(request.UserId, CancellationToken.None))
            .ReturnsAsync(friendRequests);

        // Act
        var result = await _handler.Handle(request, CancellationToken.None);

        // Assert
        result.FriendRequests.Should().HaveCount(1);
        result.FriendRequests[0].ReceiverId.Should().Be(friendRequests[0].ReceiverId);
        result.FriendRequests[0].ReceiverName.Should().Be(receiver.UserName);
        result.FriendRequests[0].ReceiverEmail.Should().Be(receiver.Email);
    }
}