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
        public DbSet<Models.Task> Tasks { get; set; }

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
                entity.Property(e => e.Project).IsRequired();
                entity.Property(e => e.IpAddress).IsRequired(false).HasMaxLength(45);
                entity.Property(e => e.GrubPassword).IsRequired(false).HasMaxLength(255);
                entity.Property(e => e.Password).IsRequired(false).HasMaxLength(255);
                entity.Property(e => e.LastConnected).IsRequired();
                
                // Foreign key relationship to Project
                entity.HasOne(e => e.ProjectNavigation)
                    .WithMany()
                    .HasForeignKey(e => e.Project)
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
            modelBuilder.Entity<Models.Task>(entity =>
            {
                entity.HasKey(e => e.Id);
                entity.Property(e => e.Id).ValueGeneratedOnAdd();
                entity.Property(e => e.Title).IsRequired().HasMaxLength(255);
                entity.Property(e => e.Description).HasMaxLength(1000);
                entity.Property(e => e.Status).HasMaxLength(50);
                entity.Property(e => e.CreatedAt).IsRequired();
            });
        }
    }
}
