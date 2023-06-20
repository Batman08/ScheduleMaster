using Microsoft.EntityFrameworkCore;
using SM.Engine.Dashboard;

namespace SM.Database
{
    public class SMContext : DbContext
    {
        public SMContext(DbContextOptions options) : base(options) { }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            //modelBuilder.Entity<SmEventDataDTO>().HasNoKey();
        }

        public DbSet<SmEventDataDTO> SaveEventData { get; set; }
    }
}
