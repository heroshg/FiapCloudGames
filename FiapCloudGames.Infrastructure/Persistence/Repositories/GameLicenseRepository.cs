using FiapCloudGames.Domain.Games;
using Microsoft.EntityFrameworkCore;

namespace FiapCloudGames.Infrastructure.Persistence.Repositories
{
    public class GameLicenseRepository : IGameLicenseRepository
    {
        private readonly FiapCloudGamesDbContext _context;

        public GameLicenseRepository(FiapCloudGamesDbContext context)
        {
            _context = context;
        }

        public async Task<bool> ExistsAsync(Guid gameId, Guid userId, CancellationToken cancellationToken)
        {
            return await _context.GameLicenses.AsNoTracking()
                .AnyAsync(gl => gl.GameId == gameId && gl.UserId == userId, cancellationToken);
        }

        public async Task<Guid> PurchaseAsync(GameLicense gameLicense, CancellationToken cancellationToken)
        {
            await _context.GameLicenses.AddAsync(gameLicense, cancellationToken);
            await _context.SaveChangesAsync(cancellationToken);
            return gameLicense.Id;
        }
    }
}
