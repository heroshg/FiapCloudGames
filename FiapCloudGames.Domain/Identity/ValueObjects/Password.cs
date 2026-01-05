using FiapCloudGames.Domain.Common;
using FiapCloudGames.Domain.Identity.Policies;

namespace FiapCloudGames.Domain.Identity.ValueObjects
{
    public class Password
    {
        public string Value { get; }

        private Password(string value)
        {
            Value = value;
        }

        public static Password FromPlainText(string plainText)
        {
            PasswordPolicy.Validate(plainText);
            return new Password(plainText);
        }

        public static Password FromHash(string hash)
        {
            if (string.IsNullOrWhiteSpace(hash))
                throw new DomainException("Password hash cannot be empty.");

            return new Password(hash);
        }
    }
}
