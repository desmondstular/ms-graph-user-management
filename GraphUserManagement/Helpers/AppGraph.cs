// /========================================================================
// File: AppGraph.cs
// Purpose: Contains API call methods to the Microsoft Graph service.
// Author: Desmond Stular
// Created: 02-16-2025
// ========================================================================

using Azure.Identity;
using GraphUserManagement.Configuration;
using Microsoft.Graph;

namespace GraphUserManagement.Helpers;

/// <summary>
/// Helper class used for authenticating into Microsoft Graph and making a variety of API calls.
/// </summary>
public static class AppGraph
{
    private static Settings? _settings;
    private static ClientSecretCredential? _clientSecretCredential;
    private static GraphServiceClient? _appClient;

    /// <summary>
    /// Initializes the Microsoft Graph service client for app-only authentication using provided settings.
    /// </summary>
    /// <param name="settings">Settings object containing the tenantId, clientId, and client secret used for
    /// authentication.</param>
    /// <exception cref="NullReferenceException">Thrown when the settings could not be loaded from the configuration
    /// file or user secrets.</exception>
    public static void InitializeAppGraph(Settings settings)
    {
        // Ensure settings are not null
        _settings = settings ??
                    throw new System.NullReferenceException("Settings cannot be null");
        
        // Create client secret credential
        _clientSecretCredential = new ClientSecretCredential(
            _settings.TenantId, _settings.ClientId, _settings.ClientSecret);
        
        // Create graph client; uses default scope
        _appClient = new GraphServiceClient(_clientSecretCredential,
            ["https://graph.microsoft.com/.default"]);
    }
}