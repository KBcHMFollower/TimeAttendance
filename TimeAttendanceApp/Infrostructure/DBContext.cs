using Microsoft.EntityFrameworkCore;
using TimeAttendanceApp.Models;

namespace TimeAttendanceApp.Context
{
    public class ApplicationDbContext : DbContext
    {
        public DbSet<Project> Projects { get; set; }
        public DbSet<Models.Task> Tasks { get; set; }
        public DbSet<TaskComment> TaskComments { get; set; }
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            foreach (var entityType in modelBuilder.Model.GetEntityTypes())
            {
                var createDateProperty = entityType.FindProperty("CreateDate");
                if (createDateProperty != null && createDateProperty.ClrType == typeof(DateTime))
                {
                    createDateProperty.SetDefaultValueSql("CURRENT_TIMESTAMP");
                }

                var updateDateProperty = entityType.FindProperty("UpdateDate");
                if (updateDateProperty != null && updateDateProperty.ClrType == typeof(DateTime))
                {
                    updateDateProperty.SetDefaultValueSql("CURRENT_TIMESTAMP");
                }
            }
        }

        public override int SaveChanges()
        {
            ChangeTracker.DetectChanges();

            var entries = ChangeTracker.Entries();

            var currentTime = DateTime.Now;

            foreach (var entry in entries)
            {
                if (entry.State == EntityState.Modified)
                {
                    entry.Property("UpdateDate").CurrentValue = currentTime;
                }
            }

            return base.SaveChanges();
        }
    }
}
