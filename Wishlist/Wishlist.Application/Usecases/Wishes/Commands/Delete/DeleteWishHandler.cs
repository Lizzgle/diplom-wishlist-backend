using Core.Exceptions;
using MediatR;
using Wishlist.Contracts.Providers;
using Wishlist.Contracts.Repositories;

namespace Wishlist.Application.Usecases.Wishes.Commands.Delete;

public class DeleteWishHandler(IWishRepository wishRepository, IFileProvider fileProvider)
    : IRequestHandler<DeleteWishRequest>
{
    public async Task Handle(DeleteWishRequest request, CancellationToken cancellationToken)
    {
        var wish = await wishRepository.GetByIdAsync(request.WishId, request.WishlistId, cancellationToken);
        if (wish is null)
            throw new NotFoundException("Wish doesn't exist");

        if (wish.CreatorId != request.UserId)
            throw new ForbiddenException("You cannot delete this wish");
        
        if (wish.File is not null)
            await fileProvider.DeleteFileAsync(request.UserId, wish.File.FileName);
        
        //TODO проверить

        await wishRepository.DeleteAsync(wish.Id, wish.WishlistId, cancellationToken);
    }
}