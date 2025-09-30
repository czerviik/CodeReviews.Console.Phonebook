namespace PhoneBook;

using Spectre.Console;
internal class Program
{
    private static void Main(string[] args)
    {
        // try
        // {
        //     var contactsContext = new ContactsContext();
        //     var menuManager = new MenuManager();

        //     menuManager.DisplayCurrentMenu();
        // }
        // catch (InvalidOperationException ex)
        // {
        //     Console.WriteLine($"Error:{ex.Message}");
        //     Environment.Exit(1);
        // }

        var name = AnsiConsole.Ask<string>("Contact's name: ");
        while (true)
        {
            var email = AnsiConsole.Ask<string>("Contact's email");
            if (Utils.ValidateEmail(email)) break;
            else
            {
                Console.WriteLine("Invalid email.");
            }
        };
        using var db = new ContactsContext();
        db.Add(new Contact { Name = name });
    }

}

   internal enum MenuOptions
    {
        ShowAllContacts,
        Exit
    }

// docker 