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

    public async Task<User?> GetUserByIdAsync(string id, CancellationToken cancellationToken = default)
    {
        return await context.Users
            .AsNoTracking()
            .Where(u => u.Id == id)
            .FirstOrDefaultAsync(cancellationToken);
    }
}