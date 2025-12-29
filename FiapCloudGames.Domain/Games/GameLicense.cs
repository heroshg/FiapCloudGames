using FiapCloudGames.Domain.Common;
using FiapCloudGames.Domain.Identity;

namespace FiapCloudGames.Domain.Games
{
    public class GameLicense : AggregateRoot
    {
        public GameLicense(Guid gameId, Guid userId, DateTime? expirationDate)
        {
            GameId = gameId;
            UserId = userId;
            ExpirationDate = expirationDate;
        }

        public Guid GameId { get; private set; }
        public Guid UserId { get; private set; }

        public DateTime? ExpirationDate { get; private set; } = null!;
    }
}
