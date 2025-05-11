using AutoMapper;
using Identity.Application.Usecases.Users.Queries.GetUserByEmailOrName;
using Identity.Contracts.Repositories;
using Identity.Domain;
using Moq;

namespace Identity.UnitTests.Usecases.Users;

public class GetUsersByEmailOrNameHandlerTests
{
    private readonly Mock<IUserRepository> _userRepositoryMock;
    private readonly IMapper _mapper;
    private readonly GetUsersByEmailOrNameHandler _handler;

    public GetUsersByEmailOrNameHandlerTests()
    {
        var configuration = new MapperConfiguration(cfg =>
        {
            cfg.CreateMap<User, UserDto>();
        });
        _mapper = configuration.CreateMapper();

        _userRepositoryMock = new Mock<IUserRepository>();
        _handler = new GetUsersByEmailOrNameHandler(_userRepositoryMock.Object, _mapper);
    }

    [Fact]
    public async Task Handle_ShouldReturnUsers_WhenFound()
    {
        // Arrange
        var request = new GetUsersByEmailOrNameRequest { Query = "test" };
        var users = new List<User>
        {
            new User { Id = "1", UserName = "user1", Email = "user1@example.com" },
            new User { Id = "2", UserName = "user2", Email = "user2@example.com" }
        };

        _userRepositoryMock
            .Setup(r => r.GetUsersByEmailOrNameAsync(request.Query, It.IsAny<CancellationToken>()))
            .ReturnsAsync(users);

        // Act
        var result = await _handler.Handle(request, CancellationToken.None);

        // Assert
        Assert.NotNull(result);
        Assert.Equal(2, result.Users.Count);
        Assert.Equal("user1@example.com", result.Users[0].Email);
        Assert.Equal("user2@example.com", result.Users[1].Email);
    }

    [Fact]
    public async Task Handle_ShouldReturnEmptyList_WhenNoUsersFound()
    {
        // Arrange
        var request = new GetUsersByEmailOrNameRequest { Query = "nonexistent" };
        var users = new List<User>();

        _userRepositoryMock
            .Setup(r => r.GetUsersByEmailOrNameAsync(request.Query, It.IsAny<CancellationToken>()))
            .ReturnsAsync(users);

        // Act
        var result = await _handler.Handle(request, CancellationToken.None);

        // Assert
        Assert.NotNull(result);
        Assert.Empty(result.Users);
    }
}