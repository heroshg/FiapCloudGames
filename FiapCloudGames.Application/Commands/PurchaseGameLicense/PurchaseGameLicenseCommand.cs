using FiapCloudGames.Application.Models;
using NetDevPack.SimpleMediator;
using System.ComponentModel.DataAnnotations;

namespace FiapCloudGames.Application.Commands.PurchaseGameLicense
{
    public record PurchaseGameLicenseCommand(
        [Required(ErrorMessage = "Game id is required.")]
        Guid GameId,
        [Required(ErrorMessage = "User id is required")]
        Guid UserId,
        DateTime? ExpirationDate) : IRequest<ResultViewModel<Guid>>
    {
    }
}
