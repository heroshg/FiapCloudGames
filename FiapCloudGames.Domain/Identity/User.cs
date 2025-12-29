using FiapCloudGames.Domain.Common;

namespace FiapCloudGames.Domain.Identity
{
    public class User : AggregateRoot
    {
        public User(Email email, Password password)
        {
            Email = email;
            Password = password;
            Role = Role.User;
            Username = email.Address;
        }

        // EF Core
        public User()
        {
            
        }

        public Email Email { get; private set; }
        public Password Password { get; private set; }
        public Role Role { get; private set; }
        public string Username { get; private set; }

        public void TurnAdmin()
        {
            Role = Role.Admin;
            UpdatedAt = DateTime.Now;
        }
    }
}
