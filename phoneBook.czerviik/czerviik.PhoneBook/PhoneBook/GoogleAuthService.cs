namespace PhoneBook;
using Google.Apis.Auth.OAuth2;
using Google.Apis.Util.Store;
using System.Threading;
using System.Threading.Tasks;
public static class GoogleAuthService
{
    public static async Task<UserCredential> GetGmailCredentialsAsync()
    {
        using var stream = new FileStream("credentials.json", FileMode.Open, FileAccess.Read);
        string[] scopes = { "https://mail.google.com/auth/gmail.send" };
        var credPath = "token.json";

        return await GoogleWebAuthorizationBroker.AuthorizeAsync(
            GoogleClientSecrets.FromStream(stream).Secrets,
            scopes,
            "user",
            CancellationToken.None,
            new FileDataStore(credPath, true));
    }
        //     public static GetUserEmail()
    // {
    //     var gmail = new GmailService(new BaseClientService.Initializer
    //     {
    //         HttpClientInitializer = credential,
    //         ApplicationName = "PhoneBook App"
    //     });

    //     var profile = await gmail.Users.GetProfile("me").ExecuteAsync();
    //     string userEmail = profile.EmailAddress;
    // }

}
