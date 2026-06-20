using _4task4.Models;
using Microsoft.EntityFrameworkCore;

namespace _4task4.DataBase;

public class UserDBContext : DbContext
{
    public UserDBContext(DbContextOptions<UserDBContext> option) : base(option) { }

    public DbSet<UserDataModel> Users { get; set; } = null!;

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);
        modelBuilder.Entity<UserDataModel>(entity =>
        {
            entity.HasKey(p => p.Id);
            entity.Property(p => p.Name).IsRequired().HasMaxLength(100);
            entity.Property(p => p.Email).IsRequired().HasMaxLength(100);
            entity.Property(p => p.Password).IsRequired().HasMaxLength(256);
            entity.Property(p => p.EmailConfirmedStatus).HasColumnName("IsEmailConfirmed");
            entity.Property(p => p.RegisterTime).HasColumnName("RegisteredAt");
            entity.Property(p => p.LastRegisterTime).HasColumnName("LastLoginAt");
            entity.HasIndex(p => p.Email).IsUnique();
        });
    }
}
