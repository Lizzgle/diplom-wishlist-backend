using AutoMapper;
using Core.Exceptions;
using Moq;
using Wishlist.Application.Usecases.Wishlists.Commands.Update;
using Wishlist.Contracts.Repositories;

namespace Wishlist.UnitTests.Usecases.Wishlists;

public class UpdateWishlistHandlerTests
{
    private readonly Mock<IWishlistRepository> _wishlistRepositoryMock;
    private readonly Mock<IMapper> _mapperMock;
    private readonly UpdateWishlistHandler _updateWishlistHandler;

    public UpdateWishlistHandlerTests()
    {
        _wishlistRepositoryMock = new Mock<IWishlistRepository>();
        _mapperMock = new Mock<IMapper>();
        _updateWishlistHandler = new UpdateWishlistHandler(_wishlistRepositoryMock.Object, _mapperMock.Object);
    }

    [Fact]
    public async Task UpdateWishlist_WishlistNotFound_ThrowsNotFoundException()
    {
        // Arrange
        var request = new UpdateWishlistRequest { Id = Guid.NewGuid(), Name = "Updated Wishlist", UserId = "123" };
        _wishlistRepositoryMock.Setup(repo => repo.GetByIdAsync(request.Id, It.IsAny<CancellationToken>()))
            .ReturnsAsync((Domain.Wishlist)null);

        // Act & Assert
        await Assert.ThrowsAsync<NotFoundException>(() => _updateWishlistHandler.Handle(request, CancellationToken.None));
    }

    [Fact]
    public async Task UpdateWishlist_UnauthorizedUser_ThrowsForbiddenException()
    {
        // Arrange
        var request = new UpdateWishlistRequest { Id = Guid.NewGuid(), Name = "Updated Wishlist", UserId = "123" };
        var wishlist = new Domain.Wishlist
        {
            Id = request.Id,
            CreatorId = "456",
            Name = "Old Wishlist",
            Uri = null
        };

        _wishlistRepositoryMock.Setup(repo => repo.GetByIdAsync(request.Id, It.IsAny<CancellationToken>()))
            .ReturnsAsync(wishlist);

        // Act & Assert
        await Assert.ThrowsAsync<ForbiddenException>(() => _updateWishlistHandler.Handle(request, CancellationToken.None));
    }

    [Fact]
    public async Task UpdateWishlist_SuccessfulUpdate_UpdatesWishlist()
    {
        // Arrange
        var request = new UpdateWishlistRequest { Id = Guid.NewGuid(), Name = "Updated Wishlist", Description = "Updated Description", UserId = "123" };
        var wishlist = new Domain.Wishlist
        {
            Id = request.Id,
            CreatorId = request.UserId,
            Name = "Old Wishlist",
            Uri = null
        };

        _wishlistRepositoryMock.Setup(repo => repo.GetByIdAsync(request.Id, It.IsAny<CancellationToken>()))
            .ReturnsAsync(wishlist);

        // Act
        await _updateWishlistHandler.Handle(request, CancellationToken.None);

        // Assert
        _mapperMock.Verify(mapper => mapper.Map(request, wishlist), Times.Once);
        _wishlistRepositoryMock.Verify(repo => repo.UpdateAsync(wishlist, It.IsAny<CancellationToken>()), Times.Once);
    }
}