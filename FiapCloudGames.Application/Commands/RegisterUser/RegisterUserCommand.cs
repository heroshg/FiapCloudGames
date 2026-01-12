using FiapCloudGames.Application.Models;
using NetDevPack.SimpleMediator;
using System.ComponentModel.DataAnnotations;

namespace FiapCloudGames.Application.Commands.RegisterUser
{
    public record RegisterUserCommand(
        [Required]
        [MaxLength(80)]
        string Name, 
        [Required]
        [EmailAddress]
        string Email, 
        [Required]
        [MinLength(8)]
        string Password) : IRequest<ResultViewModel<Guid>>
    {
    }
}
