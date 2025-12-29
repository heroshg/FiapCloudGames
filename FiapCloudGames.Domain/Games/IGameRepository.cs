namespace FiapCloudGames.Domain.Games
{
    public interface IGameRepository
    {
        Task<Guid> AddGameAsync(Game game, CancellationToken cancellationToken);
        Task<Game?> GetByIdAsync(Guid gameId, CancellationToken cancellationToken);
    }
}
