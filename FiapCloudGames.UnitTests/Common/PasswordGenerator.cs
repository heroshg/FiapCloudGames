using System.Security.Cryptography;

namespace FiapCloudGames.UnitTests.Common
{
    /// <summary>
    /// Generates dummy passwords for unit tests without hardcoding password-like strings,
    /// reducing false positives from secret scanners (e.g., GitGuardian).
    /// </summary>
    public static class PasswordGenerator
    {
        private const string Letters = "abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ";
        private const string Digits = "0123456789";
        private const string Specials = "!@#$%^&*()-_=+[]{}<>?";

        public static string Generate(int totalLength, int lettersCount, int digitsCount, int specialCount)
        {
            if (totalLength < 0) throw new ArgumentOutOfRangeException(nameof(totalLength));
            if (lettersCount < 0) throw new ArgumentOutOfRangeException(nameof(lettersCount));
            if (digitsCount < 0) throw new ArgumentOutOfRangeException(nameof(digitsCount));
            if (specialCount < 0) throw new ArgumentOutOfRangeException(nameof(specialCount));

            if (lettersCount + digitsCount + specialCount != totalLength)
                throw new ArgumentException("The sum of character counts must match totalLength.");

            var chars = new char[totalLength];
            var idx = 0;

            Fill(chars, ref idx, Letters, lettersCount);
            Fill(chars, ref idx, Digits, digitsCount);
            Fill(chars, ref idx, Specials, specialCount);

            ShuffleInPlace(chars);

            return new string(chars);
        }

        private static void Fill(char[] buffer, ref int idx, string source, int count)
        {
            for (var i = 0; i < count; i++)
            {
                buffer[idx++] = source[RandomNumberGenerator.GetInt32(source.Length)];
            }
        }

        private static void ShuffleInPlace(char[] array)
        {
            for (int i = array.Length - 1; i > 0; i--)
            {
                int j = RandomNumberGenerator.GetInt32(i + 1);
                (array[i], array[j]) = (array[j], array[i]);
            }
        }
    }
}
