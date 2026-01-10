using FiapCloudGames.Domain.Common;
using FiapCloudGames.Domain.Identity.ValueObjects;

namespace FiapCloudGames.Domain.Identity.Entities
{
    public class User : AggregateRoot
    {
        private const int MaxNameLength = 150;

        public static User Create(string name, Email email, Password password, bool emailAlreadyExists)
        {
            var validatedName = ValidateAndNormalizeName(name);

            EnsureEmailIsUnique(emailAlreadyExists);

            return new User
            {
                Name = validatedName,
                Email = email ?? throw new DomainException("Email is required."),
                Password = password ?? throw new DomainException("Password is required."),
                Role = Role.User
            };
            
        }

        // EF Core
        public User()
        {
            
        }

        public Email Email { get; private set; }
        public Password Password { get; private set; }
        public Role Role { get; private set; }
        public string Name { get; private set; }

        public void ChangeRole(string role)
        {
            if (string.IsNullOrWhiteSpace(role))
                throw new DomainException("Role cannot be null or empty.");

            if(string.Equals(role, Role.User.Value, StringComparison.OrdinalIgnoreCase))
                Role = Role.User;
            else if(string.Equals(role, Role.Admin.Value, StringComparison.OrdinalIgnoreCase))
                Role = Role.Admin;
            else
                throw new DomainException("Invalid role.");

            UpdatedAt = DateTime.UtcNow;
        }

        public void ChangeName(string name)
        {
            Name = ValidateAndNormalizeName(name);
            UpdatedAt = DateTime.UtcNow;
        }  
        
        public void ChangeEmail(Email newEmail, bool emailAlreadyExists)
        {
            EnsureEmailIsUnique(emailAlreadyExists);
            Email = newEmail ?? throw new DomainException("Email is required.");
            UpdatedAt = DateTime.UtcNow;
        }

        private static void EnsureEmailIsUnique(bool emailAlreadyExists)
        {
            if (emailAlreadyExists)
                throw new DomainException("Email already in use.");
        }

        private static string ValidateAndNormalizeName(string name)
        {
            if (string.IsNullOrWhiteSpace(name))
                throw new DomainException("User name cannot be null or empty.");

            var normalized = name.Trim();

            if (normalized.Length > MaxNameLength)
                throw new DomainException("Name is too long.");

            return normalized;
        }
    }
}
