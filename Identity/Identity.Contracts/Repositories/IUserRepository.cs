using Identity.Domain;

namespace Identity.Contracts.Repositories;

public interface IUserRepository
{
    Task<List<User>> GetUsersByEmailOrNameAsync(string emailOrName, CancellationToken cancellationToken = default);
    
    Task<User?> GetUserByIdAsync(string id, CancellationToken cancellationToken = default);
}