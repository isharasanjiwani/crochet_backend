using System;
using CrochetWorldApi.Models;
using Microsoft.EntityFrameworkCore;

namespace CrochetWorldApi.Data;

public class AppDbContext : DbContext
{
    public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }
    
    public DbSet<ProductModel> Products => Set<ProductModel>();
    public DbSet<ProductReviewModel> ProductReviews => Set<ProductReviewModel>();
    public DbSet<UserModel> Users => Set<UserModel>();
    public DbSet<OrderModel> Orders => Set<OrderModel>();
    public DbSet<NotificationModel> Notifications => Set<NotificationModel>();

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
        var entries = ChangeTracker.Entries<BaseModel>();
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
        modelBuilder.Entity<ProductModel>()
            .OwnsMany(p => p.Specifications, b => b.ToJson());

        modelBuilder.Entity<UserModel>()
            .OwnsOne(p => p.BankDetails, b => b.ToJson());

        modelBuilder.Entity<OrderModel>()
            .OwnsMany(p => p.OrderDetails, b => b.ToJson());
    }
}
