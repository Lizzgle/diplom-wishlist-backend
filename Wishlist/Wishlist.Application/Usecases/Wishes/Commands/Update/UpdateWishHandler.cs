using AutoMapper;
using Core.Exceptions;
using MediatR;
using Wishlist.Contracts.Providers;
using Wishlist.Contracts.Repositories;

namespace Wishlist.Application.Usecases.Wishes.Commands.Update;

public class UpdateWishHandler(IWishRepository wishRepository, IFileProvider fileProvider, IMapper mapper)
    : IRequestHandler<UpdateWishRequest>
{
    public async Task Handle(UpdateWishRequest request, CancellationToken cancellationToken)
    {
        var wish = await wishRepository.GetByIdAsync(request.Id, request.WishlistId, cancellationToken);
        if (wish is null)
            throw new NotFoundException("Wish doesn't exist");

        if (wish.CreatorId != request.UserId)
            throw new ForbiddenException("You cannot update this wish");

        if (await wishRepository.IsWishInWishlistExist(request.Name, wish.WishlistId, cancellationToken) && wish.Name != request.Name)
            throw new AlreadyExistException("Wish name already exists");

        if (request.File?.FileName != wish.File?.FileName)
        {
            if (wish.File is not null)
                await fileProvider.DeleteFileAsync(request.UserId, wish.File.FileName);
            
            if (request.File is not null && request.FileStream is not null )
                await fileProvider.UploadFileAsync(request.UserId, request.File.FileName, request.FileStream);
        }
        
        mapper.Map(request, wish);
          
        await wishRepository.UpdateAsync(wish, cancellationToken);
    }
}