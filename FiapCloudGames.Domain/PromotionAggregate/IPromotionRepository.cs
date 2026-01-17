namespace FiapCloudGames.Domain.PromotionAggregate
{
    public interface IPromotionRepository
    {
        Task<Guid> AddAsync(Promotion promotion, CancellationToken cancellationToken);
        Task<Promotion?> GetByIdAsync(Guid id, CancellationToken cancellationToken);
    }
}
