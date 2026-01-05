using FiapCloudGames.Domain.Games;
using FiapCloudGames.Domain.Identity.Entities;
using FiapCloudGames.Infrastructure.Persistence.Configurations;
using Microsoft.EntityFrameworkCore;

namespace FiapCloudGames.Infrastructure.Persistence
{
    public class FiapCloudGamesDbContext : DbContext
    {
        public FiapCloudGamesDbContext(DbContextOptions<FiapCloudGamesDbContext> opts) : base(opts)
        {
            
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.ApplyConfiguration(new GameConfiguration());
            modelBuilder.ApplyConfiguration(new UserConfiguration());
            modelBuilder.ApplyConfiguration(new GameLicenseConfiguration());
        }

        public DbSet<User> Users { get; set; }
        public DbSet<Game> Games { get; set; }
        public DbSet<GameLicense> GameLicenses { get; set; }

    }
}
