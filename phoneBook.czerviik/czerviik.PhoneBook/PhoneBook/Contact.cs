namespace PhoneBook;

public class Contact
{
    public int Id { get; set; }
    public string Name { get; set; }
    public string Email { get; set; }
    public string? Category { get; set; }
    public DateTime DateAdded { get; set; }
    public DateTime DateModified { get; set; }
    public ICollection<PhoneNumber> PhoneNumbers { get; set; } = new List<PhoneNumber>();
}
