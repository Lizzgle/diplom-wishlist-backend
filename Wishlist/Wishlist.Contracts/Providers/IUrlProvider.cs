namespace Wishlist.Contracts.Providers;

public interface IUrlProvider
{
    string GenerateUrl(string userId, Guid wishlistId);
}