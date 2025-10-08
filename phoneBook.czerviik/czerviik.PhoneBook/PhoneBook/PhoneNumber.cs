namespace PhoneBook;

public class PhoneNumber
{
    public int Id { get; set; }
    public string? Number { get; set; }
    public int ContactId { get; set; }
    public bool Default { get; set; } = true;
    public DateTime DateAdded { get; set; }
    public DateTime DateModified { get; set; }
    public Contact Contact { get; set; } = null!;
}
