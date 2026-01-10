using FiapCloudGames.Application.Models;
using NetDevPack.SimpleMediator;

namespace FiapCloudGames.Application.Commands.DeleteUser
{
    public record DeleteUserCommand(Guid Id) : IRequest<ResultViewModel<UserAdminViewModel>>;
}
