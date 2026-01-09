using FiapCloudGames.Application.Models;
using NetDevPack.SimpleMediator;

namespace FiapCloudGames.Application.Commands.ChangeUserRole
{
    public record ChangeUserRoleCommand(
        Guid Id,
        string Role
    ) : IRequest<ResultViewModel<UserAdminViewModel>>;
}
