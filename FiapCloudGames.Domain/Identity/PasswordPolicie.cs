using FiapCloudGames.Domain.Common;

namespace FiapCloudGames.Domain.Identity
{
    public static class PasswordPolicy
    {
        public static void Validate(string password)
        {
            if (string.IsNullOrWhiteSpace(password))
                throw new DomainException("Password is required.");

            if (password.Length < 8)
                throw new DomainException("Password must be at least 8 characters.");

            if (!password.Any(char.IsLetter))
                throw new DomainException("Password must contain a letter.");

            if (!password.Any(char.IsDigit))
                throw new DomainException("Password must contain a digit.");

            if (!password.Any(c => !char.IsLetterOrDigit(c)))
                throw new DomainException("Password must contain a special character.");
        }
    }
}
