using FiapCloudGames.Domain.GameAggregate;

namespace FiapCloudGames.Application.Models
{
    public record GameViewModel(Guid Id, string Name, string Description, decimal Price, List<PromotionViewModel> Promotions)
    {
        public static GameViewModel FromEntity(Game game) =>
            new(
                game.Id,
                game.Name,
                game.Description,
                game.Price,
                game.Promotions
                    .Select(p => new PromotionViewModel(p.Id, p.Name, p.Discount))
                    .ToList()
            );
    }
}
