using System.ComponentModel.DataAnnotations;
using System.Reflection;
using Azure;
using Microsoft.VisualBasic;
using Spectre.Console;
using System.Text.RegularExpressions;

namespace PhoneBook;

public static partial class UserInterface
{
    internal static MenuOptions OptionChoice { get; private set; }
    internal static string? NumberChoice { get; private set; }
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
                MenuOptions.Back};
        
        if (contacts.Count > MAX_ROWS && page > 1) 
            options.Insert(0,MenuOptions.PreviousPage);
        if (contacts.Count > MAX_ROWS && page < (double)contacts.Count/MAX_ROWS) 
            options.Insert(0,MenuOptions.NextPage);

        ChooseOptions(options.ToArray());

        
    }
    public static void ContactEditMenu(Contact contact)
    {
        Header("edit contact");
        DisplayContactTable(contact);

        var options = new List<MenuOptions> {
                MenuOptions.EditName,
                MenuOptions.EditEmail,
                MenuOptions.EditPhone,
                MenuOptions.AddPhone,
                MenuOptions.DeletePhone,
                MenuOptions.DeleteContact,
                MenuOptions.Back
        };

        ChooseOptions(options.ToArray());

    }
    public static void SendEmailMenu(Contact contact)
    {
        Header("send email");
        DisplayContactTable(contact);
    }
    
    public static void AllContactNumbers(List<PhoneNumber> phoneNumbers)
    {
        AnsiConsole.MarkupLine("EDIT PHONE NUMBER: ");

        List<string> numbersList = [];
        foreach (PhoneNumber phoneNumber in phoneNumbers)
        {
            var color = phoneNumber.Default ? "blue" : "white";
            numbersList.Add($"[{color}]{phoneNumber.Number}[/]");
        }
        numbersList.Add("[grey]Back[/]");

        NumberChoice = AnsiConsole.Prompt(
            new SelectionPrompt<string>()
            .HighlightStyle("yellow")
            .AddChoices(numbersList)
            );
        NumberChoice = MyRegex().Replace(NumberChoice, "");
    }
    internal static void DisplayContactTable(List<Contact> contacts, int page)
    {
        var table = new Table()
        .Border(TableBorder.Rounded);

        FormatTableData(contacts, table, page);
        AnsiConsole.Write(table);
    }
    internal static void DisplayContactTable(Contact contact, int page = 1)
    {
        var table = new Table()
        .Border(TableBorder.Rounded);

        FormatTableData(contact, table, page);
        AnsiConsole.Write(table);
    }

    internal static void FormatTableData(List<Contact> contacts, Table table, int page)
    {
        table
        .AddColumns("Id", "Name", "E-mail", "Phone number", "Date created", "Date modified")
        .Border(TableBorder.Rounded);
        int startingRow = (page * MAX_ROWS) - MAX_ROWS;


        for (int i = startingRow; i < startingRow + MAX_ROWS && i < contacts.Count; i++)
        {

            table.AddRow(contacts[i].Id.ToString(),
                        contacts[i].Name,
                        contacts[i].Email,
                        contacts[i].PhoneNumbers
                        .OrderByDescending(p => p.Default)
                        .ThenByDescending(p => p.DateAdded)
                        .FirstOrDefault()?.Number,
                        contacts[i].DateAdded.ToString("yyyy-MM-dd, HH:mm:ss"),
                        contacts[i].DateModified.ToString("yyyy-MM-dd, HH:mm:ss")
                        );
        }
    }
    internal static void FormatTableData(Contact contact, Table table, int page)
    {
        table
        .AddColumns("Id", "Name", "E-mail", "Phone number", "Date created", "Date modified")
        .Border(TableBorder.Rounded);
        int startingRow = (page * MAX_ROWS) - MAX_ROWS;

        table.AddRow(contact.Id.ToString(),
                    contact.Name,
                    contact.Email,
                    contact.PhoneNumbers
                    .OrderByDescending(p => p.Default)
                    .ThenByDescending(p => p.DateAdded)
                    .FirstOrDefault()?.Number,
                    contact.DateAdded.ToString("yyyy-MM-dd, HH:mm:ss"),
                    contact.DateModified.ToString("yyyy-MM-dd, HH:mm:ss")
                    );
    }
    public static bool ConfirmDefaultNumber()
    {
        Console.WriteLine("Mark this as the default number?");
        MenuOptions[] options = {
                MenuOptions.Yes,
                MenuOptions.No};

        ChooseOptions(options);
        return OptionChoice == MenuOptions.Yes;
    }
    public static bool ConfirmDelete()
    {
        Console.WriteLine("Do you really want to delete?");
        MenuOptions[] options = {
                MenuOptions.Yes,
                MenuOptions.No};

        ChooseOptions(options);
        return OptionChoice == MenuOptions.Yes;
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

    [GeneratedRegex(@"\[[^\]]+\]")]
    private static partial Regex MyRegex();
}