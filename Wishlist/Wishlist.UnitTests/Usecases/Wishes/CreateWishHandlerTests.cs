using AutoMapper;
using Core.Exceptions;
using Moq;
using Wishlist.Application.Usecases.Wishes.Commands.Create;
using Wishlist.Contracts.Providers;
using Wishlist.Contracts.Repositories;
using Wishlist.Domain;
using Wishlist.Domain.Enums;

namespace Wishlist.UnitTests.Usecases.Wishes;

public class CreateWishHandlerTests
{
    private readonly Mock<IWishRepository> _wishRepositoryMock;
    private readonly Mock<IWishlistRepository> _wishlistRepositoryMock;
    private readonly Mock<IFileProvider> _fileProviderMock;
    private readonly Mock<IMapper> _mapperMock;
    private readonly CreateWishHandler _handler;

    public CreateWishHandlerTests()
    {
        _wishRepositoryMock = new Mock<IWishRepository>();
        _wishlistRepositoryMock = new Mock<IWishlistRepository>();
        _fileProviderMock = new Mock<IFileProvider>();
        _mapperMock = new Mock<IMapper>();

        _handler = new CreateWishHandler(
            _wishRepositoryMock.Object,
            _wishlistRepositoryMock.Object,
            _fileProviderMock.Object,
            _mapperMock.Object
        );
    }

    [Fact]
    public async Task Handle_WishlistNotFound_ThrowsNotFoundException()
    {
        // Arrange
        var request = new CreateWishRequest
        {
            Name = "Wish 1",
            Description = "Description 1",
            CreatorId = "creator1",
            WishlistId = Guid.Parse("761c676e-563b-48c3-94a7-7723b3aafab7"),
            File = new FileData(),
            Links = new List<Link>()
        };

        _wishlistRepositoryMock.Setup(repo => repo.GetByIdAsync(request.WishlistId, It.IsAny<CancellationToken>()))
            .ReturnsAsync((Domain.Wishlist)null);

        // Act & Assert
        await Assert.ThrowsAsync<NotFoundException>(() => _handler.Handle(request, CancellationToken.None));
    }

    [Fact]
    public async Task Handle_WishAlreadyExists_ThrowsAlreadyExistException()
    {
        // Arrange
        var request = new CreateWishRequest
        {
            Name = "Existing Wish",
            WishlistId = Guid.NewGuid(),
            CreatorId = "123"
        };

        var wishlist = new Domain.Wishlist { Uri = "uri", Name = "name", CreatorId = "123" };
        _wishlistRepositoryMock.Setup(repo => repo.GetByIdAsync(request.WishlistId, It.IsAny<CancellationToken>()))
            .ReturnsAsync(wishlist);

        _wishRepositoryMock.Setup(repo => repo.IsWishInWishlistExist(request.Name, request.WishlistId, It.IsAny<CancellationToken>()))
            .ReturnsAsync(true);

        // Act & Assert
        await Assert.ThrowsAsync<AlreadyExistException>(() => _handler.Handle(request, CancellationToken.None));
    }

    [Fact]
    public async Task Handle_SuccessfulCreationWithoutFile_CreatesWish()
    {
        // Arrange
        var request = new CreateWishRequest
        {
            Name = "New Wish",
            WishlistId = Guid.NewGuid(),
            CreatorId = "123",
            Status = WishStatus.Available,
            Rating = 4
        };

        var wishlist = new Domain.Wishlist { Uri = "uri", Name = "name", CreatorId = "123" };
        var wish = new Domain.Wish
        {
            Id = Guid.NewGuid(),
            Name = request.Name,
            WishlistId = request.WishlistId
        };

        _wishlistRepositoryMock.Setup(repo => repo.GetByIdAsync(request.WishlistId, It.IsAny<CancellationToken>()))
            .ReturnsAsync(wishlist);

        _wishRepositoryMock.Setup(repo => repo.IsWishInWishlistExist(request.Name, request.WishlistId, It.IsAny<CancellationToken>()))
            .ReturnsAsync(false);

        _mapperMock.Setup(m => m.Map<Domain.Wish>(request)).Returns(wish);

        // Act
        await _handler.Handle(request, CancellationToken.None);

        // Assert
        _wishRepositoryMock.Verify(repo => repo.CreateAsync(It.Is<Domain.Wish>(w => w.Name == "New Wish"), It.IsAny<CancellationToken>()), Times.Once);
        _fileProviderMock.Verify(fp => fp.UploadFileAsync(It.IsAny<string>(), It.IsAny<string>(), It.IsAny<Stream>()), Times.Never);
    }
}
