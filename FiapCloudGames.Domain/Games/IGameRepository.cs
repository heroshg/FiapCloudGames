namespace FiapCloudGames.Domain.Games
{
    public interface IGameRepository
    {
        Task<Guid> AddGameAsync(Game game, CancellationToken cancellationToken);
        Task<Game?> GetByIdAsync(Guid gameId, CancellationToken cancellationToken);
        Task<List<Game>> GetByIdsAsync(List<Guid> gameIds, CancellationToken cancellationToken);
    }
}
