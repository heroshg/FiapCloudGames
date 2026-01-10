using FiapCloudGames.Application.Models;
using NetDevPack.SimpleMediator;

namespace FiapCloudGames.Application.Commands.NewLogin
{
    public record NewLoginCommand(string Email, string Password) : IRequest<ResultViewModel>
    {
    }
}
