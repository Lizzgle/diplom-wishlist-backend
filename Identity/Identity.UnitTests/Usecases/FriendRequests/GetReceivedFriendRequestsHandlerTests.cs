using AutoMapper;
using Core.Exceptions;
using FluentAssertions;
using Identity.Application.Usecases.FriendRequests.Queries.GetReceivedFriendRequests;
using Identity.Contracts.Repositories;
using Identity.Domain;
using Microsoft.AspNetCore.Identity;
using Moq;

namespace Identity.UnitTests.Usecases.FriendRequests;

public class GetReceivedFriendRequestsHandlerTests
{
    private readonly Mock<UserManager<User>> _userManagerMock;
    private readonly Mock<IFriendRequestRepository> _friendRequestRepositoryMock;
    private readonly IMapper _mapper;
    private readonly GetReceivedFriendRequestsHandler _handler;

    public GetReceivedFriendRequestsHandlerTests()
    {
        var store = new Mock<IUserStore<User>>();
        _userManagerMock = new Mock<UserManager<User>>(store.Object, null, null, null, null, null, null, null, null);
        _friendRequestRepositoryMock = new Mock<IFriendRequestRepository>();

        var config = new MapperConfiguration(cfg =>
        {
            cfg.CreateMap<FriendRequest, FriendRequestDto>()
                .ForMember(dest => dest.Id, opt => opt.MapFrom(src => src.Id))
                .ForMember(dest => dest.SenderId, opt => opt.MapFrom(src => src.SenderId))
                .ForMember(dest => dest.SenderEmail, opt => opt.MapFrom(src => src.Sender.Email))
                .ForMember(dest => dest.SenderName, opt => opt.MapFrom(src => src.Sender.UserName));
        });
        _mapper = config.CreateMapper();
        
        _handler = new GetReceivedFriendRequestsHandler(_friendRequestRepositoryMock.Object, _userManagerMock.Object, _mapper);
    }

    [Fact]
    public async Task Handle_ShouldThrowNotFoundException_WhenUserNotFound()
    {
        // Arrange
        var request = new GetReceivedFriendRequest { UserId = "user-id" };
        _userManagerMock.Setup(m => m.FindByIdAsync(request.UserId)).ReturnsAsync((User)null);

        // Act
        var act = async () => await _handler.Handle(request, CancellationToken.None);

        // Assert
        await act.Should().ThrowAsync<NotFoundException>().WithMessage("Not found entity. User not found");
    }

    [Fact]
    public async Task Handle_ShouldReturnEmptyList_WhenNoFriendRequestsFound()
    {
        // Arrange
        var request = new GetReceivedFriendRequest { UserId = "user-id" };
        var user = new User { Id = "user-id" };
        _userManagerMock.Setup(m => m.FindByIdAsync(request.UserId)).ReturnsAsync(user);
        _friendRequestRepositoryMock.Setup(m => m.GetReceivedFriendRequestsAsync(request.UserId, CancellationToken.None))
            .ReturnsAsync(new List<FriendRequest>());

        // Act
        var result = await _handler.Handle(request, CancellationToken.None);

        // Assert
        result.FriendRequests.Should().BeEmpty();
    }

    [Fact]
    public async Task Handle_ShouldReturnFriendRequests_WhenRequestsExist()
    {
        // Arrange
        var request = new GetReceivedFriendRequest { UserId = "user-id" };
        var user = new User { Id = "user-id" };
        var friendRequests = new List<FriendRequest>
        {
            new FriendRequest
            {
                Id = Guid.NewGuid(),
                SenderId = "sender-id",
                ReceiverId = "user-id",
                Sender = new User()
                {
                    Id = "sender-id",
                    UserName = "sender-name",
                    Email = "sender-email"
                }
            }
        };

        _userManagerMock.Setup(m => m.FindByIdAsync(request.UserId)).ReturnsAsync(user);
        _friendRequestRepositoryMock.Setup(m => m.GetReceivedFriendRequestsAsync(request.UserId, CancellationToken.None))
            .ReturnsAsync(friendRequests);

        // Act
        var result = await _handler.Handle(request, CancellationToken.None);

        // Assert
        result.FriendRequests.Should().HaveCount(1);
        result.FriendRequests[0].SenderId.Should().Be(friendRequests[0].SenderId);
        result.FriendRequests[0].SenderName.Should().Be(friendRequests[0].Sender.UserName);
        result.FriendRequests[0].SenderEmail.Should().Be(friendRequests[0].Sender.Email);
    }
}