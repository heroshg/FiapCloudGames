using FiapCloudGames.Domain.Games;
using Microsoft.EntityFrameworkCore;

namespace FiapCloudGames.Infrastructure.Persistence.Repositories
{
    public class GameRepository : IGameRepository
    {
        private readonly FiapCloudGamesDbContext _context;

        public GameRepository(FiapCloudGamesDbContext context)
        {
            _context = context;
        }

        public async Task<Guid> AddGameAsync(Game game, CancellationToken cancellationToken)
        {
            await _context.Games.AddAsync(game, cancellationToken);
            await _context.SaveChangesAsync(cancellationToken);
            return game.Id;
        }

        public async Task<List<Game>> GetAllAsync(string name = "", int page = 0, int pageSize = 10, CancellationToken cancellationToken = default)
        {
            return await _context.Games
                .AsNoTracking()
                .Where(g => g.IsActive && (name == "" || g.Name.Equals(name, StringComparison.CurrentCultureIgnoreCase)))
                .Where(g => !g.Promotions.Any(p => !p.IsActive))
                .Include(g => g.Promotions)
                .OrderBy(g => g.CreatedAt)
                .Skip(page * pageSize)
                .Take(pageSize)
                .ToListAsync(cancellationToken);
        }

        public async Task<Game?> GetByIdAsync(Guid gameId, CancellationToken cancellationToken)
        {
            return await _context.Games
                .AsNoTracking()
                .SingleOrDefaultAsync(g => g.Id == gameId, cancellationToken);
        }

        public Task<List<Game>> GetByIdsAsync(List<Guid> gameIds, CancellationToken cancellationToken)
        {
            return _context.Games
                .AsNoTracking()
                .Where(g => gameIds.Contains(g.Id))
                .ToListAsync(cancellationToken);
        }
    }
}
