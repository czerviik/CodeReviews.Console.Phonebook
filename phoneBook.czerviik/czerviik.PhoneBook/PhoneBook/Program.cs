using System.Threading.Tasks;
using Google.Apis.Auth.OAuth2;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using Microsoft.VisualBasic;

namespace PhoneBook;

internal class Program
{
    public static UserCredential? Credentials { get; private set; }

    private static async Task Main(string[] args)
    {
        var menuManager = new MenuManager();
        Credentials = await GoogleAuthService.GetGmailCredentialsAsync(); //QST je to tady spravne?

        try
        {
            using var contactsContext = new ContactsContext();
            menuManager.DisplayCurrentMenu();
        }
        catch (InvalidOperationException ex)
        {
            UserInterface.DisplayMessage($"EF Operation error: {ex.Message}", "exit", true);
            Environment.Exit(1);
        }
        catch (DbUpdateException ex)
        {
            UserInterface.DisplayMessage($"Database update failed: {ex.Message}", "return to main menu", true);
            menuManager.ReturnToMainMenu();
        }
        catch (SqlException ex)
        {
            UserInterface.DisplayMessage($"SQL Connection error: {ex.Message}", "return to main menu", true);
            menuManager.ReturnToMainMenu();
        }
        catch (Exception ex)
        {
            UserInterface.DisplayMessage($"Unexpected error: {ex.Message}", "exit", true);
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
    EditPhone,
    EditCategory,
    SendEmail,
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