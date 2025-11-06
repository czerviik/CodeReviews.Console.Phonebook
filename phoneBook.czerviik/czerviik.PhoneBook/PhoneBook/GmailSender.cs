namespace PhoneBook;

using Google.Apis.Auth.OAuth2;
using Google.Apis.Services;
using MimeKit;
using MailKit.Net.Smtp;
using MailKit.Security;
using Google.Apis.Gmail.v1;
using System.Threading.Tasks;
using MailKit;
public class GmailSender
{
    private readonly UserCredential _credentials;
    private readonly string _userEmail;
    public GmailSender(UserCredential credentials)
    {
        _credentials = credentials;
    }
    public async Task SendMailAsync(MimeMessage message)
    {
        var gmailService = new GmailService(new BaseClientService.Initializer
        {
            HttpClientInitializer = _credentials,
            ApplicationName = "PhoneBook"
        });

        try
        {
            var profile = await gmailService.Users.GetProfile("me").ExecuteAsync();
            message.From.Add(new MailboxAddress("PhoneBook App", profile.EmailAddress));

            if (_credentials.Token.IsExpired(_credentials.Flow.Clock))
            {
                await _credentials.RefreshTokenAsync(CancellationToken.None);
            }
            var logPath = Path.Combine(AppContext.BaseDirectory, "smtp.log");
            using var client = new SmtpClient(new ProtocolLogger(logPath));
            await client.ConnectAsync("smtp.gmail.com", 587, SecureSocketOptions.StartTls);

            var oauth2 = new SaslMechanismOAuth2(profile.EmailAddress, _credentials.Token.AccessToken);
            await client.AuthenticateAsync(oauth2);

            await client.SendAsync(message);
            await client.DisconnectAsync(true);
        }
        catch (Google.GoogleApiException ex)
        {
            Console.WriteLine($"Code: {ex.Error.Code}");
            Console.WriteLine($"Message: {ex.Error.Message}");
            throw;
        }
    }
}
