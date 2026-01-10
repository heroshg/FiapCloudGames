using FiapCloudGames.Domain.Identity.Entities;
using FiapCloudGames.Domain.Identity.Repositories;
using Microsoft.EntityFrameworkCore;

namespace FiapCloudGames.Infrastructure.Persistence.Repositories
{
    public class UserRepository : IUserRepository
    {
        private readonly FiapCloudGamesDbContext _context;

        public UserRepository(FiapCloudGamesDbContext context)
        {
            _context = context;
        }

        public async Task<Guid> AddAsync(User user, CancellationToken cancellationToken)
        {
            await _context.AddAsync(user, cancellationToken);
            await _context.SaveChangesAsync(cancellationToken);
            return user.Id;
        }

        public async Task<bool> ExistsById(Guid id, CancellationToken cancellationToken)
        {
            return await _context.Users.AnyAsync(u => u.Id == id, cancellationToken);
        }

        public async Task<User?> GetByIdAsync(Guid userId, CancellationToken cancellationToken)
        {
            return await _context.Users.AsNoTracking().SingleOrDefaultAsync(u => u.Id == userId, cancellationToken);
        }

        public Task<User?> GetUser(string email)
        {
            return _context.Users.AsNoTracking().SingleOrDefaultAsync(u => u.Email.Address == email);
        }

        public async Task<bool> IsEmailRegisteredAsync(string email)
        {
            return await _context.Users.AnyAsync(u => u.Email.Address == email);
        }

        public async Task<List<User>> ListAsync(string? search, bool includeInactive, CancellationToken cancellationToken)
        {
            IQueryable<User> query = _context.Users.AsNoTracking();

            if (!includeInactive)
                query = query.Where(u => u.IsActive);

            if (!string.IsNullOrWhiteSpace(search))
            {
                var s = search.Trim().ToLower();

                query = query.Where(u =>
                    u.Name.ToLower().Contains(s) ||
                    u.Email.Address.ToLower().Contains(s)
                );
            }

            // Ordenação consistente (você pode trocar por CreatedAt, UpdatedAt etc.)
            return await query
                .OrderBy(u => u.Name)
                .ToListAsync(cancellationToken);
        }

        public async Task UpdateAsync(User user, CancellationToken cancellationToken)
        {
            _context.Users.Update(user);
            await _context.SaveChangesAsync(cancellationToken);
        }

    }
}
