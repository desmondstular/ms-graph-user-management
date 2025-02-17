// ========================================================================
// File: Settings.cs
// Purpose: This file contains the Settings class used to load configuration 
//          values from the settings json for accessing Microsoft Graph.
// Author: Desmond Stular
// Created: 2025-02-16
// ========================================================================


using Microsoft.Extensions.Configuration;

namespace GraphUserManagement.Configuration;

/// <summary>
/// A configuration class that contains the client ID, tenant ID, and the client secret that is used to authenticate
/// into the Microsoft Graph API.
/// </summary>
public class Settings
{
    public string? ClientId { get; set; }
    public string? ClientSecret { get; set; }
    public string? TenantId { get; set; }

    /// <summary>
    /// 
    /// </summary>
    /// <returns>A <see cref="Settings"/> object populated with the values from the configuration file.</returns>
    /// <exception cref="Exception">Throws exception if the settings could not be loaded.</exception>
    public static Settings LoadSettings()
    {
        // Load settings json into configuration builder.
        IConfiguration config = new ConfigurationBuilder()
            .AddJsonFile("appsettings.json", optional: false)
            .AddUserSecrets<Program>()
            .Build();

        return config.GetRequiredSection("settings").Get<Settings>() ??
               throw new Exception("Could not load app settings.");
    }
}