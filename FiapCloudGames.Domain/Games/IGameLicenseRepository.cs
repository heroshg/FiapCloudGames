namespace FiapCloudGames.Domain.Games
{
    public interface IGameLicenseRepository
    {
        Task<Guid> PurchaseAsync(GameLicense gameLicense, CancellationToken cancellationToken);
        Task<bool> ExistsAsync(Guid gameId, Guid userId, CancellationToken cancellationToken);
    }
}
