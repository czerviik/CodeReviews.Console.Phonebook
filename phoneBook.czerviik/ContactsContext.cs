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
            .Property(c => c.DateAdded);

        modelBuilder.Entity<Contact>()
            .Property(c => c.DateModified);

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

        SeedData(modelBuilder);
    }
    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        var logFile = Program.LogPath;
        optionsBuilder
        .UseSqlServer(cfg.GetConnectionString());

        if (Program.EfSqlLoggingEnabled)
        {
            optionsBuilder
            .EnableSensitiveDataLogging()
            .LogTo(msg =>
                File.AppendAllText(logFile, $"[{DateTime.Now:yyyy-MM-dd HH:mm:ss}] {msg}{Environment.NewLine}"),
                Microsoft.Extensions.Logging.LogLevel.Information);
        }
    }

    private void SeedData(ModelBuilder modelBuilder)
    {
        //static date - edit
        var seedDate = new DateTime(2025, 11, 7, 12, 0, 0, DateTimeKind.Utc);

        //pre-generated
        modelBuilder.Entity<Contact>().HasData(
            new Contact { Id = 1, Name = "Alice Johnson", Email = "alice@example.com", Category = "Friends", DateAdded = seedDate, DateModified = seedDate },
            new Contact { Id = 2, Name = "Bob Novak", Email = "bob@work.com", Category = "Work", DateAdded = seedDate, DateModified = seedDate },
            new Contact { Id = 3, Name = "Charlie Smith", Email = "charlie.smith@family.net", Category = "Family", DateAdded = seedDate, DateModified = seedDate },
            new Contact { Id = 4, Name = "Diana Torres", Email = "diana.torres@fitnessclub.com", Category = "Gym", DateAdded = seedDate, DateModified = seedDate },
            new Contact { Id = 5, Name = "Ethan Green", Email = "ethan.green@work.com", Category = "Work", DateAdded = seedDate, DateModified = seedDate },
            new Contact { Id = 6, Name = "Fiona Wells", Email = "fiona.wells@gmail.com", Category = "Friends", DateAdded = seedDate, DateModified = seedDate },
            new Contact { Id = 7, Name = "George Brown", Email = "george.brown@consulting.biz", Category = "Business", DateAdded = seedDate, DateModified = seedDate },
            new Contact { Id = 8, Name = "Hannah Lee", Email = "hannah.lee@school.edu", Category = "Work", DateAdded = seedDate, DateModified = seedDate },
            new Contact { Id = 9, Name = "Ivan Petrov", Email = "ivan.petrov@mail.ru", Category = "Friends", DateAdded = seedDate, DateModified = seedDate },
            new Contact { Id = 10, Name = "Julia Meyer", Email = "julia.meyer@musiclover.com", Category = "Hobby", DateAdded = seedDate, DateModified = seedDate }
        );
        modelBuilder.Entity<PhoneNumber>().HasData(
            new PhoneNumber { Id = 1, Number = "+420111111111", ContactId = 1, Default = true, DateAdded = seedDate, DateModified = seedDate },
            new PhoneNumber { Id = 2, Number = "+420222222222", ContactId = 2, Default = true, DateAdded = seedDate, DateModified = seedDate },
            new PhoneNumber { Id = 3, Number = "+420333333333", ContactId = 3, Default = true, DateAdded = seedDate, DateModified = seedDate },
            new PhoneNumber { Id = 4, Number = "+420444444444", ContactId = 4, Default = true, DateAdded = seedDate, DateModified = seedDate },
            new PhoneNumber { Id = 5, Number = "+420555555555", ContactId = 5, Default = true, DateAdded = seedDate, DateModified = seedDate },
            new PhoneNumber { Id = 6, Number = "+420666666666", ContactId = 6, Default = true, DateAdded = seedDate, DateModified = seedDate },
            new PhoneNumber { Id = 7, Number = "+420777777777", ContactId = 7, Default = true, DateAdded = seedDate, DateModified = seedDate },
            new PhoneNumber { Id = 8, Number = "+420888888888", ContactId = 8, Default = true, DateAdded = seedDate, DateModified = seedDate },
            new PhoneNumber { Id = 9, Number = "+420999999999", ContactId = 9, Default = true, DateAdded = seedDate, DateModified = seedDate },
            new PhoneNumber { Id = 10, Number = "+420101010101", ContactId = 10, Default = true, DateAdded = seedDate, DateModified = seedDate },
            new PhoneNumber { Id = 11, Number = "+420101010101", ContactId = 10, Default = false, DateAdded = seedDate, DateModified = seedDate }

        );
    }
    public override int SaveChanges()
    {
        foreach (var entry in ChangeTracker.Entries<Contact>())
        {
            if (entry.State == EntityState.Added)
                entry.Entity.DateAdded = DateTime.UtcNow;
            if (entry.State == EntityState.Modified || entry.State == EntityState.Added)
                entry.Entity.DateModified = DateTime.UtcNow;
        }
        foreach (var entry in ChangeTracker.Entries<PhoneNumber>())
        {
            if (entry.State == EntityState.Added)
                entry.Entity.DateAdded = DateTime.UtcNow;
            if (entry.State == EntityState.Modified || entry.State == EntityState.Added)
                entry.Entity.DateModified = DateTime.UtcNow;
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
