using AutoMapper;
using Moq;
using Wishlist.Application.Usecases.Wishes.Queries.GetBookedByUser;
using Wishlist.Contracts.Repositories;
using Wishlist.Domain;

namespace Wishlist.UnitTests.Usecases.Wishes;

public class GetBookedWishesByUserHandlerTests
{
    private readonly Mock<IWishRepository> _mockWishRepository;
    private readonly Mock<IMapper> _mockMapper;
    private readonly GetBookedWishesByUserHandler _handler;

    public GetBookedWishesByUserHandlerTests()
    {
        _mockWishRepository = new Mock<IWishRepository>();
        _mockMapper = new Mock<IMapper>();
        _handler = new GetBookedWishesByUserHandler(_mockWishRepository.Object, _mockMapper.Object);
    }

    [Fact]
    public async Task Handle_GivenValidRequest_ReturnsBookedWishes()
    {
        // Arrange
        var userId = "user123";

        var wishes = new List<Wish>
        {
            new Wish
            {
                Id = Guid.Parse("7e0b9e0e-5306-48dc-9c38-9eb43bf6570d"),
                Name = "Wish 1",
                Description = "Description 1",
                CreatorId = "creator1",
                WishlistId = Guid.Parse("761c676e-563b-48c3-94a7-7723b3aafab7"),
                IsBooked = true,
                BookedBy = "user123",
                File = new FileData(),
                Links = new List<Link>()
            },
            new Wish
            {
                Id = Guid.NewGuid(),
                Name = "Wish 2",
                Description = "Description 2",
                CreatorId = "creator2",
                WishlistId = Guid.NewGuid(),
                IsBooked = true,
                BookedBy = "user123",
                File = new FileData(),
                Links = new List<Link>()
            }
        };
        
        // Ожидаемый результат (список BookedWish)
        var bookedWishes = new List<BookedWish>
        {
            new BookedWish
            {
                WishId = wishes[0].Id,
                Name = wishes[0].Name,
                Description = wishes[0].Description,
                CreatorId = wishes[0].CreatorId,
                CreatorName = "Creator 1",
                WishlistId = wishes[0].WishlistId,
                Links = wishes[0].Links,
                File = wishes[0].File,
                Price = 0
            },
            new BookedWish
            {
                WishId = wishes[1].Id,
                Name = wishes[1].Name,
                Description = wishes[1].Description,
                CreatorId = wishes[1].CreatorId,
                CreatorName = "Creator 2",
                WishlistId = wishes[1].WishlistId,
                Links = wishes[1].Links,
                File = wishes[1].File,
                Price = 0
            }
        };

        var response = new GetBookedWishesByUserResponse
        {
            BookedWishes = bookedWishes
        };
        
        var request = new GetBookedWishesByUserRequest { UserId = userId };

        // Настроим mock репозитория, чтобы он возвращал список Wish
        _mockWishRepository.Setup(repo => repo.GetBookedWishesAsync(userId, It.IsAny<CancellationToken>()))
            .ReturnsAsync(wishes);

        // Настроим mock маппера, чтобы он преобразовывал Wish в BookedWish
        _mockMapper.Setup(mapper => mapper.Map<GetBookedWishesByUserResponse>(wishes))
            .Returns(response);

        // Act
        var result = await _handler.Handle(request, CancellationToken.None);

        // Assert
        Assert.NotNull(result);
        Assert.Equal(2, result.BookedWishes.Count);
        
        Assert.Equal(Guid.Parse("7e0b9e0e-5306-48dc-9c38-9eb43bf6570d"), result.BookedWishes[0].WishId);
        Assert.Equal("Wish 1", result.BookedWishes[0].Name);
        Assert.Equal("creator1", result.BookedWishes[0].CreatorId);
        Assert.Equal("Creator 1", result.BookedWishes[0].CreatorName);
        Assert.Equal("Description 1", result.BookedWishes[0].Description);
        Assert.Equal(Guid.Parse("761c676e-563b-48c3-94a7-7723b3aafab7"), result.BookedWishes[0].WishlistId);
        Assert.Equal(0, result.BookedWishes[0].Price);
        Assert.Empty(result.BookedWishes[0].Links);
        
        Assert.Equal("Wish 2", result.BookedWishes[1].Name);
        Assert.Equal("Creator 2", result.BookedWishes[1].CreatorName);
        Assert.Equivalent(bookedWishes, result.BookedWishes);
    }

    [Fact]
    public async Task Handle_GivenNoBookedWishes_ReturnsEmptyList()
    {
        // Arrange
        var userId = "user123";

        // Пустой список Wish
        var wishes = new List<Wish>();
        
        var response = new GetBookedWishesByUserResponse
        {
            BookedWishes = new List<BookedWish>()
        };

        var request = new GetBookedWishesByUserRequest { UserId = userId };

        _mockWishRepository.Setup(repo => repo.GetBookedWishesAsync(userId, It.IsAny<CancellationToken>()))
            .ReturnsAsync(wishes);

        _mockMapper.Setup(mapper => mapper.Map<GetBookedWishesByUserResponse>(wishes))
            .Returns(response);

        // Act
        var result = await _handler.Handle(request, CancellationToken.None);

        // Assert
        Assert.NotNull(result);
        Assert.Empty(result.BookedWishes); // Проверяем, что список пуст
    }

    [Fact]
    public async Task Handle_GivenRepositoryThrowsException_ThrowsException()
    {
        // Arrange
        var userId = "user123";
        var request = new GetBookedWishesByUserRequest { UserId = userId };

        _mockWishRepository.Setup(repo => repo.GetBookedWishesAsync(userId, It.IsAny<CancellationToken>()))
            .ThrowsAsync(new Exception("Database error"));

        // Act & Assert
        var exception = await Assert.ThrowsAsync<Exception>(() => _handler.Handle(request, CancellationToken.None));
        Assert.Equal("Database error", exception.Message);
    }
}