// /========================================================================
// File: CsvReader.cs
// Purpose:
// Author: Des
// Created: 02-18-2025
// ========================================================================

namespace GraphUserManagement.Helpers;

public static class CsvReader
{
    public static List<string[]> ReadCsv(string csvPath)
    {
        List<string[]> dataMatrix = [];
        
        using (var reader = new StreamReader(csvPath))
        {
            while (reader.EndOfStream == false)
            {
                var content = reader.ReadLine();
                var cells = content?.Split(',');
                
                // If row has data, add to data return var
                if (cells?.Length > 0)
                {
                    dataMatrix.Add(cells);
                }
            }
        }

        return dataMatrix;
    }
}