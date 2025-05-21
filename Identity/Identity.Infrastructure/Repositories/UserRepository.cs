using Identity.Contracts.Models;
using Identity.Contracts.Repositories;
using Identity.Domain;
using Microsoft.EntityFrameworkCore;

namespace Identity.Infrastructure.Repositories;

public class UserRepository(AppDbContext context) : IUserRepository
{
    public async Task<List<User>> GetUsersByEmailOrNameAsync(string query, CancellationToken cancellationToken)
    {
        return await context.Users
                .AsNoTracking()
                .Where(u => u.Email!.Contains(query) || u.UserName!.Contains(query))
                .ToListAsync(cancellationToken);
    }

    public async Task<List<User>> GetAllUsersAsync(CancellationToken cancellationToken = default)
    {
        return await context.Users
            .AsNoTracking()
            .ToListAsync(cancellationToken);
    }

    public async Task<User?> GetUserByIdAsync(string id, CancellationToken cancellationToken = default)
    {
        return await context.Users
            .AsNoTracking()
            .Where(u => u.Id == id)
            .FirstOrDefaultAsync(cancellationToken);
    }

    public async Task<List<GetUsersNamesByIdsDto>> GetUsersNamesByIdsAsync(List<string> userIds, CancellationToken cancellationToken = default)
    {
        return await context.Users
            .AsNoTracking()
            .Where(u => userIds.Contains(u.Id))
            .Select(u => new GetUsersNamesByIdsDto() { Id = u.Id, UserName = u.UserName! })
            .ToListAsync(cancellationToken);
    }

    public async Task<string?> GetUsernameByIdAsync(string id, CancellationToken cancellationToken = default)
    {
        return await context.Users
            .AsNoTracking()
            .Where(u => u.Id == id)
            .Select(u => u.UserName!)
            .FirstOrDefaultAsync(cancellationToken);
    }
}