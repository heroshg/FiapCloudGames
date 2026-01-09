using FiapCloudGames.Application.Models;
using NetDevPack.SimpleMediator;

namespace FiapCloudGames.Application.Queries.GetUserById
{
    public record GetUserByIdQuery(Guid Id) : IRequest<ResultViewModel<UserAdminViewModel>>;
}
