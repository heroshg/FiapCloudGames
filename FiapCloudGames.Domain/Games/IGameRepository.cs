namespace FiapCloudGames.Domain.Games
{
    public interface IGameRepository
    {
        Task<Guid> AddGameAsync(Game game, CancellationToken cancellationToken);
        Task<Game?> GetByIdAsync(Guid gameId, CancellationToken cancellationToken);
        Task<List<Game>> GetByIdsAsync(List<Guid> gameIds, CancellationToken cancellationToken);
        Task<List<Game>> GetAllAsync(string name = "", int page = 0, int pageSize = 10, CancellationToken cancellationToken = default!);
        Task<List<Game>> GetAllByUserIdAsync(Guid userId, CancellationToken cancellationToken);
    }
}
