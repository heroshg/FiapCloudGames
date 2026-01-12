using FiapCloudGames.Application.Models;
using NetDevPack.SimpleMediator;

namespace FiapCloudGames.Application.Queries.GetAllGamesByUserId
{
    public record GetAllGamesByUserIdQuery(Guid UserId) : IRequest<ResultViewModel<List<GameViewModel>>>
    {
    }
}
