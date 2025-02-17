// /========================================================================
// File: AppGraph.cs
// Purpose: Contains API call methods to the Microsoft Graph service.
// Author: Desmond Stular
// Created: 02-16-2025
// ========================================================================

using Azure.Identity;
using Microsoft.Graph;

namespace GraphUserManagement.Helpers;

public class AppGraph
{
    private static Settings? _settings;
    private static ClientSecretCredential? _clientSecretCredential;
    private static GraphServiceClient? _appClient;

    public AppGraph(Settings settings)
    {
        // Ensure settings are not null
        _settings = settings ??
                    throw new System.NullReferenceException("Settings cannot be null");
        
        // Create client secret credential
        _clientSecretCredential = new ClientSecretCredential(
            _settings.TenantId, _settings.ClientId, _settings.ClientSecret);
        
        // Create graph client; uses default scope
        _appClient = new GraphServiceClient(_clientSecretCredential,
                new[] {"https://graph.microsoft.com/.default"});
    }
}