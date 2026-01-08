using FiapCloudGames.Domain.Games;
using FiapCloudGames.Domain.Identity;
using FiapCloudGames.Domain.Identity.Repositories;
using FiapCloudGames.Infrastructure.Auth;
using FiapCloudGames.Infrastructure.Identity;
using FiapCloudGames.Infrastructure.Logging;
using FiapCloudGames.Infrastructure.Logs;
using FiapCloudGames.Infrastructure.Persistence;
using FiapCloudGames.Infrastructure.Persistence.Repositories;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.IdentityModel.Tokens;
using System.Text;

namespace FiapCloudGames.Infrastructure
{
    public static class InfrastructureModule
    {
        public static IServiceCollection AddInfrastructureModule(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddScoped<IPasswordHasher, PasswordHasher>();
            services.AddFiapCloudGamesContext(configuration);
            services.AddRepositoriesDependencies();
            services.AddAuth(configuration);
            services.AddDomainServices();
            services.AddCorrelationIdGenerator();
            services.AddLogger();
            return services;
        }

        private static IServiceCollection AddFiapCloudGamesContext(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddDbContext<FiapCloudGamesDbContext>(opts =>
            {
                opts.UseNpgsql(configuration.GetConnectionString("FiapCloudGames"));
            });
            return services;
        }

        private static IServiceCollection AddRepositoriesDependencies(this IServiceCollection services)
        {
            services.AddScoped<IUserRepository, UserRepository>();
            services.AddScoped<IGameRepository, GameRepository>();
            services.AddScoped<IGameLicenseRepository, GameLicenseRepository>();
            services.AddScoped<IPromotionRepository, PromotionRepository>();
            return services;
        }
        private static IServiceCollection AddDomainServices(this IServiceCollection services)
        {
            services.AddScoped<IGamePurchaseService, GamePurchaseService>();
            return services;
        }
        
        private static IServiceCollection AddAuth(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddScoped<IAuthService, AuthService>();

            services
                .AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
                .AddJwtBearer(o =>
                {
                    o.TokenValidationParameters = new TokenValidationParameters()
                    {
                        ValidateIssuer = true,
                        ValidateAudience = true,
                        ValidateLifetime = true,
                        ValidateIssuerSigningKey = true,
                        ValidIssuer = configuration["Jwt:Issuer"],
                        ValidAudience = configuration["Jwt:Audience"],
                        IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(configuration["Jwt:Key"] ?? throw new NullReferenceException("The jwt key is null.")))
                    };
                });
            return services;
        }

        private static IServiceCollection AddCorrelationIdGenerator(this IServiceCollection services)
        {
            services.AddScoped<ICorrelationIdGenerator, CorrelationIdGenerator>();
            return services;
        }

        private static IServiceCollection AddLogger(this IServiceCollection services)
        {
            services.AddScoped(typeof(BaseLogger<>));
            return services;
        }

    }
}
