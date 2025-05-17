using AutoMapper;
using Moq;
using Wishlist.Application.Usecases.Wishlists.Queries.GetByUserId;
using Wishlist.Contracts.Repositories;

namespace Wishlist.UnitTests.Usecases.Wishlists;

public class GetWishlistsByUserIdHandlerTests
{
    private readonly Mock<IWishlistRepository> _wishlistRepositoryMock;
    private readonly Mock<IMapper> _mapperMock;
    private readonly GetWishlistsByUserIdHandler _handler;

    public GetWishlistsByUserIdHandlerTests()
    {
        _wishlistRepositoryMock = new Mock<IWishlistRepository>();
        _mapperMock = new Mock<IMapper>();
        _handler = new GetWishlistsByUserIdHandler(_wishlistRepositoryMock.Object, _mapperMock.Object);
    }

    [Fact]
    public async Task Handle_UserHasNoWishlists_ReturnsEmptyList()
    {
        // Arrange
        var request = new GetWishlistsByUserIdRequest { UserId = "123" };
        _wishlistRepositoryMock.Setup(repo => repo.GetByUserIdAsync(request.UserId, It.IsAny<CancellationToken>()))
            .ReturnsAsync(new List<Domain.Wishlist>());

        _mapperMock.Setup(m => m.Map<List<WishlistDto>>(It.IsAny<List<Domain.Wishlist>>()))
            .Returns(new List<WishlistDto>());

        // Act
        var response = await _handler.Handle(request, CancellationToken.None);

        // Assert
        Assert.Empty(response.Wishlists);
        Assert.Equal(0, response.Count);
    }

    [Fact]
    public async Task Handle_UserHasWishlists_ReturnsMappedWishlists()
    {
        // Arrange
        var request = new GetWishlistsByUserIdRequest { UserId = "123" };

        var wishlists = new List<Domain.Wishlist>
        {
            new Domain.Wishlist
            {
                Id = Guid.NewGuid(),
                Name = "Wishlist 1",
                Uri = null,
                CreatorId = null
            },
            new Domain.Wishlist
            {
                Id = Guid.NewGuid(),
                Name = "Wishlist 2",
                Uri = null,
                CreatorId = null
            }
        };

        var mappedWishlists = new List<WishlistDto>
        {
            new WishlistDto { Id = wishlists[0].Id, Name = "Wishlist 1", CountOfWihes = 0 },
            new WishlistDto { Id = wishlists[1].Id, Name = "Wishlist 2", CountOfWihes = 0 }
        };

        _wishlistRepositoryMock.Setup(repo => repo.GetByUserIdAsync(request.UserId, It.IsAny<CancellationToken>()))
            .ReturnsAsync(wishlists);

        _mapperMock.Setup(m => m.Map<List<WishlistDto>>(wishlists)).Returns(mappedWishlists);

        // Act
        var response = await _handler.Handle(request, CancellationToken.None);

        // Assert
        Assert.Equal(mappedWishlists.Count, response.Count);
        Assert.Equal(mappedWishlists, response.Wishlists);
    }
}