namespace SecretSanta.Contracts.Providers;

public interface IUrlProvider
{
    string GenerateUrl(string userId, Guid gameId);
}