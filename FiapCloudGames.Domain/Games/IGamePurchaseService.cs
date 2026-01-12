using FiapCloudGames.Domain.Identity.Entities;

namespace FiapCloudGames.Domain.Games
{
    public interface IGamePurchaseService
    {
        Task<GameLicense> PurchaseGameAsync(Game game,  User user, DateTime? expirationDate, CancellationToken cancellationToken);
    }
}
