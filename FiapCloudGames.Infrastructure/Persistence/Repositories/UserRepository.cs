using FiapCloudGames.Domain.Games;
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

        public async Task<IEnumerable<Guid>> GetGamesAsync(Guid userId, CancellationToken cancellationToken)
        {
            return await _context.GameLicenses
                .AsNoTracking()
                .Where(gl => gl.UserId == userId)
                .Select(gl => gl.GameId)
                .ToListAsync(cancellationToken);
        }

        public Task<User?> GetUser(string email)
        {
            return _context.Users.AsNoTracking().SingleOrDefaultAsync(u => u.Email.Address == email);
        }

        public async Task<bool> IsEmailRegisteredAsync(string email)
        {
            return await _context.Users.AnyAsync(u => u.Email.Address == email);
        }
    }
}
