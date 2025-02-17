// ========================================================================
// File: Settings.cs
// Purpose: This file contains the Settings class used to load configuration 
//          values from the settings json for accessing Microsoft Graph.
// Author: Desmond Stular
// Created: 2025-02-16
// ========================================================================


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