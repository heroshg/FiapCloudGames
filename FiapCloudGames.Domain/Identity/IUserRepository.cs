namespace FiapCloudGames.Domain.Identity
{
    public interface IUserRepository
    {
        Task<bool> IsEmailRegisteredAsync(string email);
        Task<Guid> AddAsync(User user, CancellationToken cancellationToken);
        Task<User?> GetByIdAsync(Guid userId, CancellationToken cancellationToken);
        Task<bool> ExistsById(Guid id, CancellationToken cancellationToken);
    }
}
