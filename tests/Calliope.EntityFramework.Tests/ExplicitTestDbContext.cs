using Calliope.EntityFramework.Tests.Models;
using Microsoft.EntityFrameworkCore;

namespace Calliope.EntityFramework.Tests
{
    public class ExplicitTestDbContext : DbContext
    {
        private readonly string _databaseName;

        public ExplicitTestDbContext(string databaseName)
        {
            _databaseName = databaseName;
        }
        
        public DbSet<BlogPost> Posts { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseInMemoryDatabase(_databaseName);
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<BlogPost>(e =>
            {
                e.HasKey(x => x.Id);
            });
        }
    }
}