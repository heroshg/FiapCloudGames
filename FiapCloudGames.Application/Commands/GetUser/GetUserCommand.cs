using FiapCloudGames.Application.Models;
using NetDevPack.SimpleMediator;

namespace FiapCloudGames.Application.Commands.GetUser
{
    public record GetUserCommand(Guid Id) : IRequest<ResultViewModel<UserViewModel>>;
}
