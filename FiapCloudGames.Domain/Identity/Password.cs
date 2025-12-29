using FiapCloudGames.Domain.Common;

namespace FiapCloudGames.Domain.Identity
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
