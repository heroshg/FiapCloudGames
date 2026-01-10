using FiapCloudGames.Application.Commands.RegisterUser;
using FiapCloudGames.Application.Models;
using Microsoft.Extensions.DependencyInjection;
using NetDevPack.SimpleMediator;

namespace FiapCloudGames.Application
{
    public static class ApplicationModule
    {
        public static IServiceCollection AddApplication(this IServiceCollection services)
        {
            services.AddMediator();
            return services;
        }
        private static IServiceCollection AddMediator(this IServiceCollection services)
        {
            services.AddSimpleMediator();
            services.AddScoped<IMediator, Mediator>();

            services.AddScoped<
                IRequestHandler<RegisterUserCommand, ResultViewModel<Guid>>,
                RegisterUserHandler>();
            return services;
        }
    }
}
