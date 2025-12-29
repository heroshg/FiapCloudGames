using FiapCloudGames.Domain.Games;
using FiapCloudGames.Domain.Identity;
using FiapCloudGames.Infrastructure.Identity;
using FiapCloudGames.Infrastructure.Persistence;
using FiapCloudGames.Infrastructure.Persistence.Repositories;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace FiapCloudGames.Infrastructure
{
    public static class InfrastructureModule
    {
        public static IServiceCollection AddInfrastructureModule(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddScoped<IPasswordHasher, PasswordHasher>();
            services.AddFiapCloudGamesContext(configuration);
            services.AddRepositoriesDependencies();
            return services;
        }

        private static void AddFiapCloudGamesContext(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddDbContext<FiapCloudGamesDbContext>(opts =>
            {
                opts.UseSqlServer(configuration.GetConnectionString("FiapCloudGames"));
            });
        }

        private static void AddRepositoriesDependencies(this IServiceCollection services)
        {
            services.AddScoped<IUserRepository, UserRepository>();
            services.AddScoped<IGameRepository, GameRepository>();
            services.AddScoped<IGameLicenseRepository, GameLicenseRepository>();
        }
    }
}
