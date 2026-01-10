using FiapCloudGames.Domain.Common;
using FiapCloudGames.Domain.Identity.ValueObjects;

namespace FiapCloudGames.Domain.Identity.Entities
{
    public class User : AggregateRoot
    {
        private const int maxNameLength = 150;

        // EF Core
        public User()
        {

        }

        public static User Create(string name, Email email, Password password, bool emailAlreadyExists)
        {
            if (string.IsNullOrWhiteSpace(name))
                throw new DomainException("User name cannot be null or empty.");

            if (name.Length > maxNameLength)
                throw new DomainException("Name is too long.");

            if(emailAlreadyExists) {
                throw new DomainException("Email already in use.");
            }



            return new User
            {
                Name = name,
                Email = email ?? throw new DomainException("Email is required."),
                Password = password ?? throw new DomainException("Password is required."),
                Role = new Role("User"),
                Balance = 0m
            };
            
        }

        public void Debit(decimal amount)
        {
            if (amount <= 0)
                throw new DomainException("Amount must be greater than zero.");
            if (Balance < amount)
                throw new DomainException("Insufficient balance.");
            Balance -= amount;
            UpdatedAt = DateTime.UtcNow;
        }

        public bool CanAfford(decimal amount)
            => Balance >= amount;

        public void TurnAdmin()
        {
            Role = new Role("Admin");
            UpdatedAt = DateTime.UtcNow;
        }

        public void ChangeRole(string role)
        {
            if (string.Equals(role, "user", StringComparison.OrdinalIgnoreCase))
            {
                Role = new Role("User");
            } 
            if (string.Equals(role, "admin", StringComparison.OrdinalIgnoreCase))
            {
                Role = new Role("Admin");
            }

            UpdatedAt = DateTime.UtcNow;
        }

        public Email Email { get; private set; }
        public Password Password { get; private set; }
        public Role Role { get; private set; }
        public string Name { get; private set; }
        public decimal Balance { get; private set; }
        
    }
}
