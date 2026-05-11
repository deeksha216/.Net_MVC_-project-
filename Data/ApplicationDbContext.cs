using HabitTracker.Models;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace HabitTracker.Data
{
    public class ApplicationDbContext : IdentityDbContext<ApplicationUser>
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options) { }

        public DbSet<Habit> Habits { get; set; }
        public DbSet<HabitLog> HabitLogs { get; set; }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);

            builder.Entity<Habit>(entity =>
            {
                entity.Property(h => h.Name).HasMaxLength(100).IsRequired();
                entity.Property(h => h.Description).HasMaxLength(300);
                entity.Property(h => h.Frequency).HasMaxLength(20).HasDefaultValue("Daily");
                entity.Property(h => h.CreatedAt).HasDefaultValueSql("GETUTCDATE()");
                entity.Property(h => h.IsActive).HasDefaultValue(true);

                entity.HasOne(h => h.User)
                      .WithMany(u => u.Habits)
                      .HasForeignKey(h => h.UserId)
                      .OnDelete(DeleteBehavior.Cascade);
            });

            builder.Entity<HabitLog>(entity =>
            {
                entity.Property(l => l.CompletedDate).HasColumnType("date");
                entity.Property(l => l.Notes).HasMaxLength(300);

                entity.HasOne(l => l.Habit)
                      .WithMany(h => h.Logs)
                      .HasForeignKey(l => l.HabitId)
                      .OnDelete(DeleteBehavior.Cascade);

               
                entity.HasIndex(l => new { l.HabitId, l.CompletedDate }).IsUnique();
            });
        }
    }
}
