using Microsoft.EntityFrameworkCore;

namespace PhoneBook;

internal class ContactsContext : DbContext
{
    public DbSet<Contact> Contacts { get; set; }
    private readonly ConfigReader cfg = new();
    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder) => optionsBuilder.UseSqlServer(cfg.GetConnectionString());
}
