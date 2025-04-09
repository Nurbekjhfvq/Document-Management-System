namespace Document.Data;
using Document.Entity;
using Microsoft.EntityFrameworkCore;

public class AppDbContext : DbContext
{
    public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }

    // Entity-lar uchun DbSet lar
    public DbSet<User> Users { get; set; } = null!;
    public DbSet<Document> Documents { get; set; } = null!;
    public DbSet<Category> Categories { get; set; } = null!;
    public DbSet<Version> Versions { get; set; } = null!;
    public DbSet<Archive> Archives { get; set; } = null!;
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);
     
    // User konfiguratsiyasi
    modelBuilder.Entity<User>()
        .HasIndex(u => u.Email)
        .IsUnique();

    // Document konfiguratsiyasi
    modelBuilder.Entity<Document>()
        .HasOne(d => d.User)
        .WithMany(u => u.Documents)
        .HasForeignKey(d => d.UserId)
        .OnDelete(DeleteBehavior.Cascade);

    modelBuilder.Entity<Document>()
        .HasOne(d => d.Category)
        .WithMany(c => c.Documents)
        .HasForeignKey(d => d.CategoryId)
        .OnDelete(DeleteBehavior.Restrict);

    // DocumentVersion konfiguratsiyasi
    modelBuilder.Entity<Version>()
        .HasOne(v => v.Document)
        .WithMany(d => d.Versions)
        .HasForeignKey(v => v.DocumentId)
        .OnDelete(DeleteBehavior.Cascade);

    // Archive konfiguratsiyasi
    modelBuilder.Entity<Archive>()
        .HasOne(a => a.Document)
        .WithMany(d => d.Archives) // Bog‘liqlik to‘g‘ri ishlashi uchun qo‘shildi
        .HasForeignKey(a => a.DocumentId)
        .OnDelete(DeleteBehavior.Cascade);
    }
}

