using Microsoft.EntityFrameworkCore;

namespace GetVisitantsBehaviorRobot.Models
{
    public class VisitantsBehaviorContext : DbContext
    {
        public DbSet<UserBehavior> UserBehavior { get; set; }
        protected override void OnModelCreating(ModelBuilder builder)
        {
            builder.Entity<UserBehavior>().HasKey(m => m.Id);
            base.OnModelCreating(builder);
        }
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            string constr = Utility.GetConnectionString("ConnectionStrings:SQLServerConnection");
            optionsBuilder.UseSqlServer(constr);
        }
    }
}
