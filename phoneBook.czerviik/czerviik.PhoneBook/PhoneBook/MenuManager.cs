using Google.Apis.Auth.OAuth2;
using Microsoft.Identity.Client.Platforms.Features.DesktopOs.Kerberos;

namespace PhoneBook;

public class MenuManager
{
    private readonly Stack<Menu> _menuStack = new Stack<Menu>();
    private readonly UserCredential _credentials;

    public MenuManager(UserCredential credentials)
    {
        _credentials = credentials;
        _menuStack.Push(new MainMenu(this, _credentials));
    }

    public void DisplayCurrentMenu()
    {
        if (_menuStack.Count > 0)
        {
            Menu currentMenu = _menuStack.Peek();
            currentMenu.Display();
        }
    }

    public void NewMenu(Menu menu)
    {
        _menuStack.Push(menu);
        DisplayCurrentMenu();
    }

    public void GoBack()
    {
        if (_menuStack.Count > 1)
            _menuStack.Pop();
        DisplayCurrentMenu();
    }

    public void ReturnToMainMenu()
    {
        while (_menuStack.Count > 1)
            _menuStack.Pop();
        DisplayCurrentMenu();
    }

    public void Close()
    {
        Console.Clear();
        Environment.Exit(0);
    }
}