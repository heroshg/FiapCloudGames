using FiapCloudGames.Domain.Games;
using Microsoft.EntityFrameworkCore;

namespace FiapCloudGames.Infrastructure.Persistence.Repositories
{
    public class PromotionRepository : IPromotionRepository
    {
        private readonly FiapCloudGamesDbContext _context;

        public PromotionRepository(FiapCloudGamesDbContext context)
        {
            _context = context;
        }

        public async Task<Guid> AddAsync(Promotion promotion, CancellationToken cancellationToken)
        {
            foreach (var game in promotion.Games)
                _context.Attach(game);

            _context.Promotions.Add(promotion);
            await _context.SaveChangesAsync(cancellationToken);

            return promotion.Id;
        }

        public async Task<Promotion?> GetByIdAsync(Guid id, CancellationToken cancellationToken)
        {
            return await _context.Promotions.SingleOrDefaultAsync(p => p.Id == id, cancellationToken);
        }
    }
}
