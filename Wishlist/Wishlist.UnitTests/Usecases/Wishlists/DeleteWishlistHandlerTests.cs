using Core.Exceptions;
using Moq;
using Wishlist.Application.Usecases.Wishlists.Commands.Delete;
using Wishlist.Contracts.Repositories;

namespace Wishlist.UnitTests.Usecases.Wishlists;

public class DeleteWishlistHandlerTests
{
    private readonly Mock<IWishlistRepository> _wishlistRepositoryMock;
    private readonly DeleteWishlistHandler _deleteWishlistHandler;

    public DeleteWishlistHandlerTests()
    {
        _wishlistRepositoryMock = new Mock<IWishlistRepository>();
        _deleteWishlistHandler = new DeleteWishlistHandler(_wishlistRepositoryMock.Object);
    }

    [Fact]
    public async Task DeleteWishlist_WishlistNotFound_ThrowsNotFoundException()
    {
        // Arrange
        var request = new DeleteWishlistRequest { Id = Guid.NewGuid(), UserId = "123" };
        _wishlistRepositoryMock.Setup(repo => repo.GetByIdAsync(request.Id, It.IsAny<CancellationToken>()))
            .ReturnsAsync((Domain.Wishlist)null);

        // Act & Assert
        await Assert.ThrowsAsync<NotFoundException>(() => _deleteWishlistHandler.Handle(request, CancellationToken.None));
    }

    [Fact]
    public async Task DeleteWishlist_UnauthorizedUser_ThrowsForbiddenException()
    {
        // Arrange
        var request = new DeleteWishlistRequest { Id = Guid.NewGuid(), UserId = "123" };
        var wishlist = new Domain.Wishlist
        {
            Id = request.Id,
            CreatorId = "456",
            Uri = null,
            Name = "name"
        };

        _wishlistRepositoryMock.Setup(repo => repo.GetByIdAsync(request.Id, It.IsAny<CancellationToken>()))
            .ReturnsAsync(wishlist);

        // Act & Assert
        await Assert.ThrowsAsync<ForbiddenException>(() => _deleteWishlistHandler.Handle(request, CancellationToken.None));
    }

    [Fact]
    public async Task DeleteWishlist_SuccessfulDeletion_DeletesWishlist()
    {
        // Arrange
        var request = new DeleteWishlistRequest { Id = Guid.NewGuid(), UserId = "123" };
        var wishlist = new Domain.Wishlist
        {
            Id = request.Id,
            CreatorId = request.UserId,
            Uri = null,
            Name = "name"
        };

        _wishlistRepositoryMock.Setup(repo => repo.GetByIdAsync(request.Id, It.IsAny<CancellationToken>()))
            .ReturnsAsync(wishlist);

        // Act
        await _deleteWishlistHandler.Handle(request, CancellationToken.None);

        // Assert
        _wishlistRepositoryMock.Verify(repo => repo.DeleteAsync(wishlist, It.IsAny<CancellationToken>()), Times.Once);
    }
}