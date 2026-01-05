namespace FiapCloudGames.Domain.Games
{
    public interface IGamePurchaseService
    {
        Task<GameLicense> PurchaseGameAsync(Guid gameId,  Guid userId, DateTime? expirationDate, CancellationToken cancellationToken);
    }
}
