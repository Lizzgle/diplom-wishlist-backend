using AutoMapper;
using Core.Exceptions;
using Moq;
using Wishlist.Application.Usecases.Wishes.Commands.Update;
using Wishlist.Contracts.Providers;
using Wishlist.Contracts.Repositories;
using Wishlist.Domain;
using Wishlist.Domain.Enums;

namespace Wishlist.UnitTests.Usecases.Wishes;

public class UpdateWishHandlerTests
{
    private readonly Mock<IWishRepository> _wishRepositoryMock;
    private readonly Mock<IFileProvider> _fileProviderMock;
    private readonly Mock<IMapper> _mapperMock;
    private readonly UpdateWishHandler _updateWishHandler;

    public UpdateWishHandlerTests()
    {
        _wishRepositoryMock = new Mock<IWishRepository>();
        _fileProviderMock = new Mock<IFileProvider>();
        _mapperMock = new Mock<IMapper>();
        
        _updateWishHandler = new UpdateWishHandler(_wishRepositoryMock.Object, _fileProviderMock.Object, _mapperMock.Object);
    }

    [Fact]
    public async Task UpdateWish_WishNotFound_ThrowsNotFoundException()
    {
        // Arrange
        var request = new UpdateWishRequest
        {
            Id = Guid.NewGuid(),
            WishlistId = Guid.NewGuid(),
            UserId = "123",
            Name = "Updated Wish",
            Description = "Updated Wish",
            Status = WishStatus.Available,
            Rating = 5,
        };

        _wishRepositoryMock.Setup(repo => repo.GetByIdAsync(request.Id, request.WishlistId, It.IsAny<CancellationToken>()))
            .ReturnsAsync((Wish)null);

        // Act & Assert
        await Assert.ThrowsAsync<NotFoundException>(() => _updateWishHandler.Handle(request, CancellationToken.None));
    }

    [Fact]
    public async Task UpdateWish_UnauthorizedUser_ThrowsForbiddenException()
    {
        // Arrange
        var request = new UpdateWishRequest
        {
            Id = Guid.NewGuid(),
            WishlistId = Guid.NewGuid(),
            UserId = "123",
            Name = "Updated Wish"
        };

        // Act
        _wishRepositoryMock.Setup(repo => repo.GetByIdAsync(request.Id, request.WishlistId, It.IsAny<CancellationToken>()))
            .ReturnsAsync(new Wish
            {
                Name = "Old Wish",
                WishlistId = request.WishlistId,
                CreatorId = "456"
            });

        // Assert
        await Assert.ThrowsAsync<ForbiddenException>(() => _updateWishHandler.Handle(request, CancellationToken.None));
    }

    [Fact]
    public async Task UpdateWish_NameAlreadyExists_ThrowsAlreadyExistException()
    {
        // Arrange
        var request = new UpdateWishRequest
        {
            Id = Guid.NewGuid(),
            WishlistId = Guid.NewGuid(),
            UserId = "123",
            Name = "Updated Wish"
        };

        // Act
        _wishRepositoryMock.Setup(repo => repo.GetByIdAsync(request.Id, request.WishlistId, It.IsAny<CancellationToken>()))
            .ReturnsAsync(new Wish
            {
                Name = "Old Wish",
                WishlistId = request.WishlistId,
                CreatorId = "123"
            });

        _wishRepositoryMock.Setup(repo => repo.IsWishInWishlistExist(request.Name, request.WishlistId, It.IsAny<CancellationToken>()))
            .ReturnsAsync(true);

        // Assert
        await Assert.ThrowsAsync<AlreadyExistException>(() => _updateWishHandler.Handle(request, CancellationToken.None));
    }

    [Fact]
    public async Task UpdateWish_FileUpdated_DeletesOldFileAndUploadsNewFile()
    {
        // Arrange
        var request = new UpdateWishRequest
        {
            Id = Guid.NewGuid(),
            WishlistId = Guid.NewGuid(),
            UserId = "123",
            Name = "Updated Wish",
            File = new FileData { FileName = "newfile.png" },
            FileStream = new MemoryStream()
        };

        // Act
        _wishRepositoryMock.Setup(repo => repo.GetByIdAsync(request.Id, request.WishlistId, It.IsAny<CancellationToken>()))
            .ReturnsAsync(new Wish
            {
                Name = "Old Wish",
                WishlistId = request.WishlistId,
                CreatorId = "123",
                File = new FileData { FileName = "oldfile.png" }
            });

        _wishRepositoryMock.Setup(repo => repo.IsWishInWishlistExist(request.Name, request.WishlistId, It.IsAny<CancellationToken>()))
            .ReturnsAsync(false);

        // Act
        await _updateWishHandler.Handle(request, CancellationToken.None);

        // Assert
        _fileProviderMock.Verify(fp => fp.DeleteFileAsync(request.UserId, "oldfile.png"), Times.Once);
        _fileProviderMock.Verify(fp => fp.UploadFileAsync(request.UserId, "newfile.png", request.FileStream), Times.Once); 
    }

    [Fact]
    public async Task UpdateWish_SuccessfulUpdate_UpdatesWish()
    {
        // Arrange
        var request = new UpdateWishRequest
        {
            Id = Guid.NewGuid(),
            WishlistId = Guid.NewGuid(),
            UserId = "123",
            Name = "Updated Wish"
        };

        // Act
        _wishRepositoryMock.Setup(repo => repo.GetByIdAsync(request.Id, request.WishlistId, It.IsAny<CancellationToken>()))
            .ReturnsAsync(new Wish
            {
                Name = "Old Wish",
                WishlistId = request.WishlistId,
                CreatorId = "123"
            });

        _wishRepositoryMock.Setup(repo => repo.IsWishInWishlistExist(request.Name, request.WishlistId, It.IsAny<CancellationToken>()))
            .ReturnsAsync(false);

        _mapperMock.Setup(mapper => mapper.Map(request, It.IsAny<Wish>()))
            .Verifiable();

        // Act
        await _updateWishHandler.Handle(request, CancellationToken.None);

        // Assert
        _wishRepositoryMock.Verify(repo => repo.UpdateAsync(It.IsAny<Wish>(), It.IsAny<CancellationToken>()), Times.Once);
        _mapperMock.Verify();
    }
}