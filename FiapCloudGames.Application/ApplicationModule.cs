using FiapCloudGames.Domain.Games;
using FiapCloudGames.Domain.Identity;
using Microsoft.Extensions.DependencyInjection;

namespace FiapCloudGames.Application
{
    public static class ApplicationModule
    {
        public static IServiceCollection AddApplicationModule(this IServiceCollection services)
        {
            services.AddScoped<UniquenessChecker>();
            services.AddScoped<GameOwnershipPolicy>();
            return services;
        }
    }
}
