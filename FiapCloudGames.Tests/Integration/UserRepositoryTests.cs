using FiapCloudGames.Domain.Identity;
using FiapCloudGames.Domain.Identity.Entities;
using FiapCloudGames.Domain.Identity.Repositories;
using FiapCloudGames.Domain.Identity.ValueObjects;
using FiapCloudGames.Infrastructure.Identity;
using FiapCloudGames.Infrastructure.Persistence;
using FiapCloudGames.Infrastructure.Persistence.Repositories;
using FiapCloudGames.Tests.Integration.Fakers;
using FluentAssertions;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace FiapCloudGames.Tests.Integration
{
    public class UserRepositoryTests
    {
        const string mySecretUserToTests = "!Usuario200";
        private readonly IUserRepository _userRepository;
        private readonly IPasswordHasher _passwordHasher;
        protected readonly IServiceScope Scope;
        protected readonly IServiceProvider ServiceProvider;
        protected readonly FiapCloudGamesDbContext _context;
        public UserRepositoryTests()
        {
            var configuration = new ConfigurationBuilder()
                .AddUserSecrets<UserRepositoryTests>()
                .Build();

            var connectionString = configuration.GetConnectionString("FiapCloudGames");

            var services = new ServiceCollection();

            services.AddDbContext<FiapCloudGamesDbContext>(options =>
                options.UseNpgsql(connectionString));

            services.AddScoped<IUserRepository, UserRepository>();
            services.AddScoped<IPasswordHasher, PasswordHasher>();

            var provider = services.BuildServiceProvider();
            Scope = provider.CreateScope();

            _context = Scope.ServiceProvider.GetRequiredService<FiapCloudGamesDbContext>();
            _userRepository = Scope.ServiceProvider.GetRequiredService<IUserRepository>();
            _passwordHasher = Scope.ServiceProvider.GetRequiredService<IPasswordHasher>();
        }


        [Fact]
        public async Task User_Is_Valid_Adding_User_To_Database_ReturnsUserId()
        {
            // Arrange
            var email = EmailFaker.Generate();

            var plainPassword = Password.FromPlainText(mySecretUserToTests);
            var passwordHash = _passwordHasher.HashPassword(plainPassword.Value);

            var user = User.Create(
                "Usuario de teste de integração.",
                email,
                Password.FromHash(passwordHash)
            );

            var expectedUserId = user.Id;

            // Act
            var returnedId = await _userRepository.AddAsync(user, CancellationToken.None);

            // Assert
            returnedId.Should().Be(expectedUserId);

            var userExists = await _userRepository.ExistsById(expectedUserId, CancellationToken.None);
            userExists.Should().BeTrue();
        }
    }
}
