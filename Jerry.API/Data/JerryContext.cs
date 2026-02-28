using Microsoft.EntityFrameworkCore;
using Jerry.API.Models;

namespace Jerry.API.Data
{
    public class JerryContext : DbContext
    {
        public JerryContext(DbContextOptions<JerryContext> options) : base(options)
        {
        }

        public DbSet<User> Users { get; set; }
        public DbSet<Project> Projects { get; set; }
        public DbSet<SaltTask> Tasks { get; set; }
        public DbSet<TaskUser> TaskUsers { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            // User configuration
            modelBuilder.Entity<User>(entity =>
            {
                entity.HasKey(e => e.Id);
                entity.Property(e => e.Id).HasDefaultValueSql("randomblob(16)");
                entity.Property(e => e.Name).IsRequired().HasMaxLength(255);
                entity.Property(e => e.Hostname).IsRequired().HasMaxLength(255);
                entity.HasIndex(e => e.Hostname).IsUnique();
                entity.Property(e => e.ProjectId).IsRequired();
                entity.Property(e => e.IpAddress).IsRequired(false).HasMaxLength(45);
                entity.Property(e => e.GrubPassword).IsRequired(false).HasMaxLength(255);
                entity.Property(e => e.Password).IsRequired(false).HasMaxLength(255);
                entity.Property(e => e.LastConnected).IsRequired(false);
                entity.Property(e => e.AILTag).IsRequired(false).HasMaxLength(255);

                // Foreign key relationship to Project
                entity.HasOne(e => e.Project)
                    .WithMany(p => p.Users)
                    .HasForeignKey(e => e.ProjectId)
                    .OnDelete(DeleteBehavior.Restrict);
            });

            // Project configuration
            modelBuilder.Entity<Project>(entity =>
            {
                entity.HasKey(e => e.Id);
                entity.Property(e => e.Id).ValueGeneratedOnAdd();
                entity.Property(e => e.ProjectName).IsRequired().HasMaxLength(255);
            });

            // Task configuration
            modelBuilder.Entity<SaltTask>(entity =>
            {
                entity.HasKey(e => e.Id);
                entity.Property(e => e.Id).ValueGeneratedOnAdd();
                entity.Property(e => e.Title).IsRequired().HasMaxLength(255);
                entity.Property(e => e.Description).HasMaxLength(1000);
                entity.Property(e => e.Status).HasMaxLength(50);
                entity.Property(e => e.CreatedAt).IsRequired();

                entity.HasOne(p => p.Project)
                    .WithMany(p => p.SaltTasks)
                    .HasForeignKey(e => e.ProjectId)
                    .OnDelete(DeleteBehavior.NoAction);
            });

            // TaskUsers configuration
            modelBuilder.Entity<TaskUser>(entity =>
            {
                entity.HasKey(e => e.Id);
                entity.Property(e => e.Id).ValueGeneratedOnAdd();
                entity.Property(e => e.Status).HasMaxLength(50);

                entity.HasOne(e => e.SaltTask)
                    .WithMany(e => e.TaskUsers)
                    .HasForeignKey(e => e.TaskId)
                    .OnDelete(DeleteBehavior.NoAction);

                entity.HasOne(e => e.User)
                    .WithMany(e => e.TaskUsers)
                    .HasForeignKey(e => e.UserId)
                    .OnDelete(DeleteBehavior.NoAction);
            });
        }
    }
}
