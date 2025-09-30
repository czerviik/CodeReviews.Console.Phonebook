using Spectre.Console;

namespace PhoneBook;

public class ContactsController
{
    internal static void AddContact()
    {
        var name = AnsiConsole.Ask<string>("Contact's name: ").Trim();
        while (true)
        {
            var email = AnsiConsole.Ask<string>("Contact's email").Trim();
            if (Utils.ValidateEmail(email))
                break;
            else
                Console.WriteLine("Invalid email.");
        };
        while (true)
        {
            var phonenum = AnsiConsole.Ask<string>("Contact's telephone number (prefix)").Replace(" ", "");
            if (Utils.ValidatePhone(phonenum))
                break;
            else
                Console.WriteLine("Invalid email.");
        };
        using var db = new ContactsContext();
        db.Add(new Contact { Name = name });
    }
}
