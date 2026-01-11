using FiapCloudGames.Domain.Identity.ValueObjects;

namespace FiapCloudGames.Domain.Identity
{
    public interface IUserSpecification
    {
        Task<bool> IsSatisfiedByAsync(Email email, CancellationToken ct);
    }
}
