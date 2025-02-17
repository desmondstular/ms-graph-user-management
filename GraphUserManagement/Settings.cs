using Microsoft.Extensions.Configuration;

namespace GraphUserManagement;

public class Settings
{
    public string? ClientId { get; set; }
    public string? ClientSecret { get; set; }
    public string? TenantId { get; set; }

    public static Settings LoadSettings()
    {
        // Load settings
        IConfiguration config = new ConfigurationBuilder()
            .AddJsonFile("appsettings.json", optional: false)
            .AddUserSecrets<Program>()
            .Build();

        return config.GetRequiredSection("settings").Get<Settings>() ??
               throw new Exception("Could not load app settings.");
    }
}