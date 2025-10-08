namespace PhoneBook;

using Spectre.Console;
using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;
using System.Reflection.Metadata.Ecma335;

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

    internal static string GetUserEmail(ContactsContext context)
    {
        string email;
        while (true)
        {
            email = AnsiConsole.Ask<string>("Contact's email").Trim();
            if (Utils.ValidateEmail(email) && !context.Contacts.AsNoTracking().Any(c => c.Email == email))
                break;
            else
                Console.WriteLine("Invalid or already used email.");
        }
        return email;
    }
    internal static string GetUserName() => AnsiConsole.Ask<string>("Contact's name: ").Trim();
    internal static string GetUserPhone()
    {
        string phoneNum;
        while (true)
        {
            phoneNum = AnsiConsole.Ask<string>("Contact's telephone number (with prefix)").Replace(" ", "");
            if (ValidatePhone(phoneNum))
                break;
            else
                Console.WriteLine("Invalid phone number.");
        }
        return phoneNum;
    }
}
