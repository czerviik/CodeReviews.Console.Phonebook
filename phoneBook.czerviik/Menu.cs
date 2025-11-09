using Spectre.Console;
using MimeKit;
using Google.Apis.Auth.OAuth2;

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
    public MainMenu(MenuManager menuManager) : base(menuManager){}
    public override void Display()
    {
        UserInterface.ContactsPage = 1;
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
            case MenuOptions.DevMode:
                Program.EfSqlLoggingEnabled = !Program.EfSqlLoggingEnabled;
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
public class AllContactsMenu : Menu
{
    private List<Contact> contacts;
    public AllContactsMenu(MenuManager menuManager) : base(menuManager){}
    public override void Display()
    {
        contacts = ContactsController.GetContacts();
        UserInterface.AllContactsMenu(contacts);
        HandleUserOptions();
    }
    private void HandleUserOptions()
    {
        switch (UserInterface.OptionChoice)
        {
            case MenuOptions.NextPage:
                UserInterface.PageMod = PageModifier.Increase;
                MenuManager.DisplayCurrentMenu();
               break;
            case MenuOptions.PreviousPage:
                UserInterface.PageMod = PageModifier.Decrease;
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
        UserInterface.AddContactMenu();
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
            case MenuOptions.EditCategory:
                ContactsController.EditCategory(Contact);
                MenuManager.DisplayCurrentMenu();
                break;
            case MenuOptions.EditEmail:
                ContactsController.EditEmail(Contact);
                MenuManager.DisplayCurrentMenu();
                break;
            case MenuOptions.EditPhone:
                var contactsNumbers = Contact.PhoneNumbers.ToList();
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
                contactsNumbers = Contact.PhoneNumbers.ToList();
                UserInterface.AllContactNumbers(contactsNumbers);
                userNumber = contactsNumbers.FirstOrDefault(p => p.Number == UserInterface.NumberChoice);
                if (userNumber != null)
                {
                    if (UserInterface.Confirm("Do you really want to delete?"))
                        ContactsController.DeletePhone(userNumber);
                }
                MenuManager.DisplayCurrentMenu();
                break;
            case MenuOptions.DeleteContact:
                if (Contact != null)
                {
                    if (UserInterface.Confirm("Do you really want to delete?"))
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
    private int Id { get; }
    private readonly UserCredential _credentials;
    public SendEmail(MenuManager menuManager) : base(menuManager)
    {
        _credentials = GoogleAuthService.GetGmailCredentialsAsync().GetAwaiter().GetResult(); 

        while (true)
        {
            Id = AnsiConsole.Ask<int>("Enter the contact's ID: ");
            if (ContactsController.IdExists(Id) == true) break;
            else UserInterface.DisplayMessage("Wrong ID.");
        }
    }
    public override void Display()
    {
        var contact = ContactsController.GetContactById(Id);
        UserInterface.SendEmailMenu(contact);
        ConstructEmail(contact).GetAwaiter().GetResult();
    }
    private async Task ConstructEmail(Contact contact)
    {
        {
            string receiverAddress = contact.Email;
            var message = new MimeMessage();

            message.To.Add(new MailboxAddress(receiverAddress));
            message.Subject = "A message from PhoneBook";

            message.Body = new TextPart("plain")
            {
                Text = AnsiConsole.Prompt(
                    new TextPrompt<string>("Message: ")
                        .PromptStyle("green")
                )
            };
            if (UserInterface.Confirm("Send email?"))
            {
                var gmailSender = new GmailSender(_credentials);
                await gmailSender.SendMailAsync(message);
                UserInterface.DisplayMessage("Message sent!", "go back");
                MenuManager.GoBack();
            }
        }
    }
}