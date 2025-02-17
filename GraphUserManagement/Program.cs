// ========================================================================
// File: Program.cs
// Purpose: This file contains the main program for this console app which
//          can be used to do user management on AzureAD.
// Author: Desmond Stular
// Created: 2025-02-16
// ========================================================================

using GraphUserManagement.Configuration;
using GraphUserManagement.Helpers;

namespace GraphUserManagement;

internal class Program
{
    private static void Main(string[] args)
    {
        Console.WriteLine("AzureAD User Management Console App");
        
        // Load settings and initialize app graph with settings
        var settings = Settings.LoadSettings();
        AppGraph.InitializeAppGraph(settings);

        Console.WriteLine("Goodbye for now!");
    }
}