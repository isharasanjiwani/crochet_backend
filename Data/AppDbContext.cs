using System;
using CrochetWorldApi.Entities;
using Microsoft.EntityFrameworkCore;

namespace CrochetWorldApi.Data;

public class AppDbContext : DbContext
{
    public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }
    
    public DbSet<ProductEntity> Products => Set<ProductEntity>();
    public DbSet<ProductReviewEntity> ProductReviews => Set<ProductReviewEntity>();
    public DbSet<UserEntity> Users => Set<UserEntity>();
    public DbSet<OrderEntity> Orders => Set<OrderEntity>();
    public DbSet<NotificationEntity> Notifications => Set<NotificationEntity>();

    public override int SaveChanges()
    {
        AddTimestamps();
        return base.SaveChanges();
    }

    public override Task<int> SaveChangesAsync(
        CancellationToken cancellationToken = default)
    {
        AddTimestamps();
        return base.SaveChangesAsync(cancellationToken);
    }

    private void AddTimestamps()
    {
        var entries = ChangeTracker.Entries<BaseEntity>();
        var now = DateTime.UtcNow;

        foreach (var entity in entries)
        {
            if (entity.State == EntityState.Added)
            {
                entity.Entity.CreatedAt = now;
                entity.Entity.UpdatedAt = now;
            }
            else if (entity.State == EntityState.Modified)
            {
                entity.Entity.UpdatedAt = now;
                entity.Property(x => x.CreatedAt).IsModified = false;
            }
        }
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<ProductEntity>()
            .OwnsMany(p => p.Specifications, b => b.ToJson())
            .OwnsMany(p => p.Images, b => b.ToJson());

        modelBuilder.Entity<UserEntity>()
            .OwnsOne(p => p.BankDetails, b => b.ToJson())
            .Property(u => u.UserType)
            .HasConversion<string>();

        modelBuilder.Entity<OrderEntity>()
            .OwnsMany(p => p.OrderDetails, b => b.ToJson());
    }
}
