using Spectre.Console;
using Microsoft.EntityFrameworkCore;

namespace PhoneBook;

public class ContactsController
{
    internal static void AddContact()
    {
      try
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
      catch (Exception ex) //podchytit connection exception
      {
            UserInterface.DisplayMessage($"Connection lost. Er:{ex.Message}","return to main menu",true);
      }
    }
    internal static void AddPhone(Contact contact)
    {
        using var context = new ContactsContext();
        var phoneNumber = new PhoneNumber()
        {
            Number = Utils.GetUserPhone(),
            ContactId = contact.Id
        };
        SetDefaultPhone(context, phoneNumber);
        context.PhoneNumbers.Add(phoneNumber);
        context.SaveChanges();
    }
    internal static void DeletePhone(PhoneNumber phoneNum)
    {
        using var context = new ContactsContext();
        if (phoneNum.Default)
            UserInterface.DisplayMessage("Cannot delete the default phone number.");
        else
        {        
            context.PhoneNumbers.Remove(phoneNum);
            context.SaveChanges();
            UserInterface.DisplayMessage("Phone number removed.");
        }
    }
    internal static void DeleteContact(Contact contact)
    {
        using var context = new ContactsContext();
        context.Contacts.Remove(contact);
        context.SaveChanges();
        UserInterface.DisplayMessage("Contact removed.");
    }
    internal static List<Contact> GetContacts()
    {
        using var context = new ContactsContext();
        var allContacts = context.Contacts
        .Include(c => c.PhoneNumbers)
        .ToList();
        return allContacts;
    }
    internal static Contact GetContactById(int id)
    {
        using var context = new ContactsContext();

        var contact = context.Contacts
        .Include(c => c.PhoneNumbers)
        .FirstOrDefault(c => c.Id == id);

        return contact;
    }
    internal static void EditName(Contact contact)
    {
        using var context = new ContactsContext();
        if (contact != null)
        {
            contact.Name = Utils.GetUserName(contact.Name);
            UserInterface.DisplayMessage("Contact's name modified.");
            context.Contacts.Update(contact);
            context.SaveChanges();
        }
        else
        {
            UserInterface.DisplayMessage("Contact couldn't be found.");
        }
    }
    internal static void EditEmail(Contact contact)
    {
        using var context = new ContactsContext();
        if (contact != null)
        {
            contact.Email = Utils.GetUserEmail(context,contact.Email);
            context.Contacts.Update(contact);
            context.SaveChanges();
        }
        else
        {
            UserInterface.DisplayMessage("Contact couldn't be found.");
        }
    }
    internal static void EditPhone(Contact contact, PhoneNumber phoneNumber)
    {

        using var context = new ContactsContext();
        if (contact != null)
        {
            if (phoneNumber != null)
            {
                phoneNumber.Number = Utils.GetUserPhone(phoneNumber.Number);
                if(!phoneNumber.Default)
                    SetDefaultPhone(context, phoneNumber);
                UserInterface.DisplayMessage("Contact's phone number modified.");
                //context.PhoneNumbers.Update(phoneNumber);
                context.SaveChanges();
            }
            else UserInterface.DisplayMessage("Phone number couldn't be found.");

        }
        else UserInterface.DisplayMessage("Contact couldn't be found.");
    }
        private static void SetDefaultPhone(ContactsContext context, PhoneNumber phoneNumber)
    {   
        if (UserInterface.Confirm("Mark this as the default number?"))
        {
            if (phoneNumber != null)
            {
                UndefaultPhoneNumbers(context,phoneNumber.ContactId);
                var phoneToUpdate = context.PhoneNumbers.First(p => p.Id == phoneNumber.Id);
                phoneToUpdate.Default = true;
            }
        }        
    }
    private static void UndefaultPhoneNumbers(ContactsContext context, int contactId)
    {
        var phoneNumbers = context.PhoneNumbers
        .Where(p => p.ContactId == contactId)
        .ToList();
        if (phoneNumbers.Any())
        {
            foreach (var phoneNum in phoneNumbers)
                phoneNum.Default = false;
        }
    }
    internal static List<PhoneNumber> GetContactNumbers(Contact contact)
    {
        using var context = new ContactsContext();
        return contact.PhoneNumbers
        .Where(p => p.ContactId == contact.Id)
        .ToList();
    }
    
    internal static bool IdExists(int id)
    {
        using var context = new ContactsContext();
        return context.Contacts.Any(c => c.Id == id);
    }

}
