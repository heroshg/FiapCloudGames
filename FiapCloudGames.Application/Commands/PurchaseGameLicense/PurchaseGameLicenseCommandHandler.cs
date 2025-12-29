using FiapCloudGames.Application.Models;
using FiapCloudGames.Domain.Common;
using FiapCloudGames.Domain.Games;
using FiapCloudGames.Domain.Identity;
using NetDevPack.SimpleMediator;

namespace FiapCloudGames.Application.Commands.PurchaseGameLicense
{
    internal class PurchaseGameLicenseCommandHandler : IRequestHandler<PurchaseGameLicenseCommand, ResultViewModel<Guid>>
    {
        private readonly IUserRepository _userRepository;
        private readonly IGameRepository _gameRepository;
        private readonly IGameLicenseRepository _gameLicenseRepository;
        private readonly GameOwnershipPolicy _gameOwnershipPolicy;

        public PurchaseGameLicenseCommandHandler(IUserRepository userRepository, IGameRepository gameRepository, IGameLicenseRepository gameLicenseRepository, GameOwnershipPolicy gameOwnershipPolicy)
        {
            _userRepository = userRepository;
            _gameRepository = gameRepository;
            _gameLicenseRepository = gameLicenseRepository;
            _gameOwnershipPolicy = gameOwnershipPolicy;
        }

        public async Task<ResultViewModel<Guid>> Handle(PurchaseGameLicenseCommand request, CancellationToken cancellationToken)
        {
            var user = _userRepository.GetByIdAsync(request.UserId, cancellationToken);
            if(user is null)
            {
                return ResultViewModel<Guid>.Error("User not found.");
            }
            var game = _gameRepository.GetByIdAsync(request.GameId, cancellationToken);
            if(game is null)
            {
                return ResultViewModel<Guid>.Error("Game not found.");
            }

            await _gameOwnershipPolicy.EnsureUserOwnsGameAsync(request.UserId, request.GameId, cancellationToken);

            var gameLicense = new GameLicense(request.GameId, request.UserId, request.ExpirationDate);

            var id = await _gameLicenseRepository.PurchaseAsync(gameLicense, cancellationToken);

            return ResultViewModel<Guid>.Success(id);
        }
    }
}
