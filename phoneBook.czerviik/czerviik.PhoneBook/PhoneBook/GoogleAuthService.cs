namespace PhoneBook;
using Google.Apis.Auth.OAuth2;
using Google.Apis.Util.Store;
using System.Threading;
using System.Threading.Tasks;


public static class GoogleAuthService
{
    public static async Task<UserCredential> GetGmailCredentialsAsync() //vyresit google auth pri startu
    {
        var path = Environment.GetEnvironmentVariable("GOOGLE_CREDENTIALS_PATH");

    if (string.IsNullOrEmpty(path))
    {
        path = Path.Combine(AppContext.BaseDirectory, "..", "..", "..", "credentials.json");
    }

    if (!File.Exists(path))
    {
        Console.ForegroundColor = ConsoleColor.Yellow;
        Console.WriteLine("Missing credentials.json");
        Console.WriteLine("Please place your Google OAuth credentials file here:");
        Console.WriteLine($"  {Path.GetFullPath(path)}");
        Console.WriteLine("Or set environment variable GOOGLE_CREDENTIALS_PATH=/path/to/credentials.json");
        Console.ResetColor();
        throw new FileNotFoundException("Missing Google OAuth credentials file", path);
    }
        using var stream = new FileStream(path, FileMode.Open, FileAccess.Read);
        string[] scopes = { "https://mail.google.com/" };
        var tokenFolder = Path.Combine(AppContext.BaseDirectory, "..", "..", "..", "google-token");
        Directory.CreateDirectory(tokenFolder);

        return await GoogleWebAuthorizationBroker.AuthorizeAsync(
            GoogleClientSecrets.FromStream(stream).Secrets,
            scopes,
            "user",
            CancellationToken.None,
            new FileDataStore(tokenFolder, true));
    }
}
