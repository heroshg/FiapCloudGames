using FiapCloudGames.Application.Models;
using FiapCloudGames.Domain.Games;
using NetDevPack.SimpleMediator;
using System.ComponentModel.DataAnnotations;

namespace FiapCloudGames.Application.Commands.PurchasePromotion
{
    public record PurchasePromotionCommand(
        [Required(ErrorMessage = "Promotion id is required")]
        Guid PromotionId,
        [Required(ErrorMessage = "User id is required")]
        Guid UserId) : IRequest<ResultViewModel<List<GameLicense>>>
    {

    }
}
