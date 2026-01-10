using FiapCloudGames.Application.Models;
using NetDevPack.SimpleMediator;

namespace FiapCloudGames.Application.Commands.NewLogin
{
    public record NewLoginCommand(string Email, string Password) : IRequest<ResultViewModel>
    {
        public string Email { get; set; }
        public string Password { get; set; }
    }
}
