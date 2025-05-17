using AutoMapper;
using Core.Exceptions;
using Moq;
using Wishlist.Application.Usecases.Wishlists.Commands.Create;
using Wishlist.Contracts.Providers;
using Wishlist.Contracts.Repositories;

namespace Wishlist.UnitTests.Usecases.Wishlists;

public class CreateWishlistHandlerTests
{
    private readonly Mock<IWishlistRepository> _mockWishlistRepository;
    private readonly Mock<IMapper> _mockMapper;
    private readonly Mock<IUrlProvider> _mockUrlProvider;
    private readonly CreateWishlistHandler _handler;

    public CreateWishlistHandlerTests()
    {
        _mockWishlistRepository = new Mock<IWishlistRepository>();
        _mockMapper = new Mock<IMapper>();
        _mockUrlProvider = new Mock<IUrlProvider>();
        _handler = new CreateWishlistHandler(_mockWishlistRepository.Object, _mockMapper.Object, _mockUrlProvider.Object);
    }

    [Fact]
    public async Task Handle_WishlistAlreadyExists_ThrowsAlreadyExistException()
    {
        // Arrange
        var request = new CreateWishlistRequest { Name = "Existing Wishlist", UserId = "user123" };
        _mockWishlistRepository.Setup(repo => repo.IsExistsByNameAsync(request.Name, request.UserId, It.IsAny<CancellationToken>()))
            .ReturnsAsync(true);

        // Act & Assert
        await Assert.ThrowsAsync<AlreadyExistException>(() => _handler.Handle(request, CancellationToken.None));
    }

    [Fact]
    public async Task Handle_NewWishlist_CreatesWishlist()
    {
        // Arrange
        var request = new CreateWishlistRequest { Name = "New Wishlist", UserId = "user123" };
        var newWishlist = new Domain.Wishlist
        {
            Id = Guid.NewGuid(),
            Name = request.Name,
            CreatorId = request.UserId,
            Uri = null
        };
        var generatedUri = "https://wishlist.app/user123/" + newWishlist.Id;

        _mockWishlistRepository.Setup(repo => repo.IsExistsByNameAsync(request.Name, request.UserId, It.IsAny<CancellationToken>()))
            .ReturnsAsync(false);

        _mockMapper.Setup(mapper => mapper.Map<Domain.Wishlist>(request)).Returns(newWishlist);
        _mockUrlProvider.Setup(provider => provider.GenerateUrl(request.UserId, newWishlist.Id)).Returns(generatedUri);

        // Act
        await _handler.Handle(request, CancellationToken.None);

        // Assert
        _mockWishlistRepository.Verify(repo => repo.CreateAsync(newWishlist, It.IsAny<CancellationToken>()), Times.Once);
        Assert.Equal(generatedUri, newWishlist.Uri);
    }
}
