using Microsoft.EntityFrameworkCore;
using Entities;

namespace DataAccess
{
    public class AppDbContext : DbContext
    {
        public DbSet<User> Users { get; set; }
        public DbSet<Meal> Meals { get; set; }
        public DbSet<WeightLog> WeightLogs { get; set; }
        public DbSet<Workout> Workouts { get; set; }
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseNpgsql("Host=localhost;Port=5432;Database=HealthTrackerDb;Username=postgres;Password=Domates123");
        }
    }
}
