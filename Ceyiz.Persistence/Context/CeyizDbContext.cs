using Microsoft.EntityFrameworkCore;
using Ceyiz.Domain.Entities;

namespace Ceyiz.Persistence.Context;

public class CeyizDbContext : DbContext
{
    public CeyizDbContext(DbContextOptions<CeyizDbContext> options) : base(options)
    {
    }

    public DbSet<User> Users { get; set; }
    public DbSet<UserProfile> UserProfiles { get; set; }
    public DbSet<UserSettings> UserSettings { get; set; }
    public DbSet<TrousseauItem> TrousseauItems { get; set; }
    public DbSet<Budget> Budgets { get; set; }
    public DbSet<Achievement> Achievements { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        // User configuration
        modelBuilder.Entity<User>(entity =>
        {
            entity.HasKey(e => e.Id);
            entity.ToTable("CeyizUsers");
            entity.Property(e => e.Email).IsRequired().HasMaxLength(255);
            entity.Property(e => e.Name).IsRequired().HasMaxLength(255);
            entity.Property(e => e.PasswordHash).IsRequired();
            entity.Property(e => e.CreatedAt).HasDefaultValueSql("GETDATE()");
            entity.HasIndex(e => e.Email).IsUnique();
        });

        // UserProfile configuration
        modelBuilder.Entity<UserProfile>(entity =>
        {
            entity.HasKey(e => e.Id);
            entity.ToTable("CeyizUserProfiles");
            entity.Property(e => e.UserId).IsRequired();
            entity.Property(e => e.CreatedAt).HasDefaultValueSql("GETDATE()");
            entity.HasOne(e => e.User)
                  .WithOne(u => u.Profile)
                  .HasForeignKey<UserProfile>(e => e.UserId)
                  .OnDelete(DeleteBehavior.Cascade);
        });

        // UserSettings configuration
        modelBuilder.Entity<UserSettings>(entity =>
        {
            entity.HasKey(e => e.Id);
            entity.ToTable("CeyizUserSettings");
            entity.Property(e => e.UserId).IsRequired();
            entity.Property(e => e.TotalBudget).HasDefaultValue(75000);
            entity.Property(e => e.WeddingDate).HasMaxLength(50);
            entity.Property(e => e.PartnerName).HasMaxLength(255);
            entity.Property(e => e.CreatedAt).HasDefaultValueSql("GETDATE()");
            entity.HasOne(e => e.User)
                  .WithMany()
                  .HasForeignKey(e => e.UserId)
                  .OnDelete(DeleteBehavior.Cascade);
        });

        // TrousseauItem configuration
        modelBuilder.Entity<TrousseauItem>(entity =>
        {
            entity.HasKey(e => e.Id);
            entity.ToTable("CeyizTrousseauItems");
            entity.Property(e => e.UserId).IsRequired();
            entity.Property(e => e.Name).IsRequired().HasMaxLength(255);
            entity.Property(e => e.Category).IsRequired().HasMaxLength(100);
            entity.Property(e => e.Price).HasDefaultValue(0);
            entity.Property(e => e.Quantity).HasDefaultValue(1);
            entity.Property(e => e.CreatedAt).HasDefaultValueSql("GETDATE()");
            entity.HasOne(e => e.User)
                  .WithMany(u => u.TrousseauItems)
                  .HasForeignKey(e => e.UserId)
                  .OnDelete(DeleteBehavior.Cascade);
        });

        // Budget configuration
        modelBuilder.Entity<Budget>(entity =>
        {
            entity.HasKey(e => e.Id);
            entity.ToTable("CeyizBudgets");
            entity.Property(e => e.UserId).IsRequired();
            entity.Property(e => e.Category).IsRequired().HasMaxLength(100);
            entity.Property(e => e.TotalAmount).HasDefaultValue(0);
            entity.Property(e => e.SpentAmount).HasDefaultValue(0);
            entity.Property(e => e.CreatedAt).HasDefaultValueSql("GETDATE()");
            entity.HasOne(e => e.User)
                  .WithMany(u => u.Budgets)
                  .HasForeignKey(e => e.UserId)
                  .OnDelete(DeleteBehavior.Cascade);
        });

        // Achievement configuration
        modelBuilder.Entity<Achievement>(entity =>
        {
            entity.HasKey(e => e.Id);
            entity.ToTable("CeyizAchievements");
            entity.Property(e => e.Title).IsRequired().HasMaxLength(255);
            entity.Property(e => e.Description).IsRequired();
            entity.Property(e => e.IconName).IsRequired().HasMaxLength(100);
            entity.Property(e => e.CreatedAt).HasDefaultValueSql("GETDATE()");
        });
    }
}
