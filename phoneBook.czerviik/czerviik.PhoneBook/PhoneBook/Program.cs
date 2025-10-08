namespace PhoneBook;

internal class Program
{
    private static void Main(string[] args)
    {
        try
        {
            var contactsContext = new ContactsContext();
            var menuManager = new MenuManager();

            menuManager.DisplayCurrentMenu();
        }
        catch (InvalidOperationException ex)
        {
            Console.WriteLine($"Error:{ex.Message}");
            Environment.Exit(1);
        }
    }

}

public enum MenuOptions
{
    ShowAllContacts,
    AddContact,
    EditContact,
    DeleteContact,
    Back,
    Yes,
    No,
    NextPage,
    PreviousPage,
    EditName,
    EditEmail,
    AddPhone,
    DeletePhone,
    DefaultPhone,
    EditPhone,
    EditCategory,
    Exit
}
public enum PageModifier
{
    Increase,
    Decrease,
    None
}
public enum EditProperty
{
    Name,
    Email,
    Phone,
    Category
}