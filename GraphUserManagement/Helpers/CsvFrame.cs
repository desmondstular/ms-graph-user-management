// /========================================================================
// File: CsvFrame.cs
// Purpose:
// Author: Des
// Created: 02-18-2025
// ========================================================================

namespace GraphUserManagement.Helpers;

public class CsvFrame
{
    public List<string>? Headers { get; set; }
    private List<string[]>? Data { get; set; }

    public CsvFrame(string? csvPath = null)
    {
        // If CSV path supplied, call read function
        if (csvPath != null)
        {
            try
            {
                ReadCsv(csvPath);
            }
            catch (Exception ex)
            {
                throw new System.IO.FileNotFoundException();
            }
        }
        else
        {
            this.Headers = [];
            this.Data = [];
        }
    }

    public void ReadCsv(string csvPath)
    {
        var hasHeaders = false;
        this.Headers = [];
        this.Data = [];
        
        using (var reader = new StreamReader(csvPath))
        {
            while (reader.EndOfStream == false)
            {
                var content = reader.ReadLine();
                var cells = content?.Split(',');
                
                // If row has contains values, add to Frame
                if (cells?.Length > 0)
                {
                    // Store data if headers not already stored
                    if (hasHeaders)
                    {
                        this.Data.Add(cells);
                    }
                    else
                    {
                        hasHeaders = true;
                        this.Headers = cells.ToList();
                    }
                }
            }
        }
    }
}