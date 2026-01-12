using FiapCloudGames.Application.Models;
using FiapCloudGames.Domain.Games;
using FiapCloudGames.Domain.Identity.Repositories;
using NetDevPack.SimpleMediator;

namespace FiapCloudGames.Application.Commands.PurchaseGameLicense
{
    public class PurchaseGameLicenseHandler : IRequestHandler<PurchaseGameLicenseCommand, ResultViewModel<Guid>>
    {
        private readonly IUserRepository _userRepository;
        private readonly IGameRepository _gameRepository;
        private readonly IGameLicenseRepository _gameLicenseRepository;
        private readonly IGamePurchaseService _purchaseService;

        public PurchaseGameLicenseHandler(IUserRepository userRepository, IGameRepository gameRepository, IGameLicenseRepository gameLicenseRepository, IGamePurchaseService purchaseService)
        {
            _userRepository = userRepository;
            _gameRepository = gameRepository;
            _gameLicenseRepository = gameLicenseRepository;
            _purchaseService = purchaseService;
        }

        public async Task<ResultViewModel<Guid>> Handle(PurchaseGameLicenseCommand request, CancellationToken cancellationToken)
        {
            var user = await _userRepository.GetByIdAsync(request.UserId, cancellationToken);
            if(user is null)
            {
                return ResultViewModel<Guid>.Error("User not found.");
            }
            var game = await _gameRepository.GetByIdAsync(request.GameId, cancellationToken);
            if(game is null)
            {
                return ResultViewModel<Guid>.Error("Game not found.");
            }

            var gameLicense = await _purchaseService.PurchaseGameAsync(game, user, request.ExpirationDate, cancellationToken);

            var id = await _gameLicenseRepository.PurchaseAsync(gameLicense, cancellationToken);

            return ResultViewModel<Guid>.Success(id);
        }
    }
}
