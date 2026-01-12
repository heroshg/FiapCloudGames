using FiapCloudGames.Application.Models;
using NetDevPack.SimpleMediator;
using System.ComponentModel.DataAnnotations;

namespace FiapCloudGames.Application.Commands.UpdateUser
{
    public record UpdateUserCommand(
        [Required]
        Guid Id,
        string? Name = null,
        string? Email = null,
        bool? IsActive = null
    ) : IRequest<ResultViewModel<UserAdminViewModel>>;
}
