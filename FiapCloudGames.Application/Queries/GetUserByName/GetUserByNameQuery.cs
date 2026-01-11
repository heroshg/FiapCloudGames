using FiapCloudGames.Application.Models;
using NetDevPack.SimpleMediator;

namespace FiapCloudGames.Application.Queries.GetUserByName
{
    public record GetUserByNameQuery(string Name) : IRequest<ResultViewModel<List<UserAdminViewModel>>>;
}
