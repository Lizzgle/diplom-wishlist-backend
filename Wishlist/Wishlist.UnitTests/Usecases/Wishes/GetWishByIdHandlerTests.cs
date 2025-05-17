using AutoMapper;
using Core.Exceptions;
using Moq;
using Wishlist.Application.Usecases.Wishes.Queries.GetById;
using Wishlist.Contracts.Repositories;
using Wishlist.Domain;
using Wishlist.Domain.Enums;

namespace Wishlist.UnitTests.Usecases.Wishes;

public class GetWishByIdHandlerTests
{
    private readonly Mock<IWishRepository> _mockWishRepository;
        private readonly Mock<IMapper> _mockMapper;
        private readonly GetWishByIdHandler _handler;

        public GetWishByIdHandlerTests()
        {
            _mockWishRepository = new Mock<IWishRepository>();
            _mockMapper = new Mock<IMapper>();
            _handler = new GetWishByIdHandler(_mockWishRepository.Object, _mockMapper.Object);
        }

        [Fact]
        public async Task Handle_GivenValidRequest_ReturnsMappedResponse()
        {
            // Arrange
            var wishId = Guid.NewGuid();
            var wishlistId = Guid.NewGuid();
            var request = new GetWishByIdRequest { Id = wishId, WishlistId = wishlistId };
            var wish = new Wish
            {
                Id = wishId,
                Name = "Test Wish",
                Description = "Test Description",
                Status = WishStatus.Available,
                Rating = 5,
                IsBooked = false,
                Links = new List<Link>(),
                File = new FileData(),
                WishlistId = wishlistId,
                CreatorId = "user123"
            };

            var wishResponse = new GetWishByIdResponse
            {
                Id = wishId,
                Name = "Test Wish",
                Description = "Test Description",
                Status = WishStatus.Available,
                Rating = 5,
                IsBooked = false,
                BookedBy = null,
                Links = new List<Link>(),
                File = new FileData(),
                WishlistId = wishlistId,
                CreatorId = "user123"
            };

            _mockWishRepository.Setup(repo => repo.GetByIdAsync(wishId, wishlistId, It.IsAny<CancellationToken>()))
                .ReturnsAsync(wish);

            _mockMapper.Setup(mapper => mapper.Map<GetWishByIdResponse>(It.IsAny<Wish>()))
                .Returns(wishResponse);

            // Act
            var result = await _handler.Handle(request, CancellationToken.None);

            // Assert
            Assert.NotNull(result);
            Assert.Equal(wishId, result.Id);
            Assert.Equal("Test Wish", result.Name);
            Assert.Equal("Test Description", result.Description);
            Assert.Equal(WishStatus.Available, result.Status);
            Assert.Equal(5, result.Rating);
            Assert.False(result.IsBooked);
            Assert.Null(result.BookedBy);
            Assert.Empty(result.Links);
            Assert.Equal("user123", result.CreatorId);
            _mockWishRepository.Verify(repo => repo.GetByIdAsync(wishId, wishlistId, It.IsAny<CancellationToken>()), Times.Once);
            _mockMapper.Verify(mapper => mapper.Map<GetWishByIdResponse>(wish), Times.Once);
        }

        [Fact]
        public async Task Handle_GivenNonExistentWish_ThrowsNotFoundException()
        {
            // Arrange
            var wishId = Guid.NewGuid();
            var wishlistId = Guid.NewGuid();
            var request = new GetWishByIdRequest { Id = wishId, WishlistId = wishlistId };

            _mockWishRepository.Setup(repo => repo.GetByIdAsync(wishId, wishlistId, It.IsAny<CancellationToken>()))
                .ReturnsAsync((Wish?)null);

            // Act & Assert
            var exception = await Assert.ThrowsAsync<NotFoundException>(() => _handler.Handle(request, CancellationToken.None));
            Assert.Equal("Not found entity. Wish doesn't exist", exception.Message);

            _mockWishRepository.Verify(repo => repo.GetByIdAsync(wishId, wishlistId, It.IsAny<CancellationToken>()), Times.Once);
        }
}