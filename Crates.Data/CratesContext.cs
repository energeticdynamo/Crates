using Microsoft.EntityFrameworkCore;
using Crates.Domain;

namespace Crates.Data
{
    public class CratesContext : DbContext
    {
        // Constructor that accepts options (connection string, etc.) from the API
        public CratesContext(DbContextOptions<CratesContext> options) : base(options)
        {
        }

        // These properties become your database tables
        public DbSet<Album> Albums { get; set; }
        public DbSet<Artist> Artists { get; set; }
        public DbSet<Genre> Genres { get; set; }
        public DbSet<Tag> Tags { get; set; }

        // Optional: You can override this method to seed data or customize table names
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
        }
    }
}
