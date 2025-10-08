using Spectre.Console;
using Microsoft.EntityFrameworkCore;

namespace PhoneBook;

public class ContactsController
{
    internal static void AddContact()
    {
        using var context = new ContactsContext();
        var contact = new Contact
        {
            Name = Utils.GetUserName(),
            Email = Utils.GetUserEmail(context),
        };
        contact.PhoneNumbers.Add(new PhoneNumber {Number = Utils.GetUserPhone()});
        context.Add(contact);
        context.SaveChanges();
    }

    internal static List<Contact> GetContacts()
    {
        using var context = new ContactsContext();
        var allContacts = context.Contacts
        .Include(c => c.PhoneNumbers)
        .ToList();
        return allContacts;
    }
    internal static List<Contact> GetContactById(int id)
    {
        using var context = new ContactsContext();

        var contact = context.Contacts
        .Where(c => c.Id == id)
        .Include(c => c.PhoneNumbers)
        .ToList();

        return contact;
    }
    internal static void EditName(int id)
    {
        using var context = new ContactsContext();
        var contact = context.Contacts
        .FirstOrDefault(c => c.Id == id);
        if (contact != null)
        {
            contact.Name = Utils.GetUserName();
            UserInterface.DisplayMessage("Contact's name modified.");
            context.SaveChanges();
        }
        else
        {
            UserInterface.DisplayMessage("Contact couldn't be found.");
        }
    }
    internal static void EditEmail(int id)
    {
        using var context = new ContactsContext();
        var contact = context.Contacts
        .FirstOrDefault(c => c.Id == id);
        if (contact != null)
        {
            contact.Email = Utils.GetUserEmail(context);
            UserInterface.DisplayMessage("Contact's e-mail modified.");
            context.SaveChanges();
        }
        else
        {
            UserInterface.DisplayMessage("Contact couldn't be found.");
        }
    }
    internal static void EditPhone(int contactId, int phoneId)
    {
        using var context = new ContactsContext();
        var contact = context.Contacts
        .Include(c => c.PhoneNumbers)
        .FirstOrDefault(c => c.Id == contactId);
        if (contact != null)
        {
            var phoneToUpdate = contact.PhoneNumbers.FirstOrDefault(p => p.Id == phoneId);
            if (phoneToUpdate != null)
            {
                phoneToUpdate.Number = Utils.GetUserPhone();
                UserInterface.DisplayMessage("Contact's phone modified.");
                context.SaveChanges();
            }
            else UserInterface.DisplayMessage("Phone number couldn't be found.");

        }
        else UserInterface.DisplayMessage("Contact couldn't be found.");
    }
    internal static List<PhoneNumber> GetContactNumbers(Contact contact)
    {
        using var context = new ContactsContext();
        return contact.PhoneNumbers
        .Where(p => p.ContactId == contact.Id)
        .ToList();
    }
    // internal static void EditContact(int id,EditProperty editProperty)
    // {
    //     string propertyTxt;
    //     string propertyValue;

    //     using var context = new ContactsContext();
    //     var contact = context.Contacts
    //     .FirstOrDefault(c => c.Id == id);

    //     switch (editProperty)
    //     {
    //         case EditProperty.Name:
    //             propertyTxt = "name";
    //             contact.Name = Utils.GetUserName();
    //             break;
    //         case EditProperty.Email:
    //             contact.Email = "e-mail";
    //             propertyValue = Utils.GetUserEmail(context);
    //             break;
    //         case EditProperty.Phone:
    //             propertyTxt = "phone";
    //             contact.PhoneNumbers = Utils.GetUserPhone();
    //             break;
    //     }

    //     if (contact != null)
    //     {
    //         contact.Email = Utils.GetUserPhone();
    //         UserInterface.DisplayMessage("Contact's phone modified.");
    //         context.SaveChanges();
    //     }
    //     else
    //     {
    //         UserInterface.DisplayMessage("Contact couldn't be found.");
    //     }
    // }
    internal static bool IdExists(int id)
    {
        using var context = new ContactsContext();
        return context.Contacts.Any(c => c.Id == id);
    }

}
