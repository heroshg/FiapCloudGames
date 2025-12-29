using FiapCloudGames.Application.Models;
using NetDevPack.SimpleMediator;

namespace FiapCloudGames.Application.Commands.RegisterGame
{
    public record RegisterGameCommand(string Name, string Description, decimal Price) : IRequest<ResultViewModel<Guid>>
    {
    }
}
