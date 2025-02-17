// /========================================================================
// File: AppGraph.cs
// Purpose: Contains API call methods to the Microsoft Graph service.
// Author: Desmond Stular
// Created: 02-16-2025
// ========================================================================

using Azure.Core;
using Azure.Identity;
using GraphUserManagement.Configuration;
using Microsoft.Graph;
using Microsoft.Graph.Models;
using System.Linq;

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


    /// <summary>
    /// Retrieves an application authentication token for Microsoft Graph.
    /// </summary>
    /// <returns>A string containing the application token.</returns>
    /// <exception cref="NullReferenceException">Throws an error if Graph has not been setup for application only
    /// authentication.</exception>
    public static async Task<string> GetAppTokenAsync()
    {
        // Ensure credential is not null
        _ = _clientSecretCredential ??
            throw new System.NullReferenceException("Graph has not been initialized for app-only authentication.");

        // Request token with given scopes
        var context = new TokenRequestContext(["https://graph.microsoft.com/.default"]);
        var response = await _clientSecretCredential.GetTokenAsync(context);
        return response.Token;
    }

    
    /// <summary>
    /// Retrieves a list of users from the Microsoft graph API. By default, it only returns the top 25 but
    /// the parameter can be changed to include all users.
    /// </summary>
    /// <param name="selections">An array of strings specifying what attributes to return for each user.</param>
    /// <param name="allUsers">A boolean that if true, returns all users from Graph.</param>
    /// <param name="top">Specify how many users to return in single call.</param>
    /// <param name="orderBy">Select what attribute to order users list by.</param>
    /// <returns>A list of User models.</returns>
    /// <exception cref="NullReferenceException">Throws error if graph has not been initialized yet.</exception>
    public static async Task<List<User>> GetUsersAsync(
        string[]? selections = null, bool allUsers = false, int top = 25, string orderBy = "displayName")
    {
        // Ensure client is not null
        _ = _appClient ??
            throw new System.NullReferenceException("Graph has not been initialized for app-only authentication.");

        // If selections array empty, put select defaults
        selections = selections ?? ["id", "displayName", "userPrincipalName"];
        
        // Get first response
        var response = await _appClient.Users.GetAsync((config) =>
        {
            config.QueryParameters.Select = selections;
            config.QueryParameters.Top = top;
            config.QueryParameters.Orderby = selections.Contains(orderBy) ? [orderBy] : [selections[0]];
        });

        List<User> users = response?.Value ?? [];
        
        // If all users is true, keep sending odata url requests till all users returned
        if (allUsers)
        {
            // While there is a next link, get users
            while (!string.IsNullOrEmpty(response?.OdataNextLink))
            {
                response = await _appClient.Users.WithUrl(response?.OdataNextLink).GetAsync();
                users.AddRange(response?.Value ?? new List<User>(1));
            }
        }

        return users;
    }
}