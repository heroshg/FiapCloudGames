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

        public async Task<Game?> GetByIdAsync(Guid gameId, CancellationToken cancellationToken)
        {
            return await _context.Games.AsNoTracking().SingleOrDefaultAsync(g => g.Id == gameId, cancellationToken);
        }
    }
}
