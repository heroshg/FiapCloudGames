using FiapCloudGames.Application.Models;
using NetDevPack.SimpleMediator;
using System.ComponentModel.DataAnnotations;

namespace FiapCloudGames.Application.Commands.DeleteUser
{
    public record DeleteUserCommand(
    [Required(ErrorMessage = "User id is required")] 
    Guid Id
        ) : IRequest<ResultViewModel<UserAdminViewModel>>;
}
