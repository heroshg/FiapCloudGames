namespace FiapCloudGames.Domain.GameLicenseAggregate
{
    public interface IGameLicenseRepository
    {
        Task<Guid> PurchaseAsync(GameLicense gameLicense, CancellationToken cancellationToken);
        Task<bool> ExistsAsync(Guid gameId, Guid userId, CancellationToken cancellationToken);
        Task PurchaseGamesAsync(IEnumerable<GameLicense> gameLicenses, CancellationToken cancellationToken); 
    }
}
