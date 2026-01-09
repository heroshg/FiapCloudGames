using FiapCloudGames.Domain.Identity.Entities;

namespace FiapCloudGames.Domain.Identity.Repositories
{
    public interface IUserRepository
    {
        Task<bool> IsEmailRegisteredAsync(string email);
        Task<Guid> AddAsync(User user, CancellationToken cancellationToken);
        Task<User?> GetByIdAsync(Guid userId, CancellationToken cancellationToken);
        Task<bool> ExistsById(Guid id, CancellationToken cancellationToken);
        Task<User?> GetUser(string email);
        Task<List<User>> ListAsync(string? search, bool includeInactive, CancellationToken cancellationToken);
        Task UpdateAsync(User user, CancellationToken cancellationToken);
    }
}
