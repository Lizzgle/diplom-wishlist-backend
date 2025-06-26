using AutoMapper;
using Core.Exceptions;
using Identity.Application.Usecases.Friends.Queries.GetFriends;
using Identity.Contracts.Repositories;
using Identity.Domain;
using Microsoft.AspNetCore.Identity;
using Moq;

namespace Identity.UnitTests.Usecases.Friends;

public class GetFriendsHandlerTests
{
    private readonly Mock<UserManager<User>> _userManagerMock;
    private readonly Mock<IFriendshipRepository> _friendshipRepositoryMock;
    private readonly IMapper _mapper;
    private readonly GetFriendsHandler _handler;

    public GetFriendsHandlerTests()
    {
        // Настройка маппера
        var configuration = new MapperConfiguration(cfg =>
        {
            cfg.CreateMap<User, FriendDto>()
                .ForMember(dext => dext.FriendId, opt => opt.MapFrom(src => src.Id))
                .ForMember(dext => dext.FriendEmail, opt => opt.MapFrom(src => src.Email))
                .ForMember(dext => dext.FriendName, opt => opt.MapFrom(src => src.UserName));
        });
        _mapper = configuration.CreateMapper();

        _userManagerMock = new Mock<UserManager<User>>(
            new Mock<IUserStore<User>>().Object,
            null, null, null, null, null, null, null, null
        );
        _friendshipRepositoryMock = new Mock<IFriendshipRepository>();

        _handler = new GetFriendsHandler(_userManagerMock.Object, _friendshipRepositoryMock.Object, _mapper);
    }

    [Fact]
    public async Task Handle_ShouldReturnFriends_WhenUserFound()
    {
        // Arrange
        var request = new GetFriendsRequest { UserId = "1" };
        var user = new User { Id = "1", UserName = "testuser", Email = "testuser@example.com" };

        var friends = new List<User>
        {
            new User { Id = "2", UserName = "friend2", Email = "friend2@example.com" },
            new User { Id = "3", UserName = "friend3", Email = "friend3@example.com" }
        };

        _userManagerMock
            .Setup(um => um.FindByIdAsync(request.UserId))
            .ReturnsAsync(user);

        _friendshipRepositoryMock
            .Setup(fr => fr.GetFriendsByUserIdAsync(request.UserId, It.IsAny<CancellationToken>()))
            .ReturnsAsync(friends);

        // Act
        var result = await _handler.Handle(request, CancellationToken.None);

        // Assert
        Assert.NotNull(result);
        Assert.Equal(2, result.Friends.Count);
        Assert.Equal("friend2", result.Friends[0].FriendName);
        Assert.Equal("friend3", result.Friends[1].FriendName);
    }

    [Fact]
    public async Task Handle_ShouldThrowNotFoundException_WhenUserNotFound()
    {
        // Arrange
        var request = new GetFriendsRequest { UserId = "1" };

        _userManagerMock
            .Setup(um => um.FindByIdAsync(request.UserId))
            .ReturnsAsync((User)null); // User is not found

        // Act & Assert
        await Assert.ThrowsAsync<NotFoundException>(() => _handler.Handle(request, CancellationToken.None));
    }

    [Fact]
    public async Task Handle_ShouldReturnEmptyList_WhenNoFriends()
    {
        // Arrange
        var request = new GetFriendsRequest { UserId = "1" };
        var user = new User { Id = "1", UserName = "testuser", Email = "testuser@example.com" };

        var friends = new List<User>();

        _userManagerMock
            .Setup(um => um.FindByIdAsync(request.UserId))
            .ReturnsAsync(user);

        _friendshipRepositoryMock
            .Setup(fr => fr.GetFriendsByUserIdAsync(request.UserId, It.IsAny<CancellationToken>()))
            .ReturnsAsync(friends);

        // Act
        var result = await _handler.Handle(request, CancellationToken.None);

        // Assert
        Assert.NotNull(result);
        Assert.Empty(result.Friends); // Should return an empty list
    }
}