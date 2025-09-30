using Microsoft.Extensions.Configuration;

namespace PhoneBook;

public class ConfigReader
{
    public IConfigurationRoot Configuration { get; }

    public ConfigReader()
    {
        Configuration = new ConfigurationBuilder()
            .SetBasePath(Directory.GetCurrentDirectory())
            .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true)
            .Build();
    }

#nullable enable
    public string GetConnectionString()
    {
        string? configString = Configuration.GetConnectionString("PhoneBookDb");
        if (string.IsNullOrEmpty(configString))
        {
            throw new InvalidOperationException("Connection string 'PhoneBookDb'is not configured.");
        }
        return configString;
    }
}