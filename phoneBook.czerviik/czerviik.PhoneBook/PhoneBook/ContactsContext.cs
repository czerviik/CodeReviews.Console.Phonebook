using Microsoft.EntityFrameworkCore;

namespace PhoneBook;

internal class ContactsContext : DbContext
{
    public DbSet<Contact> Contacts { get; set; }
    public DbSet<PhoneNumber> PhoneNumbers { get; set; }

    private readonly ConfigReader cfg = new();
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Contact>()
            .Property(c => c.DateAdded)
            .HasDefaultValueSql("GETDATE()");

        modelBuilder.Entity<Contact>()
            .Property(c => c.DateModified)
            .HasDefaultValueSql("GETDATE()");

        modelBuilder.Entity<Contact>()
            .HasMany(c => c.PhoneNumbers)
            .WithOne(p => p.Contact)
            .OnDelete(DeleteBehavior.Cascade);

        modelBuilder.Entity<Contact>()
            .HasIndex(c => c.Email)
            .IsUnique();

        modelBuilder.Entity<PhoneNumber>()
            .Property(c => c.DateAdded)
            .HasDefaultValueSql("GETDATE()");

        modelBuilder.Entity<PhoneNumber>()
            .Property(c => c.DateModified)
            .HasDefaultValueSql("GETDATE()");
    }
    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder) => optionsBuilder.UseSqlServer(cfg.GetConnectionString());
}
