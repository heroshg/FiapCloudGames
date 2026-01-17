using FiapCloudGames.Domain.GameLicenseAggregate;
using FiapCloudGames.Domain.UserAggregate;

namespace FiapCloudGames.Domain.GameAggregate
{
    public interface IGamePurchaseService
    {
        Task<GameLicense> PurchaseGameAsync(Game game,  User user, DateTime? expirationDate, CancellationToken cancellationToken);
    }
}
