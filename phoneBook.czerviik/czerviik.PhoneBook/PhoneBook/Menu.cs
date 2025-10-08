using Azure;
using Microsoft.VisualBasic;
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
    private readonly List<Contact> _contacts = ContactsController.GetContacts();
    private PageModifier pageMod = PageModifier.None;
    public AllContactsMenu(MenuManager menuManager) : base(menuManager) { }
    public override void Display()
    {
        UserInterface.AllContactsMenu(_contacts, pageMod);
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
            case MenuOptions.AddContact:
                ContactsController.AddContact();
                UserInterface.DisplayMessage("Contact added.");
                MenuManager.DisplayCurrentMenu();
                break;
            case MenuOptions.EditContact:
                MenuManager.NewMenu(new EditContactMenu(MenuManager));
                break;
            case MenuOptions.DeleteContact:
                //MenuManager.NewMenu(new DeleteContactMenu(MenuManager));
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
    internal int Id { get; set; }
    internal List<Contact> Contacts { get; set; }
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
        Contacts = ContactsController.GetContactById(Id);
        UserInterface.ContactEditMenu(Contacts);
        HandleUserOptions();
    }
    private void HandleUserOptions()
    {
        switch (UserInterface.OptionChoice)
        {
            case MenuOptions.EditName:
                ContactsController.EditName(Id);
                MenuManager.DisplayCurrentMenu();
                break;
            case MenuOptions.EditEmail:
                ContactsController.EditEmail(Id);
                MenuManager.DisplayCurrentMenu();
                break;
            case MenuOptions.EditPhone:
                
                UserInterface.AllContactNumbers(ContactsController.GetContactNumbers(Contacts.FirstOrDefault(c => c.Id == Id)));
                //ContactsController.EditPhone(Id,);
                MenuManager.DisplayCurrentMenu();
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