// Thanks MS: https://learn.microsoft.com/en-us/dotnet/api/system.io.directory.getfiles?view=net-10.0
// For Directory.GetFiles and Directory.GetDirectories
// For File.Exists, Directory.Exists
using System;
using System.IO;
using System.Collections;

public class RecursiveFileProcessor
{
    // Process all files in the directory passed in, recurse on any directories
    // that are found, and process the files they contain.
    public static List<string> ProcessDirectory(string targetDirectory, List<string>? recursedFiles = null)
    {
        // Initialize the list if it's null
        if (recursedFiles == null)
        {
            recursedFiles = new List<string>();
        }

        // Process the list of files found in the directory.
        string[] fileEntries = Directory.GetFiles(targetDirectory);
        foreach (string fileName in fileEntries)
        {
            string processedFile = ProcessFile(fileName);
            if (!String.IsNullOrEmpty(processedFile))
            {
                recursedFiles.Add(processedFile);
            }
        }

        // Recurse into subdirectories of this directory.
        string[] subdirectoryEntries = Directory.GetDirectories(targetDirectory);
        foreach (string subdirectory in subdirectoryEntries)
        {
            ProcessDirectory(subdirectory, recursedFiles);
        }

        return recursedFiles;
    }

    // Insert logic for processing found files here.
    public static string ProcessFile(string path)
    {
        if (Path.GetExtension(path).ToLower() != ".csv")
        {
            Console.WriteLine("Skipping non-.csv file at '{0}'.", path);
            return String.Empty;
        }
        Console.WriteLine("Found .csv file at '{0}'.", path);
        return path;
    }
}