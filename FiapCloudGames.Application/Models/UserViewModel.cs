using FiapCloudGames.Domain.Identity;

namespace FiapCloudGames.Application.Models
{
    public class UserViewModel
    {
        public Guid Id { get; init; }
        public string Name { get; init; } = null!;
        public string Email { get; init; } = null!;
        public Role Role { get; init; }
    }
}
