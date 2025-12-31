using Bogus;
using FiapCloudGames.Domain.Identity;

namespace FiapCloudGames.Tests.Integration.Fakers
{
    public static class EmailFaker
    {
        private static int _counter = 0;
        private static readonly Faker Faker = new Faker("pt_BR");

        public static Email Generate()
        {
            var unique = Interlocked.Increment(ref _counter);

            var address = Faker.Internet.Email(
                firstName: $"user{unique}",
                uniqueSuffix: "integration.test"
            );

            return new Email(address);
        }
    }
}
