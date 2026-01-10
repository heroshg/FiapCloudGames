using FiapCloudGames.Application.Models;
using FiapCloudGames.Domain.Games;
using NetDevPack.SimpleMediator;

namespace FiapCloudGames.Application.Queries.GetAllGames
{
    public record GetAllGamesQuery(string Name = "", int Page = 0, int PageSize = 0) : IRequest<ResultViewModel<List<GameViewModel>>>
    {

    }
}
