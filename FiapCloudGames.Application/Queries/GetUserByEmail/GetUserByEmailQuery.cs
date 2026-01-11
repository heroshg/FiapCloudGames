using FiapCloudGames.Application.Models;
using NetDevPack.SimpleMediator;

namespace FiapCloudGames.Application.Queries.GetUserByEmail
{
    public record GetUserByEmailQuery(string Email): IRequest<ResultViewModel<UserAdminViewModel>>;
}
