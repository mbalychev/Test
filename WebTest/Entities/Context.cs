using Microsoft.EntityFrameworkCore;

namespace WebTest.Entities
{
    public class Context : DbContext
    {
        public Context()
        {
            Database.EnsureCreated();
        }

        public DbSet<Organization> Organizations { get; set; }
        public DbSet<OrgRating> OrgRatings { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseSqlServer("Server=(localdb)\\mssqllocaldb;Database=TestDb;Trusted_Connection=true;");
        }

    }
}
