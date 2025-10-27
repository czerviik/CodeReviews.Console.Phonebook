namespace PhoneBook;

using Google.Apis.Auth.OAuth2;
using MailKit.Net.Smtp;
using MailKit.Security;
using Microsoft.Identity.Client.Platforms.Features.DesktopOs.Kerberos;
using MimeKit;
using Spectre.Console;
using System.Threading.Tasks;
public class GmailSender
{
    private readonly UserCredential _credential;
    private readonly string _userEmail;

    public GmailSender(UserCredential credential, string userEmail)
    {
        _credential = credential;
        _userEmail = userEmail;
    }
    public async Task SendMailAsync(Contact contact)
    {
        string receiverAddress = contact.Email;
        var message = new MimeMessage();
        
        message.From.Add(new MailboxAddress("PhoneBook App", _userEmail));
        message.To.Add(new MailboxAddress(receiverAddress));
        message.Subject = "A message from PhoneBook";
        message.Body = new TextPart("plain")
        {
            Text = AnsiConsole.Prompt(
                new TextPrompt<string>("Message: ")
                    .PromptStyle("green")
            )//QST dodelat odesilani, predtim jeste potvrzeni odeslani.
        };
    }

}
