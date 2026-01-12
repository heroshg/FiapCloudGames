using FiapCloudGames.Application.Models;
using NetDevPack.SimpleMediator;
using System.ComponentModel.DataAnnotations;

namespace FiapCloudGames.Application.Commands.ChangeUserRole
{
    public record ChangeUserRoleCommand(
        [Required(ErrorMessage = "User id is required")] 
        Guid Id,
        [Required(ErrorMessage = "Role is required")]
        string Role
    ) : IRequest<ResultViewModel<UserAdminViewModel>>;
}
