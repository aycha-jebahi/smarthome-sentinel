using Microsoft.EntityFrameworkCore;
using DotnetWebProject1.Models;

namespace DotnetWebProject1.Data
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }

        public DbSet<Sensor> Sensors => Set<Sensor>();
        public DbSet<Reading> Readings => Set<Reading>();
        public DbSet<Anomaly> Anomalies => Set<Anomaly>();
    }
}