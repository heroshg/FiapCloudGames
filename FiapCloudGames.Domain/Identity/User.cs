using FiapCloudGames.Domain.Common;

namespace FiapCloudGames.Domain.Identity
{
    public class User : AggregateRoot
    {
        private const int maxNameLength = 150;

        public User(string name, Email email, Password password)
        {
            if (string.IsNullOrWhiteSpace(name))
                throw new DomainException("User name cannot be null or empty.");

            if (name.Length > maxNameLength)
                throw new DomainException("Name is too long.");

            Name = name;
            Email = email ?? throw new DomainException("Email is required.");
            Password = password ?? throw new DomainException("Password is required.");
            Role = Role.User;
        }

        // EF Core
        public User()
        {
            
        }

        public Email Email { get; private set; }
        public Password Password { get; private set; }
        public Role Role { get; private set; }
        public string Name { get; private set; }

        public void TurnAdmin()
        {
            Role = Role.Admin;
            UpdatedAt = DateTime.Now;
        }
    }
}
