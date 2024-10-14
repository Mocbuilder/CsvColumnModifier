using Microsoft.VisualBasic.FileIO;
using System;
using System.Collections.Generic;
using System.IO;

namespace CsvColumnModifier
{
    internal class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("Enter the path to the input CSV file:");
            string inputFilePath = Console.ReadLine();

            Console.WriteLine("Enter the path for the output CSV file:");
            string outputFilePath = Console.ReadLine();

            Console.WriteLine("Enter the index of the column to modify (0-based):");
            int columnToModify;
            while (!int.TryParse(Console.ReadLine(), out columnToModify))
            {
                Console.WriteLine("Invalid input. Please enter a valid column index (0-based):");
            }

            List<string[]> csvData = LoadCSV(inputFilePath);
            ModifyColumn(csvData, columnToModify);
            WriteCSV(outputFilePath, csvData);

            Console.WriteLine("Processing complete. The modified CSV has been saved.");
            Console.ReadLine();
        }

        public static List<string[]> LoadCSV(string filePath)
        {
            List<string[]> csvData = new List<string[]>();

            using (TextFieldParser parser = new TextFieldParser(filePath))
            {
                parser.TextFieldType = FieldType.Delimited;
                parser.SetDelimiters(",");

                //skip header row
                if (!parser.EndOfData)
                {
                    parser.ReadLine();
                }

                // Read all rows
                while (!parser.EndOfData)
                {
                    string[] fields = parser.ReadFields();
                    csvData.Add(fields);
                }
            }

            return csvData;
        }

        public static void ModifyColumn(List<string[]> csvData, int columnIndex)
        {
            for (int i = 0; i < csvData.Count; i++)
            {
                // Ensure the row has enough columns
                if (csvData[i].Length > columnIndex)
                {
                    string originalValue = csvData[i][columnIndex];
                    if (!string.IsNullOrEmpty(originalValue))
                    {
                        // Add ':' after every second character
                        csvData[i][columnIndex] = InsertColonEverySecondChar(originalValue);
                    }
                }
            }
        }

        public static string InsertColonEverySecondChar(string input)
        {
            string modified = string.Empty;
            for (int i = 0; i < input.Length; i++)
            {
                modified += input[i]; // Append the current character
                if ((i + 1) % 2 == 0 && i < input.Length - 1) // Check if it's the second character
                {
                    modified += ':'; // Add ':' after every second character
                }
            }
            return modified;
        }

        public static void WriteCSV(string filePath, List<string[]> data)
        {
            using (StreamWriter writer = new StreamWriter(filePath))
            {
                foreach (var fields in data)
                {
                    writer.WriteLine(string.Join(",", fields));
                }
            }
        }
    }
}
