using FiapCloudGames.Application.Models;
using NetDevPack.SimpleMediator;

namespace FiapCloudGames.Application.Commands.NewLogin
{
    public class NewLoginCommand : IRequest<ResultViewModel>
    {
        public string Email { get; set; }
        public string Password { get; set; }
    }
}
