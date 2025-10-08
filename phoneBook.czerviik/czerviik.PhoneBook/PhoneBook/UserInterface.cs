using System.ComponentModel.DataAnnotations;
using System.Reflection;
using Azure;
using Microsoft.VisualBasic;
using Spectre.Console;

namespace PhoneBook;

public static class UserInterface
{
    internal static MenuOptions OptionChoice { get; private set; }
    const int MAX_ROWS = 4;
    internal static int page = 1;
    public static void MainMenu()
    {
        Header("phone book");

        MenuOptions[] options = {
                MenuOptions.ShowAllContacts,
                MenuOptions.AddContact,
                MenuOptions.Exit};

        ChooseOptions(options);
    }
    public static void AllContactsMenu(List<Contact> contacts, PageModifier pageMod = PageModifier.None)
    {
        if (pageMod == PageModifier.Increase) page += 1;
        else if(pageMod == PageModifier.Decrease) page -= 1;

        Header("contacts");
        DisplayContactTable(contacts,page);
        var options = new List<MenuOptions> {
                MenuOptions.AddContact,
                MenuOptions.EditContact,
                MenuOptions.DeleteContact,
                MenuOptions.Back};
        
        if (contacts.Count > MAX_ROWS && page > 1) 
            options.Insert(0,MenuOptions.PreviousPage);
        if (contacts.Count > MAX_ROWS && page < (double)contacts.Count/MAX_ROWS) 
            options.Insert(0,MenuOptions.NextPage);

        ChooseOptions(options.ToArray());

        
    }
    public static void ContactEditMenu(List<Contact> contact)
    {
        page = 1;
        Header("edit contact");
        DisplayContactTable(contact, page);

        var options = new List<MenuOptions> {
                MenuOptions.EditName,
                MenuOptions.EditEmail,
                MenuOptions.EditPhone,
                MenuOptions.AddPhone,
                MenuOptions.DefaultPhone,
                MenuOptions.DeletePhone,
                MenuOptions.Back
        };

        ChooseOptions(options.ToArray());

    }
    public static void AllContactNumbers(List<PhoneNumber> phoneNumbers)
    {
        List<string> numbersList = new ();
        foreach (PhoneNumber number in phoneNumbers)
        {
            numbersList.Add(number.Number);
        }
        numbersList.Add("Back"); //dokoncit logiku vybirani cisel

        var numberChoice = AnsiConsole.Prompt(
            new SelectionPrompt<string>()
            .HighlightStyle("yellow")
            .AddChoices(numbersList)
            );
    }
    internal static void DisplayContactTable(List<Contact> contacts, int page)
    {
        var table = new Table()
        .Border(TableBorder.Rounded);

        FormatTableData(contacts, table, page);
        AnsiConsole.Write(table);
    }

    internal static void FormatTableData(List<Contact> contacts, Table table, int page)
        {
            table        
            .AddColumns("Id", "Name","E-mail","Phone number","Date created","Date modified")
            .Border(TableBorder.Rounded);
            int startingRow = (page * MAX_ROWS)-MAX_ROWS;


            for (int i = startingRow; i < startingRow + MAX_ROWS && i < contacts.Count; i++)
            {

                table.AddRow(contacts[i].Id.ToString(),
                            contacts[i].Name,
                            contacts[i].Email,
                            contacts[i].PhoneNumbers.FirstOrDefault(p => p.Default)?.Number,
                            contacts[i].DateAdded.ToString("yyyy-MM-dd, HH:mm:ss"),
                            contacts[i].DateModified.ToString("yyyy-MM-dd, HH:mm:ss")
                            );
            }
        }
    public static void DefaultNumber(string phoneNum)
    {

        Console.WriteLine($"Is {phoneNum} the default number?");
        MenuOptions[] options = {
                MenuOptions.Yes,
                MenuOptions.No};

        ChooseOptions(options);
    }

    private static void Header(string headerText)
    {
        Console.Clear();
        Console.WriteLine($"----- {headerText.ToUpper()} -----");
        Console.WriteLine();
    }

    private static void ChooseOptions(MenuOptions[] options)
    {
        OptionChoice = AnsiConsole.Prompt(
            new SelectionPrompt<MenuOptions>()
            .HighlightStyle("yellow")
            .AddChoices(options)
            );
    }


    public static void DisplayMessage(string message = "", string actionMessage = "continue", bool consoleClear = false)
    {
        if (consoleClear) Console.Clear();

        if (message == "")
            Console.WriteLine($"\nPress any key to {actionMessage}...");
        else
            Console.WriteLine($"\n{message} Press any key to {actionMessage}...");

        Console.ReadKey();
    }
}