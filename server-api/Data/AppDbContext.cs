using electricity_provider_server_api.Models;
using Microsoft.EntityFrameworkCore;

namespace electricity_provider_server_api.Data
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }

        public DbSet<User> Users { get; set; }
        public DbSet<Provider> Providers { get; set; }
        public DbSet<ProviderAddress> ProviderAddress { get; set; }
        public DbSet<ProviderRating> ProviderRatings { get; set; }
        public DbSet<Tariff> Tariffs { get; set; }
        public DbSet<UserAddress> UserAddress { get; set; }
        public DbSet<UserProviderChoice> UserProviderChoices { get; set; }
        public DbSet<LogItem> LogItems { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            var decimalProps = modelBuilder.Model
            .GetEntityTypes()
            .SelectMany(t => t.GetProperties())
            .Where(p => (System.Nullable.GetUnderlyingType(p.ClrType) ?? p.ClrType) == typeof(decimal));

            foreach (var property in decimalProps)
            {
                property.SetPrecision(18);
                property.SetScale(2);
            }
        }
    }
}
