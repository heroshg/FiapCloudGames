using FiapCloudGames.Application.Models;
using NetDevPack.SimpleMediator;

namespace FiapCloudGames.Application.Commands.UpdateUser
{
    public record UpdateUserCommand(
        Guid Id,
        string? Name = null,
        string? Email = null,
        bool? IsActive = null
    ) : IRequest<ResultViewModel<UserAdminViewModel>>;
}
