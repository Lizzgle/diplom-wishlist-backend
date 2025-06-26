using Core.Api.Options;
using Microsoft.Extensions.Options;
using SecretSanta.Contracts.Providers;

namespace SecretSanta.Presentation.Providers;

public class UrlProvider(IOptions<UrlOptions> urlOptions) : IUrlProvider
{
    private readonly UrlOptions _urlOptions = urlOptions.Value;

    public string GenerateUrl(string userId, Guid wishlistId)
    {
        return $"{_urlOptions}/wishlists/{wishlistId}";
    }
}