using Core.Exceptions;
using Moq;
using Wishlist.Application.Usecases.Wishes.Commands.Delete;
using Wishlist.Contracts.Providers;
using Wishlist.Contracts.Repositories;

namespace Wishlist.UnitTests.Usecases.Wishes;

public class DeleteWishHandlerTests
{
    private readonly Mock<IWishRepository> _wishRepositoryMock;
    private readonly Mock<IFileProvider> _fileProviderMock;
    private readonly DeleteWishHandler _deleteWishHandler;

    public DeleteWishHandlerTests()
    {
        _wishRepositoryMock = new Mock<IWishRepository>();
        _fileProviderMock = new Mock<IFileProvider>();

        _deleteWishHandler = new DeleteWishHandler(
            _wishRepositoryMock.Object,
            _fileProviderMock.Object
        );
    }

    [Fact]
    public async Task Handle_WishNotFound_ThrowsNotFoundException()
    {
        // Arrange
        var request = new DeleteWishRequest
        {
            WishId = Guid.NewGuid(),
            WishlistId = Guid.NewGuid(),
            UserId = "123"
        };

        _wishRepositoryMock.Setup(repo => repo.GetByIdAsync(request.WishId, request.WishlistId, It.IsAny<CancellationToken>()))
            .ReturnsAsync((Domain.Wish)null);

        // Act & Assert
        await Assert.ThrowsAsync<NotFoundException>(() => _deleteWishHandler.Handle(request, CancellationToken.None));
    }

    [Fact]
    public async Task Handle_UnauthorizedUser_ThrowsForbiddenException()
    {
        // Arrange
        var request = new DeleteWishRequest
        {
            WishId = Guid.NewGuid(),
            WishlistId = Guid.NewGuid(),
            UserId = "123"
        };

        var wish = new Domain.Wish
        {
            Id = request.WishId,
            WishlistId = request.WishlistId,
            CreatorId = "456",
            Name = null
        };

        _wishRepositoryMock.Setup(repo => repo.GetByIdAsync(request.WishId, request.WishlistId, It.IsAny<CancellationToken>()))
            .ReturnsAsync(wish);

        // Act & Assert
        await Assert.ThrowsAsync<ForbiddenException>(() => _deleteWishHandler.Handle(request, CancellationToken.None));
    }

    [Fact]
    public async Task Handle_WishWithFile_DeletesFileAndWish()
    {
        // Arrange
        var request = new DeleteWishRequest
        {
            WishId = Guid.NewGuid(),
            WishlistId = Guid.NewGuid(),
            UserId = "123"
        };

        var wish = new Domain.Wish
        {
            Id = request.WishId,
            WishlistId = request.WishlistId,
            CreatorId = request.UserId,
            File = new Domain.FileData
            {
                FileName = "test.png"
            },
            Name = null
        };

        _wishRepositoryMock.Setup(repo => repo.GetByIdAsync(request.WishId, request.WishlistId, It.IsAny<CancellationToken>()))
            .ReturnsAsync(wish);

        // Act
        await _deleteWishHandler.Handle(request, CancellationToken.None);

        // Assert
        _fileProviderMock.Verify(fp => fp.DeleteFileAsync(request.UserId, wish.File.FileName), Times.Once);
        _wishRepositoryMock.Verify(repo => repo.DeleteAsync(wish.Id, wish.WishlistId, It.IsAny<CancellationToken>()), Times.Once);
    }

    [Fact]
    public async Task Handle_WishWithoutFile_DeletesWishWithoutFileDeletion()
    {
        // Arrange
        var request = new DeleteWishRequest
        {
            WishId = Guid.NewGuid(),
            WishlistId = Guid.NewGuid(),
            UserId = "123"
        };

        var wish = new Domain.Wish
        {
            Id = request.WishId,
            WishlistId = request.WishlistId,
            CreatorId = request.UserId,
            File = null,
            Name = null
        };

        _wishRepositoryMock.Setup(repo => repo.GetByIdAsync(request.WishId, request.WishlistId, It.IsAny<CancellationToken>()))
            .ReturnsAsync(wish);

        // Act
        await _deleteWishHandler.Handle(request, CancellationToken.None);

        // Assert
        _fileProviderMock.Verify(fp => fp.DeleteFileAsync(It.IsAny<string>(), It.IsAny<string>()), Times.Never);
        _wishRepositoryMock.Verify(repo => repo.DeleteAsync(wish.Id, wish.WishlistId, It.IsAny<CancellationToken>()), Times.Once);
    }
}