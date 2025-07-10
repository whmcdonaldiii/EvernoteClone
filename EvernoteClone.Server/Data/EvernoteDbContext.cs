using EvernoteClone.Shared.Models;
using Microsoft.EntityFrameworkCore;

namespace EvernoteClone.Server.Data;

public class EvernoteDbContext : DbContext
{
    public EvernoteDbContext(DbContextOptions<EvernoteDbContext> options) : base(options)
    {
    }

    public DbSet<Note> Notes { get; set; }
    public DbSet<Category> Categories { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        // Configure Note entity
        modelBuilder.Entity<Note>(entity =>
        {
            entity.HasKey(e => e.Id);
            entity.Property(e => e.Title).IsRequired().HasMaxLength(500);
            entity.Property(e => e.Content).IsRequired();
            entity.Property(e => e.Category).IsRequired().HasMaxLength(100);
            entity.Property(e => e.Tags)
                .HasConversion(
                    v => string.Join(',', v),
                    v => v.Split(',', StringSplitOptions.RemoveEmptyEntries).ToList()
                );
        });

        // Configure Category entity
        modelBuilder.Entity<Category>(entity =>
        {
            entity.HasKey(e => e.Id);
            entity.Property(e => e.Name).IsRequired().HasMaxLength(100);
            entity.HasIndex(e => e.Name).IsUnique();
        });

        // Seed default categories
        modelBuilder.Entity<Category>().HasData(
            new Category { Id = 1, Name = "General", IsDefault = true, CreatedAt = DateTime.UtcNow },
            new Category { Id = 2, Name = "Work", IsDefault = true, CreatedAt = DateTime.UtcNow },
            new Category { Id = 3, Name = "Personal", IsDefault = true, CreatedAt = DateTime.UtcNow },
            new Category { Id = 4, Name = "Ideas", IsDefault = true, CreatedAt = DateTime.UtcNow },
            new Category { Id = 5, Name = "Tasks", IsDefault = true, CreatedAt = DateTime.UtcNow }
        );
    }
}
