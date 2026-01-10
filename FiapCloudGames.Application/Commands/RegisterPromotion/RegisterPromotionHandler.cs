using FiapCloudGames.Application.Models;
using FiapCloudGames.Domain.Games;
using NetDevPack.SimpleMediator;

namespace FiapCloudGames.Application.Commands.RegisterPromotion
{
    public class RegisterPromotionHandler : IRequestHandler<RegisterPromotionCommand, ResultViewModel<Guid>>
    {
        private readonly IGameRepository _gameRepository;
        private readonly IPromotionRepository _promotionRepository;

        public RegisterPromotionHandler(IGameRepository gameRepository, IPromotionRepository promotionRepository)
        {
            _gameRepository = gameRepository;
            _promotionRepository = promotionRepository;
        }

        public async Task<ResultViewModel<Guid>> Handle(RegisterPromotionCommand request, CancellationToken cancellationToken)
        {

            var games = await _gameRepository.GetByIdsAsync(request.GameIds, cancellationToken);

            var promotion = new Promotion(request.Name,
                request.Discount,
                request.StartsAt,
                request.EndsAt,
                games
            );


            var id = await _promotionRepository.AddAsync(promotion, cancellationToken);

            return ResultViewModel<Guid>.Success(id);
        }
    }
}
