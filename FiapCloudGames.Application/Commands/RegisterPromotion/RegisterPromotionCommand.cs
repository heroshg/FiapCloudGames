using FiapCloudGames.Application.Models;
using FiapCloudGames.Domain.Games;
using NetDevPack.SimpleMediator;

namespace FiapCloudGames.Application.Commands.RegisterPromotion
{
    public class RegisterPromotionCommand : IRequest<ResultViewModel<Guid>>
    {
        public string Name { get; set; }
        public Discount Discount { get; set; }
        public DateTime StartsAt { get; set; }
        public DateTime EndsAt { get; set; }
        public List<Guid> GameIds { get; set; }
    }
}
