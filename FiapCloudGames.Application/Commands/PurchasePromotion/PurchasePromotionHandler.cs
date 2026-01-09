using FiapCloudGames.Application.Models;
using FiapCloudGames.Domain.Games;
using FiapCloudGames.Domain.Identity.Repositories;
using NetDevPack.SimpleMediator;
using System.ComponentModel;

namespace FiapCloudGames.Application.Commands.PurchasePromotion
{
    public class PurchasePromotionHandler : IRequestHandler<PurchasePromotionCommand, ResultViewModel<List<GameLicense>>>
    {
        private readonly IPromotionRepository _promotionRepository;
        private readonly IUserRepository _userRepository;
        private readonly IGameLicenseRepository _gameLicenseRepository;

        public PurchasePromotionHandler(IPromotionRepository promotionRepository, IUserRepository userRepository, IGameLicenseRepository gameLicenseRepository)
        {
            _promotionRepository = promotionRepository;
            _userRepository = userRepository;
            _gameLicenseRepository = gameLicenseRepository;
        }

        public async Task<ResultViewModel<List<GameLicense>>> Handle(PurchasePromotionCommand request, CancellationToken cancellationToken)
        {
            var promotion = await _promotionRepository.GetByIdAsync(request.PromotionId, cancellationToken);

            if (promotion is null)
            {
                return ResultViewModel<List<GameLicense>>.Error("Promotion not found.");   
            }

            var user = await _userRepository.GetByIdAsync(request.UserId, cancellationToken);

            if (user is null)
            {
                return ResultViewModel<List<GameLicense>>.Error("User not found.");
            }

            var purchasedGamesIds = await _userRepository.GetGamesAsync(request.UserId, cancellationToken);

            var gameLicenses = promotion.Purchase(user, purchasedGamesIds);
            await _gameLicenseRepository.PurchaseGamesAsync(gameLicenses, cancellationToken);

            return ResultViewModel<List<GameLicense>>.Success(gameLicenses);
        }
    }
}
