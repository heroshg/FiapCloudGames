namespace FiapCloudGames.Domain.Games
{
    public interface IPromotionRepository
    {
        Task<Guid> AddAsync(Promotion promotion, CancellationToken cancellationToken);
        Task<Promotion?> GetByIdAsync(Guid id, CancellationToken cancellationToken);
    }
}
