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
    public AllContactsMenu(MenuManager menuManager) : base(menuManager) { }

    public override void Display()
    {
        Console.WriteLine("under constraction");
        // UserInterface.AllContactsMenu();
        // HandleUserOptions();
    }

    // private void HandleUserOptions()
    // {
    //     switch (UserInterface.OptionChoice)
    //     {
    //         case MenuOptions.ShowAllContacts:
    //             MenuManager.NewMenu(new AllMenu(MenuManager));
    //             break;
    //         case MenuOptions.Exit:
    //             MenuManager.NewMenu(new FavoritesMenu(MenuManager));
    //             break;
    //         default:
    //             MenuManager.Close();
    //             break;
    //     }
    // }
}

