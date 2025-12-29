using FiapCloudGames.Domain.Common;

namespace FiapCloudGames.Domain.Games
{
    public class GameOwnershipPolicy
    {
        private readonly IGameLicenseRepository _repository;

        public GameOwnershipPolicy(IGameLicenseRepository repository)
        {
            _repository = repository;
        }

        public async Task EnsureUserOwnsGameAsync(Guid userId, Guid gameId, CancellationToken cancellationToken)
        {
            var ownsGame = await _repository.ExistsAsync(gameId, userId, cancellationToken);
            if (ownsGame)
            {
                throw new DomainException("User already have game license");
            }
        }

    }
}
