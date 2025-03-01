﻿// ========================================================================
// File: Program.cs
// Purpose: This file contains the main program for this console app which
//          can be used to do user management on AzureAD.
// Author: Desmond Stular
// Created: 2025-02-16
// ========================================================================

using GraphUserManagement.Configuration;
using GraphUserManagement.Helpers;
using Microsoft.Graph.Models;

namespace GraphUserManagement;

internal class Program
{
    private static async Task Main(string[] args)
    {
        Console.WriteLine("AzureAD User Management Console App");
        
        // Load settings, initialize app graph, and then get token.
        var settings = Settings.LoadSettings();
        AppGraph.InitializeAppGraph(settings);

        var choice = -1;

        while (choice != 0)
        {
            Console.WriteLine("Please select one of the following options:");
            Console.WriteLine("0: Exit");
            Console.WriteLine("1: Display App Token");
            Console.WriteLine("2: Display Users");
            Console.Write("\nChoice: ");

            try
            {
                choice = int.Parse(Console.ReadLine() ?? string.Empty);
            }
            catch (System.FormatException)
            {
                // Set to a not valid value
                choice = -1;
            }

            switch (choice)
            {
                case 0:
                    Console.WriteLine("Goodbye for now!");
                    System.Environment.Exit(0);
                    break;
                    
                case 1:
                    await DisplayAppTokenAsync();
                    break;
                
                case 2:
                    await ListUsersAsync();
                    break;
                
                default:
                    Console.WriteLine("Invalid choice. Please select again.");
                    break;
            }
        }
    }

    private static async Task DisplayAppTokenAsync()
    {
        try
        {
            var appToken = await AppGraph.GetAppTokenAsync();
            Console.WriteLine($"App Token: {appToken[..25]}...");
        }
        catch (Exception ex)
        {
            Console.WriteLine(ex.Message);
        }
    }

    private static async Task ListUsersAsync()
    {
        try
        {
            var allUsers = true;
            
            // Get users from graph
            List<User>? users = await AppGraph.GetUsersAsync(allUsers:allUsers);

            if (users.Count == 0)
            {
                Console.WriteLine("There are no users.");
                return;
            }
            
            // If requested to retrieve all users, only display user count
            if (allUsers)
            {
                Console.WriteLine($"There are {users.Count} users.");
            }
            else // display all users usernames and user principal names
            {
                foreach (var user in users)
                {
                    Console.WriteLine($"\tUsername: {user.DisplayName}; UserPrincipalName: {user.UserPrincipalName}");
                }
            }
            
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error getting users: {ex.Message}");
        }
    }
}