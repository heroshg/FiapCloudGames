using FiapCloudGames.Domain.Games;

namespace FiapCloudGames.Application.Models
{
    public record PromotionViewModel(Guid Id, string Name, Discount discount)
    {
    }
}
