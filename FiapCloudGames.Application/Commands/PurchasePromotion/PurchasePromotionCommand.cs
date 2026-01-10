using FiapCloudGames.Application.Models;
using FiapCloudGames.Domain.Games;
using NetDevPack.SimpleMediator;

namespace FiapCloudGames.Application.Commands.PurchasePromotion
{
    public record PurchasePromotionCommand(Guid PromotionId, Guid UserId) : IRequest<ResultViewModel<List<GameLicense>>>
    {

    }
}
