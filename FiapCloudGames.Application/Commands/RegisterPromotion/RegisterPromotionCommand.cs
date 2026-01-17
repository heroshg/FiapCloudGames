using FiapCloudGames.Application.Models;
using FiapCloudGames.Domain.PromotionAggregate;
using NetDevPack.SimpleMediator;
using System.ComponentModel.DataAnnotations;

namespace FiapCloudGames.Application.Commands.RegisterPromotion
{
    public record RegisterPromotionCommand(
        [Required(ErrorMessage = "Promotion name is required.")]
        [MaxLength(100)]
        string Name,
        [Required(ErrorMessage = "Discount is required.")]
        Discount Discount, 
        DateTime StartsAt, 
        DateTime EndsAt,
        [Required]
        List<Guid> GameIds) : IRequest<ResultViewModel<Guid>>
    {
    }
}
