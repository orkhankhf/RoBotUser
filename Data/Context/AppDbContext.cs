using Microsoft.EntityFrameworkCore;

namespace Data.Context
{
    public class AppDbContext : DbContext
    {
        //entity
        //public DbSet<AppSetting> AppSettings { get; set; }

        public AppDbContext(DbContextOptions<AppDbContext> options)
            : base(options)
        {
            Database.EnsureCreated();
        }

        // Constructor for design-time support, required by EF Core migrations
        public AppDbContext()
        {
            Database.EnsureCreated();
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            // Apply default collation for all string properties
            modelBuilder.UseCollation("Latin1_General_100_CI_AS_SC_UTF8");
        }
    }
}
