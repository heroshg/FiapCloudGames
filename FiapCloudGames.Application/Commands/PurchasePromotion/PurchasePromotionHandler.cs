using FiapCloudGames.Application.Models;
using FiapCloudGames.Domain.GameLicenseAggregate;
using FiapCloudGames.Domain.Identity.Entities;
using FiapCloudGames.Domain.PromotionAggregate;
using FiapCloudGames.Domain.UserAggregate;
using Microsoft.AspNetCore.Http;
using NetDevPack.SimpleMediator;
using System.ComponentModel;

namespace FiapCloudGames.Application.Commands.PurchasePromotion
{
    public class PurchasePromotionHandler : IRequestHandler<PurchasePromotionCommand, ResultViewModel<List<GameLicense>>>
    {
        private readonly IPromotionRepository _promotionRepository;
        private readonly IUserRepository _userRepository;
        private readonly IGameLicenseRepository _gameLicenseRepository;
        private readonly IHttpContextAccessor _accessor;

        public PurchasePromotionHandler(
            IPromotionRepository promotionRepository, 
            IUserRepository userRepository, 
            IGameLicenseRepository gameLicenseRepository, 
            IHttpContextAccessor accessor)
        {
            _promotionRepository = promotionRepository;
            _userRepository = userRepository;
            _gameLicenseRepository = gameLicenseRepository;
            _accessor = accessor;
        }

        public async Task<ResultViewModel<List<GameLicense>>> Handle(PurchasePromotionCommand request, CancellationToken cancellationToken)
        {
            var promotion = await _promotionRepository.GetByIdAsync(request.PromotionId, cancellationToken);

            if (promotion is null)
            {
                return ResultViewModel<List<GameLicense>>.Error("Promotion not found.");   
            }

            var userIdClaim = _accessor.HttpContext?
            .User
            .FindFirst("userId")?.Value;

            if (userIdClaim is null || !Guid.TryParse(userIdClaim, out var userId))
            {
                return ResultViewModel<List<GameLicense>>.Error("PurchasePromotion failed: User ID claim is missing or invalid.");
            }

            var user = await _userRepository.GetByIdAsync(userId, cancellationToken);

            if (user is null)
            {
                return ResultViewModel<List<GameLicense>>.Error("User not found.");
            }

            var purchasedGamesIds = await _userRepository.GetGamesAsync(userId, cancellationToken);

            var gameLicenses = promotion.Purchase(user, purchasedGamesIds);
            await _userRepository.UpdateAsync(user, cancellationToken);
            await _gameLicenseRepository.PurchaseGamesAsync(gameLicenses, cancellationToken);

            return ResultViewModel<List<GameLicense>>.Success(gameLicenses);
        }
    }
}
