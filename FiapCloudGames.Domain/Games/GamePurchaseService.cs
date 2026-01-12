
using FiapCloudGames.Domain.Common;
using FiapCloudGames.Domain.Identity.Entities;
using FiapCloudGames.Domain.Identity.Repositories;

namespace FiapCloudGames.Domain.Games
{
    public class GamePurchaseService : IGamePurchaseService
    {
        private readonly IGameLicenseRepository _repository;
        private readonly IUserRepository _userRepository;

        public GamePurchaseService(IGameLicenseRepository repository, IUserRepository userRepository)
        {
            _repository = repository;
            _userRepository = userRepository;
        }

        public async Task<GameLicense> PurchaseGameAsync(Game game, User user, DateTime? expirationDate, CancellationToken cancellationToken)
        {
            if(await _repository.ExistsAsync(game.Id, user.Id, cancellationToken))
            {
                throw new DomainException("User already owns this game.");
            }

            if(user.Balance < game.Price) 
            {
                throw new DomainException("Insufficient balance to purchase the game.");
            }

            user.Debit(game.Price);
            await _userRepository.UpdateAsync(user, cancellationToken);

            return new GameLicense(game.Id, user.Id, expirationDate);
        }
    }
}
