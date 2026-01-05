
namespace FiapCloudGames.Domain.Games
{
    public class GamePurchaseService : IGamePurchaseService
    {
        private readonly IGameLicenseRepository _repository;

        public GamePurchaseService(IGameLicenseRepository repository)
        {
            _repository = repository;
        }

        public async Task<GameLicense> PurchaseGameAsync(Guid gameId, Guid userId, DateTime? expirationDate, CancellationToken cancellationToken)
        {
            if(await _repository.ExistsAsync(gameId, userId, cancellationToken))
            {
                throw new InvalidOperationException("User already owns this game.");
            }

            return new GameLicense(gameId, userId, expirationDate);
        }
    }
}
