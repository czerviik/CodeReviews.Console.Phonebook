namespace PhoneBook;

using Spectre.Console;
using Microsoft.EntityFrameworkCore;
using System;

public static class Utils
{
    internal static bool ValidateEmail(string email)
    {
        if (email.EndsWith('.'))
        {
            return false;
        }
        try
        {
            var addr = new System.Net.Mail.MailAddress(email);
            return addr.Address == email;
        }
        catch
        {
            return false;
        }
    }
    internal static bool ValidatePhone(string phonenum)
    {
        return phonenum.StartsWith('+') && long.TryParse(phonenum.AsSpan(1), out long _);
    }

    internal static string GetUserEmail(ContactsContext context, string email = "")
    {
        var isEdit = email != "";
        var originalEmail = email;
        while (true)
        {
            email = AnsiConsole.Prompt(
                new TextPrompt<string>("Contact's email (e. g. test@email.com): ")
                    .DefaultValue(email)
                    .PromptStyle("yellow")
            ).Trim();

            if (!ValidateEmail(email))
                Console.WriteLine("Invalid email (expected format: test@email.com): ");
            else if (!context.Contacts.AsNoTracking().Any(c => c.Email == email))
            {
                if (isEdit)
                    UserInterface.DisplayMessage("Contact's e-mail was modified.");
                
                break;
            }
            else if (isEdit)
            {
                UserInterface.DisplayMessage("Contact's e-mail was not modified.");
                break;
            }
            else
            {
                Console.WriteLine("Email already used.");
                email = originalEmail;
            }
        }
        return email;
    }
    internal static string GetUserName(string contactName = "")
    {
        return AnsiConsole.Prompt(
                new TextPrompt<string>("Contact's name: ")
                    .DefaultValue(contactName)
                    .PromptStyle("yellow")
            ).Trim();
    }
    internal static string GetUserPhone(string phoneNum = "")
    {
        var isEdit = phoneNum != "";

        while (true)
        {
            phoneNum = AnsiConsole.Prompt(
                new TextPrompt<string>("Contact's telephone number (e. g. +420987654321): ")
                    .DefaultValue(phoneNum)
                    .PromptStyle("yellow")
            ).Replace(" ", "");
            if (!ValidatePhone(phoneNum))
                Console.WriteLine("Invalid phone number (expected format: +420987654321): ");
            else
                break;
                
        }
        return phoneNum;
    }
    internal static string GetCategory()
    {
        return AnsiConsole.Prompt(new SelectionPrompt<Categories>()
        .Title("Select a category:")
        .HighlightStyle("yellow")
        .AddChoices(Enum.GetValues<Categories>())).ToString();
    }
}