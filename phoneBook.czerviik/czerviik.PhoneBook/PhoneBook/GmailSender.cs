namespace PhoneBook;

using Google.Apis.Auth.OAuth2;
using MailKit.Net.Smtp;
using MailKit.Security;
using Microsoft.Identity.Client.Platforms.Features.DesktopOs.Kerberos;
using MimeKit;
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
    public async Task SendMailAsync(string receiverAdress)
    {
        var credential = await GoogleAuthService.GetGmailCredentialsAsync(); //presunout do program.cs tam inicializovat googleauthservice a gmailsender pomoci credentials a username

        var message = new MimeMessage();
        message.From.Add(new MailboxAddress("PhoneBook App", _userEmail));
        message.To.Add(new MailboxAddress(receiverAdress));
    }

}
