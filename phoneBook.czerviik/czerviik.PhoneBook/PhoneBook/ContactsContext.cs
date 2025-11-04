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
            .Property(c => c.Category)
            .HasDefaultValue("-");


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
    public override int SaveChanges()
{
        foreach (var entry in ChangeTracker.Entries<Contact>())
        {
            if (entry.State == EntityState.Added)
                entry.Entity.DateAdded = DateTime.Now;
            if (entry.State == EntityState.Modified||entry.State == EntityState.Added)
                entry.Entity.DateModified = DateTime.Now;
        }
        foreach (var entry in ChangeTracker.Entries<PhoneNumber>())
        {
            if (entry.State == EntityState.Added)
                entry.Entity.DateAdded = DateTime.Now;
            if (entry.State == EntityState.Modified || entry.State == EntityState.Added)
                entry.Entity.DateModified = DateTime.Now;
            if (entry.State == EntityState.Modified)
            {
                var contact = Contacts.FirstOrDefault(c => c.Id == entry.Entity.ContactId);
                if (contact != null)
                    contact.DateModified = DateTime.Now;
            }
        }
    return base.SaveChanges();
}

}
