using FiapCloudGames.Application.Models;
using NetDevPack.SimpleMediator;
using System.ComponentModel.DataAnnotations;

namespace FiapCloudGames.Application.Commands.RegisterGame
{
    public record RegisterGameCommand(
        [Required(ErrorMessage = "Name is required")]
        [MaxLength(80)]
        string Name,
        [Required(ErrorMessage = "Description is required")]
        [MinLength(300)]
        string Description,
        [Required(ErrorMessage = "Price is required")]
        [Range(0.01, double.MaxValue, ErrorMessage = "Price must be greater than zero")]
        decimal Price
        ) : IRequest<ResultViewModel<Guid>>
    {
    }
}
