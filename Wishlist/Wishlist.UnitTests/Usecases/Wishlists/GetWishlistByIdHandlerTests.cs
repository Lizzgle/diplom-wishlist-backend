using AutoMapper;
using Core.Exceptions;
using Moq;
using Wishlist.Application.Usecases.Wishlists.Queries.GetById;
using Wishlist.Contracts.Repositories;
using Wishlist.Domain;
using Wishlist.Domain.Enums;

namespace Wishlist.UnitTests.Usecases.Wishlists;

public class GetWishlistByIdHandlerTests
{
    private readonly Mock<IWishlistRepository> _wishlistRepositoryMock;
    private readonly Mock<IMapper> _mapperMock;
    private readonly GetWishlistByIdHandler _getWishlistByIdHandler;

    public GetWishlistByIdHandlerTests()
    {
        _wishlistRepositoryMock = new Mock<IWishlistRepository>();
        _mapperMock = new Mock<IMapper>();
        _getWishlistByIdHandler = new GetWishlistByIdHandler(_wishlistRepositoryMock.Object, _mapperMock.Object);
    }

    [Fact]
    public async Task Handle_WishlistNotFound_ThrowsNotFoundException()
    {
        // Arrange
        var request = new GetWishlistByIdRequest { Id = Guid.NewGuid(), UserId = "123" };
        _wishlistRepositoryMock.Setup(repo => repo.GetByIdAsync(request.Id, It.IsAny<CancellationToken>()))
            .ReturnsAsync((Domain.Wishlist)null);

        // Act & Assert
        await Assert.ThrowsAsync<NotFoundException>(() => _getWishlistByIdHandler.Handle(request, CancellationToken.None));
    }

    [Fact]
    public async Task Handle_WishlistFound_ReturnsMappedResponse()
    {
        // Arrange
        var request = new GetWishlistByIdRequest { Id = Guid.NewGuid(), UserId = "123" };
        var wishlist = new Domain.Wishlist
        {
            Id = request.Id,
            CreatorId = request.UserId,
            Name = "Test Wishlist",
            Description = "Test Description",
            Uri = null,
            Wishes = new List<Wish>()
            {
                new Wish
                {
                    Id = Guid.NewGuid(),
                    Name = "Test Wish",
                    Description = "Test Description",
                    Status = WishStatus.Available,
                    Rating = 5,
                    IsBooked = false,
                    Links = new List<Link>(),
                    File = new FileData(),
                    WishlistId = request.Id,
                    CreatorId = "user123"
                }
            }
        };

        var response = new GetWishlistByIdResponse
        {
            Id = wishlist.Id,
            CreatorId = wishlist.CreatorId,
            Name = wishlist.Name,
            Description = wishlist.Description,
            Wishes = new List<WishDto>()
            {
                new WishDto()
                {
                    Id = wishlist.Wishes[0].Id,
                    Name = wishlist.Wishes[0].Name,
                    Description = wishlist.Wishes[0].Description,
                    Status = wishlist.Wishes[0].Status,
                    Rating = wishlist.Wishes[0].Rating,
                    IsBooked = wishlist.Wishes[0].IsBooked,
                    Links = wishlist.Wishes[0].Links,
                    File = wishlist.Wishes[0].File,
                    Price = 0
                }
            }
        };

        _wishlistRepositoryMock.Setup(repo => repo.GetByIdAsync(request.Id, It.IsAny<CancellationToken>()))
            .ReturnsAsync(wishlist);

        _mapperMock.Setup(mapper => mapper.Map<GetWishlistByIdResponse>(wishlist)).Returns(response);

        // Act
        var result = await _getWishlistByIdHandler.Handle(request, CancellationToken.None);

        // Assert
        Assert.Equal(response.Id, result.Id);
        Assert.Equal(response.Name, result.Name);
        Assert.Equal(response.Description, result.Description);
        Assert.Equal(response.CreatorId, result.CreatorId);
        
        Assert.Equal(response.Wishes[0].Id,  wishlist.Wishes[0].Id);
        Assert.Equal(response.Wishes[0].Name, wishlist.Wishes[0].Name);
        Assert.Equal(response.Wishes[0].Description, wishlist.Wishes[0].Description);
        Assert.Equal(response.Wishes[0].Status, wishlist.Wishes[0].Status);
        Assert.Equal(response.Wishes[0].Rating, wishlist.Wishes[0].Rating);
        Assert.Equal(response.Wishes[0].IsBooked, wishlist.Wishes[0].IsBooked);
        Assert.Equal(response.Wishes[0].BookedBy, wishlist.Wishes[0].BookedBy);
        Assert.Equal(response.Wishes[0].Links, wishlist.Wishes[0].Links);
        Assert.Equal(response.Wishes[0].File, wishlist.Wishes[0].File);
        Assert.Equal(response.Wishes[0].Price, 0);
        
        _mapperMock.Verify(mapper => mapper.Map<GetWishlistByIdResponse>(wishlist), Times.Once);
    }
}