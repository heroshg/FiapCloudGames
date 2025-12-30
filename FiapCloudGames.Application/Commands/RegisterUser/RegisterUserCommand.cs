using FiapCloudGames.Application.Models;
using NetDevPack.SimpleMediator;

namespace FiapCloudGames.Application.Commands.RegisterUser
{
    public record RegisterUserCommand(string Name, string Email, string Password) : IRequest<ResultViewModel<Guid>>
    {
    }
}
