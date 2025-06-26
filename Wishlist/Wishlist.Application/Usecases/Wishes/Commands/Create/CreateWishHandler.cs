using AutoMapper;
using Core.Exceptions;
using MediatR;
using Wishlist.Contracts.Providers;
using Wishlist.Contracts.Repositories;
using Wishlist.Domain;

namespace Wishlist.Application.Usecases.Wishes.Commands.Create;

public class CreateWishHandler(IWishRepository wishRepository, IWishlistRepository wishlistRepository, IFileProvider fileProvider, IMapper mapper)
    : IRequestHandler<CreateWishRequest>
{
    public async Task Handle(CreateWishRequest request, CancellationToken cancellationToken)
    {
        var wishlist = await wishlistRepository.GetByIdAsync(request.WishlistId, cancellationToken);
        if (wishlist is null)
            throw new NotFoundException("Wishlist not found");

        if (await wishRepository.IsWishInWishlistExist(request.Name, request.WishlistId, cancellationToken))
            throw new AlreadyExistException($"Wish with name = { request.Name } already exists");

        if (request.File is not null && request.FileStream is not null)
            await fileProvider.UploadFileAsync(request.CreatorId, request.File.FileName, request.FileStream);

        await wishRepository.CreateAsync(mapper.Map<Wish>(request), cancellationToken);
    }
}