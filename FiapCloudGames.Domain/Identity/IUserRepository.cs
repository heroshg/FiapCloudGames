namespace FiapCloudGames.Domain.Identity
{
    public interface IUserRepository
    {
        Task<bool> IsEmailRegisteredAsync(string email);
        Task<Guid> AddUserAsync(User user, CancellationToken cancellationToken);
        Task<User?> GetByIdAsync(Guid userId, CancellationToken cancellationToken);
    }
}
