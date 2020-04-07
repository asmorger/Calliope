using Calliope.EntityFramework.Tests.Models;
using Microsoft.EntityFrameworkCore;

namespace Calliope.EntityFramework.Tests
{
    /* Removing feature (for now).  Will return to it soon.
    public class ConventionBasedTestDbContext : DbContext
    {
        private readonly string _databaseName;

        public ConventionBasedTestDbContext(string databaseName)
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
                e.Property(x => x.Id);
                e.Property(x => x.Title);

                e.OwnsOne(x => x.Information, b =>
                {
                    b.Property(x => x.Author);
                    b.Property(x => x.Date);
                });
            });
            
            modelBuilder.AddValueObjectConversions();
        }
    }
    
    */
}