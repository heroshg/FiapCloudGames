using FiapCloudGames.Domain.Common;

namespace FiapCloudGames.Domain.Identity.ValueObjects
{
    public class Role
    {
        public static readonly Role User = new("User");
        public static readonly Role Admin = new("Admin");

        public string Value { get; }

        private Role(string value)
        {
            Value = value;
        }

        public static Role Parse(string value)
        {
            if (string.IsNullOrWhiteSpace(value))
                throw new DomainException("Role is required.");

            var normalized = value.Trim();

            if (string.Equals(normalized, User.Value, StringComparison.OrdinalIgnoreCase))
                return User;

            if (string.Equals(normalized, Admin.Value, StringComparison.OrdinalIgnoreCase))
                return Admin;

            throw new DomainException("Invalid role.");
        }
    }
}
