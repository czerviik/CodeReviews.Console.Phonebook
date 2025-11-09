namespace PhoneBook;

using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
internal class Program
{
    internal static bool EfSqlLoggingEnabled { get; set; } = false;
    internal static string LogPath { get; set; } = Path.Combine(AppContext.BaseDirectory, "ef-sql.log");

    private static void Main(string[] args)
    {
        var menuManager = new MenuManager();

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
    DevMode,
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
public enum Categories
{
    None,
    Family,
    Work,
    Friends,
    Enemies
}