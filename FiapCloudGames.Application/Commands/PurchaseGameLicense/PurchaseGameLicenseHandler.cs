using FiapCloudGames.Application.Models;
using FiapCloudGames.Domain.Games;
using FiapCloudGames.Domain.Identity.Repositories;
using Microsoft.AspNetCore.Http;
using NetDevPack.SimpleMediator;

namespace FiapCloudGames.Application.Commands.PurchaseGameLicense
{
    public class PurchaseGameLicenseHandler : IRequestHandler<PurchaseGameLicenseCommand, ResultViewModel<Guid>>
    {
        private readonly IUserRepository _userRepository;
        private readonly IGameRepository _gameRepository;
        private readonly IGameLicenseRepository _gameLicenseRepository;
        private readonly IGamePurchaseService _purchaseService;
        private readonly IHttpContextAccessor _accessor;

        public PurchaseGameLicenseHandler(IUserRepository userRepository, IGameRepository gameRepository, IGameLicenseRepository gameLicenseRepository, IGamePurchaseService purchaseService, IHttpContextAccessor accessor)
        {
            _userRepository = userRepository;
            _gameRepository = gameRepository;
            _gameLicenseRepository = gameLicenseRepository;
            _purchaseService = purchaseService;
            _accessor = accessor;
        }

        public async Task<ResultViewModel<Guid>> Handle(PurchaseGameLicenseCommand request, CancellationToken cancellationToken)
        {

            var userIdClaim = _accessor.HttpContext?
            .User
            .FindFirst("userId")?.Value;

            if (userIdClaim is null || !Guid.TryParse(userIdClaim, out var userId))
            {
                return ResultViewModel<Guid>.Error("PurchasePromotion failed: User ID claim is missing or invalid.");
            }

            var user = await _userRepository.GetByIdAsync(userId, cancellationToken);
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
