using System.Threading.Tasks;
using Spectre.Console;

namespace PhoneBook;

public abstract class Menu
{
    protected MenuManager MenuManager { get; }

    protected Menu(MenuManager menuManager)
    {
        MenuManager = menuManager;
    }

    public abstract void Display();
}
public class MainMenu : Menu
{
    public MainMenu(MenuManager menuManager) : base(menuManager) { }

    public override void Display()
    {
        UserInterface.MainMenu();
        HandleUserOptions();
    }

    private void HandleUserOptions()
    {
        switch (UserInterface.OptionChoice)
        {
            case MenuOptions.ShowAllContacts:
                MenuManager.NewMenu(new AllContactsMenu(MenuManager));
                break;
            case MenuOptions.AddContact:
                MenuManager.NewMenu(new AddContactMenu(MenuManager));
                break;
            case MenuOptions.Exit:
                MenuManager.Close();
                break;
            default:
                MenuManager.Close();
                break;
        }
    }
}

public class AllContactsMenu : Menu
{
    private List<Contact> contacts;
    private PageModifier pageMod = PageModifier.None;
    public AllContactsMenu(MenuManager menuManager) : base(menuManager) { }
    public override void Display()
    {
        contacts = ContactsController.GetContacts();
        UserInterface.AllContactsMenu(contacts, pageMod);
        HandleUserOptions();
    }

    private void HandleUserOptions()
    {
        switch (UserInterface.OptionChoice)
        {
            case MenuOptions.NextPage:
                pageMod = PageModifier.Increase;
                MenuManager.DisplayCurrentMenu();
               break;
            case MenuOptions.PreviousPage:
                pageMod = PageModifier.Decrease;
                MenuManager.DisplayCurrentMenu();
                break;
            case MenuOptions.SendEmail:
                MenuManager.NewMenu(new SendEmail(MenuManager));
                break;
            case MenuOptions.AddContact:
                ContactsController.AddContact();
                UserInterface.DisplayMessage("Contact added.");
                MenuManager.DisplayCurrentMenu();
                break;
            case MenuOptions.EditContact:
                MenuManager.NewMenu(new EditContactMenu(MenuManager));
                MenuManager.DisplayCurrentMenu();
                break;
            case MenuOptions.Back:
                MenuManager.GoBack();
                break;
            default:
                MenuManager.Close();
                break;
        }
    }
}
public class AddContactMenu : Menu
{
    public AddContactMenu(MenuManager menuManager) : base(menuManager) { }

    public override void Display()
    {
        ContactsController.AddContact();
        UserInterface.DisplayMessage("Contact added.", "return to main menu");
        MenuManager.GoBack();
    }
}
public class EditContactMenu : Menu
{
    internal int Id { get; }
    internal Contact Contact { get; set; }
    public EditContactMenu(MenuManager menuManager) : base(menuManager)
    {
        while (true)
        {
            Id = AnsiConsole.Ask<int>("Enter the contact's ID: ");
            if (ContactsController.IdExists(Id) == true) break;
            else UserInterface.DisplayMessage("Wrong ID.");
        }
    }
    public override void Display()
    {
        Contact = ContactsController.GetContactById(Id);
        UserInterface.ContactEditMenu(Contact);
        HandleUserOptions();
    }
    private void HandleUserOptions()
    {
        switch (UserInterface.OptionChoice)
        {
            case MenuOptions.EditName:
                ContactsController.EditName(Contact);
                MenuManager.DisplayCurrentMenu();
                break;
            case MenuOptions.EditEmail:
                ContactsController.EditEmail(Contact);
                MenuManager.DisplayCurrentMenu();
                break;
            case MenuOptions.EditPhone:
                var contactsNumbers = ContactsController.GetContactNumbers(Contact);
                UserInterface.AllContactNumbers(contactsNumbers);

                var userNumber = contactsNumbers.FirstOrDefault(p => p.Number == UserInterface.NumberChoice);
                if (userNumber != null)
                    ContactsController.EditPhone(Contact, userNumber);
                MenuManager.DisplayCurrentMenu();
                break;
            case MenuOptions.AddPhone:
                ContactsController.AddPhone(Contact);
                MenuManager.DisplayCurrentMenu();
                break;
            case MenuOptions.DeletePhone:
                contactsNumbers = ContactsController.GetContactNumbers(Contact);
                UserInterface.AllContactNumbers(contactsNumbers);
                userNumber = contactsNumbers.FirstOrDefault(p => p.Number == UserInterface.NumberChoice);
                if (userNumber != null)
                {
                    if (UserInterface.ConfirmDelete())
                        ContactsController.DeletePhone(userNumber);
                }
                MenuManager.DisplayCurrentMenu();
                break;
            case MenuOptions.DeleteContact:
                if (Contact != null)
                {
                    if (UserInterface.ConfirmDelete())
                        ContactsController.DeleteContact(Contact);
                }
                MenuManager.GoBack();
                break;
            case MenuOptions.Back:
                MenuManager.GoBack();
                break;
            case MenuOptions.Exit:
                MenuManager.Close();
                break;
            default:
                MenuManager.Close();
                break;
        }
    }
}
public class SendEmail : Menu
{
    internal int Id { get; }

    public SendEmail(MenuManager menuManager) : base(menuManager)
    {
        while (true)
        {
            Id = AnsiConsole.Ask<int>("Enter the contact's ID: ");
            if (ContactsController.IdExists(Id) == true) break;
            else UserInterface.DisplayMessage("Wrong ID.");
        }
    }

    public override void Display()
    {
        UserInterface.SendEmailMenu(ContactsController.GetContactById(Id));
    }
}