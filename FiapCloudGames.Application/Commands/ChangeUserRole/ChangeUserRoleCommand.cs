using FiapCloudGames.Application.Models;
using NetDevPack.SimpleMediator;

namespace FiapCloudGames.Application.Commands.ChangeUserRole
{
    public record ChangeUserRoleCommand(Guid UserId, string Role) : IRequest<ResultViewModel<Guid>>
    {
    }
}
