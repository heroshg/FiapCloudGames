using FiapCloudGames.Application.Models;
using NetDevPack.SimpleMediator;

namespace FiapCloudGames.Application.Commands.PurchaseGameLicense
{
    public record PurchaseGameLicenseCommand(Guid GameId, Guid UserId, DateTime? ExpirationDate) : IRequest<ResultViewModel<Guid>>
    {
    }
}
