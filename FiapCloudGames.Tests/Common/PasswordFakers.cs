namespace FiapCloudGames.Tests.Common
{
    using Bogus;

    public static class PasswordFakers
    {
        public static readonly Faker faker = new("pt_BR");

        public static IEnumerable<object[]> NullOrWhiteSpaceStrings =>
        [
            new object[] { null },
            new object[] { "" },
            new object[] { "   " }
        ];
        
        public static IEnumerable<object[]> InvalidPasswordsForPolicy =>
        [
            new object[] { PasswordFakers.GenerateTooShortPassword(), "at least 8" },
            new object[] { PasswordFakers.GeneratePasswordWithoutLetters(), "letter" },
            new object[] { PasswordFakers.GeneratePasswordWithoutDigits(), "digit" },
            new object[] { PasswordFakers.GeneratePasswordWithoutSpecialCharacters(), "special" }
        ];

        public static string GenerateValidPassword(int length = 12)
        {
            var letters = faker.Random.String2(length - 3, "abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ");
            var digit = faker.Random.Char('0', '9');
            var special = faker.PickRandom("!@#$%^&*");

            return $"{letters}{digit}{special}";
        }

        public static string GeneratePasswordWithoutLetters()
        {
            var digits = faker.Random.String2(8, "0123456789");
            var special = faker.Random.String2(2, "!@#$%^&*");

            return digits + special;
        }

        public static string GeneratePasswordWithoutDigits()
        {
            var letters = faker.Random.String2(10, "abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ");
            var special = faker.Random.String2(2, "!@#$%^&*");

            return letters + special;
        }

        public static string GeneratePasswordWithoutSpecialCharacters()
        {
            var letters = faker.Random.String2(8, "abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ");
            var digit = faker.Random.Char('0', '9');

            return letters + digit;
        }

        public static string GenerateTooShortPassword()
        {
            return faker.Random.String2(5, "Ab1!");
        }

    }

}
