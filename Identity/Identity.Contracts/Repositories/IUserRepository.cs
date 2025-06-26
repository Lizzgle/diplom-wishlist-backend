using Identity.Contracts.Models;
using Identity.Domain;

namespace Identity.Contracts.Repositories;

public interface IUserRepository
{
    Task<List<User>> GetUsersByEmailOrNameAsync(string emailOrName, CancellationToken cancellationToken = default);
    
    Task<List<User>> GetAllUsersAsync(CancellationToken cancellationToken = default);
    
    Task<User?> GetUserByIdAsync(string id, CancellationToken cancellationToken = default);
    
    Task<List<GetUsersNamesByIdsDto>> GetUsersNamesByIdsAsync(List<string> userIds, CancellationToken cancellationToken = default);
    
    Task<string?> GetUsernameByIdAsync(string id, CancellationToken cancellationToken = default);
}